using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;
using System;

namespace ArcadeIdle
{
    public class HamBurgerMachine : MonoBehaviour
    {
        public float GrowTime;

        [BoxGroup("LINKS")]
        [SerializeField]
        private Transform[] _objectSpawnPoints;

        #region ____________Use_On_Player_Enter____________

        [InfoBox("Delay is only used for OnEnterPlacement.")]
        [BoxGroup("DELAY")]
        [SerializeField]
        private bool _useDelay;
        [BoxGroup("DELAY")]
        [ShowIf("_useDelay")]
        [SerializeField]
        private float _delayTime = 0.1f;
        [BoxGroup("ANIMATION")]
        [SerializeField]
        private AnimationType _itemType;
        private AnimationState _animationState;

        [BoxGroup("Capacity According To Level")]
        [SerializeField]
        private int[] _capacity;
        [BoxGroup("Capacity According To Level")]
        [SerializeField]
        CanvasGroup _MaxText;

        //Private Data Fields

        private PickDropAnimation pickDropAnimation;
        private GameObject player;
        private GameObject employee;

        private int playerCapacity;
        private int burgerFactoryCapacity;

        #endregion ____________Use_On_Player_Enter____________

        [BoxGroup("SETTINGS")]
        [SerializeField]
        private GameObject _objectPrefab;
        [BoxGroup("SETTINGS")]
        [SerializeField]
        private GameObject _objectPrefabToAnimate;
        [BoxGroup("Money Pack")]
        [SerializeField]
        private bool _moneyPackAvailable;
        [BoxGroup("Money Pack")]
        [ShowIf("_moneyPackAvailable")]
        [SerializeField]
        private MoneyPack moneyPack;

        //Private Data fields

        private readonly List<GameObject> _spawnedBurger = new List<GameObject>();

        public int SpawnBurgerCount
        {
            get => _spawnedBurger.Count;
        }

        // Start is called before the first frame update
        void Start()
        {
            OnUnlock();
            SubscribeEvents();
        }
        private void SubscribeEvents()
        {
            var trigger = GetComponent<Trigger>();
            //Subscribes trigger events by script because it more safety
            //Events in inspector can accidentally resets and you'll to set it again
            trigger.OnEnterTrigger.AddListener(OnPlayerEnter);
            trigger.OnExitTrigger.AddListener(OnPlayerExit);
            pickDropAnimation = GetComponent<PickDropAnimation>();
        }
        private void OnUnlock()
        {
            if (_itemType == AnimationType.PickItem)
                StartCoroutine(SpawnLoop());
        }
        /// <summary>
        /// The method is called when the player enters the placement's collider.
        /// </summary>
        /// <param name="player">Link to object entered in collider.</param>
        public void OnPlayerEnter(GameObject gameObject)
        {
            if (gameObject.tag == "Player")
            {
                this.player = gameObject;
                StartCoroutine(WaitDelay(() =>
                {
                    ManageItemsByPlayer();
                }));
            }
            else
            {
                employee = gameObject;
                var navMeshAgent = employee.GetComponent<NavMeshAgent>();
                var employeeAnimator = employee.GetComponent<Animator>();
                navMeshAgent.enabled = false;
                employeeAnimator.SetBool("Walk", false);
                employeeAnimator.SetBool("Idle", true);
                var delay = GrowTime / _objectSpawnPoints.Length;

            }
        }
        /// <summary>
        /// The method is called when the player exits the placement's collider.
        /// </summary>
        /// <param name="player">Link to object left the collider.</param>
        public void OnPlayerExit(GameObject player)
        {
            if (_itemType.Equals(AnimationType.PickItem))
            {
                StopAllCoroutines(); //Stops WaitDelay coroutine 
                StartCoroutine(SpawnLoop());// Restart Spawn loop to manage 
            }
        }
        private IEnumerator WaitDelay(Action onComplete)
        {
            var time = 0f;
            var _delayTime = GrowTime / _objectSpawnPoints.Length;

            while (time < _delayTime)
            {
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            onComplete?.Invoke();
        }

        private IEnumerator SpawnLoop()
        {
            var delay = GrowTime / _objectSpawnPoints.Length;

            while (true)
            {
                //StartCoroutine(SmoothTimer());
                if (_spawnedBurger.Count < burgerFactoryCapacity)
                    HideMaxText();
                burgerFactoryCapacity = CapacityCount();
                while (_spawnedBurger.Count != _objectSpawnPoints.Length && _spawnedBurger.Count < burgerFactoryCapacity)
                {
                    yield return new WaitForSeconds(delay);

                    SpawnObject();
                }

                ShowMaxText();
                //yield return new WaitWhile(() => _spawnedBurger.Count != 0);
                yield return new WaitWhile(() => _spawnedBurger.Count >= burgerFactoryCapacity);
                delay = _delayTime;
            }
        }
        private void SpawnObject()
        {
            var freshObject = Instantiate(_objectPrefab, _objectSpawnPoints[_spawnedBurger.Count]);
            _spawnedBurger.Add(freshObject);
        }
        #region _____Max_Text_Canvas_Group______
        private void ShowMaxText()
        {
            _MaxText.alpha = 1f;
        }
        private void HideMaxText()
        {
            _MaxText.alpha = 0f;
        }
        #endregion
        private void OnPickup(GameObject gameObject)
        {
            if (gameObject != null)
            {
                player.GetComponent<Trigger>().OnEnterTrigger?.Invoke(gameObject);
                _spawnedBurger.Remove(gameObject);
                Destroy(gameObject);
            }
        }
        private int CapacityCount()
        {
            int capacityLevel = UnityEngine.Random.Range(0, _capacity.Length);
            return _capacity[capacityLevel];
        }

        private void ManageItemsByPlayer()
        {
            SpecialBackpack playerBackpack = player.GetComponent<PlayerPicker>()._specialBackpack;
            if (playerBackpack.ItemsCount == 0)
                StartCoroutine(AddItemsToPlayerPack());
        }
        private IEnumerator AddItemsToPlayerPack()
        {
            PlayerPicker playerPicker = player.GetComponent<PlayerPicker>();
            SpecialBackpack playerBackpack = playerPicker._specialBackpack;

            playerCapacity = 1;
            while (SpawnBurgerCount > 0 && !playerBackpack.IsBackpackFull() && playerBackpack.ItemsCount < playerCapacity)
            {
                _animationState = AnimationState.Running;
                playerPicker.HideMaxText();
                var burger = Instantiate(_objectPrefabToAnimate, this.transform).transform;
                pickDropAnimation.ParabolicAnimation(burger, player.transform, _itemType, ChangeAnimationState);

                yield return new WaitWhile(() => _animationState != AnimationState.Complete);
                //yield return new WaitForSeconds(delay);
                OnPickup(_spawnedBurger[SpawnBurgerCount - 1]);
                yield return new WaitForEndOfFrame();
            }

            if (playerBackpack.ItemsCount == playerCapacity)
                playerPicker.ShowMaxText();
            yield return null;
        }

        private void ChangeAnimationState()
        {
            _animationState = AnimationState.Complete;
        }
    }
}

using System.Collections;
using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;

namespace ArcadeIdle.Shan
{
    public class BallsMachine : MonoBehaviour
    {
        [BoxGroup("Ball Prefab")]
        [SerializeField] GameObject _ballPrefab;
        [BoxGroup("Ball Prefab To Animate")]
        [SerializeField] GameObject _ballPrefabToAnimate;
        [BoxGroup("Ball Prefab")]
        [SerializeField] Transform _spawnPoint;
        [BoxGroup("Ball Prefab Count")]
        [SerializeField] int _ballsCount;

        [BoxGroup("Wait Time")]
        [SerializeField] float _waitTime;
        [SerializeField] Transform _ballMakingMachine;

        private List<GameObject> ballsGenerated = new List<GameObject>();
        private GameObject player;
        private AnimateBalls _Animation;


        private int SpawnBallsCount
        {
            get => ballsGenerated.Count;
        }
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(SpawnLoop());
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            var trigger = GetComponent<Trigger>();
            trigger.OnEnterTrigger.AddListener(OnPlayerEnter);
            trigger.OnExitTrigger.AddListener(OnPlayerExit);
            _Animation = GetComponent<AnimateBalls>();
        }
        private IEnumerator SpawnLoop()
        {
            Rigidbody ballRigidBody = null;
            while (true)
            {
                yield return new WaitWhile(() => !SaveSystem.Instance.Data.UnlockCounter);
                yield return new WaitWhile(() => _ballsCount <= ballsGenerated.Count);
                yield return new WaitForSeconds(_waitTime);
                ballRigidBody = SpawnNumberPrefab().GetComponent<Rigidbody>();
                ballRigidBody.useGravity = true;
            }
        }

        private GameObject SpawnNumberPrefab()
        {
            var ball = Instantiate(_ballPrefab, transform);
            ballsGenerated.Add(ball);
            ball.transform.position = _spawnPoint.position;
            ball.transform.rotation = _spawnPoint.rotation;
            ball.SetActive(true);
            return ball;
        }


        private void OnPlayerEnter(GameObject gameObject)
        {
            if (gameObject.tag == "Player")
            {
                this.player = gameObject;
                PickupBallsByPlayer();
                bool shown = SaveSystem.Instance.Data.PickupBallsShown;
                bool UnlockShown = SaveSystem.Instance.Data.UnlockCounter;
                if (!shown && UnlockShown)
                {
                    SaveSystem.Instance.Data.PickupBallsShown = true;
                    SaveSystem.Instance.SaveData();
                    PlayerController.Instance.ShowNextTargetNavmesh(_ballMakingMachine);
                }
            }
        }
        private void OnPlayerExit(GameObject gameObject)
        {
            if (gameObject.tag == "Player")
            {
                StopAllCoroutines();
                StartCoroutine(SpawnLoop());

            }
        }
        private void PickupBallsByPlayer()
        {
            StartCoroutine(AddBallsToPlayerPack());
        }
        private IEnumerator AddBallsToPlayerPack()
        {
            PlayerPicker playerPicker = player.GetComponent<PlayerPicker>();
            SpecialBackpack playerBackpack = playerPicker._specialBackpackForBalls;

            int playerCapacity = playerPicker.PlayerCapacityCount();
            while(playerBackpack.ItemsCount < playerCapacity)
            {
                while (SpawnBallsCount > 0 && playerBackpack.ItemsCount < playerCapacity)
                {
                    AnimationState _animationState = AnimationState.Running;
                    playerPicker.HideMaxText();
                    ballsGenerated[SpawnBallsCount - 1].SetActive(false);
                    var ball = Instantiate(_ballPrefabToAnimate, this.transform).transform;

                    _Animation.ParabolicAnimation(ball, transform, playerPicker.AnimationEndPoint(), () => {

                        OnPickupPackage(ballsGenerated[SpawnBallsCount - 1]);
                        _animationState = AnimationState.Complete;
                    });

                    yield return new WaitWhile(() => _animationState != AnimationState.Complete);
                    yield return new WaitForEndOfFrame();
                }
                if(playerBackpack.ItemsCount < playerCapacity)
                    yield return new WaitForSeconds(2f);
            }

            if (playerBackpack.ItemsCount == playerCapacity)
                playerPicker.ShowMaxText();
            yield return null;
        }

        private void OnPickupPackage(GameObject package)
        {
            if (package != null)
            {
                player.GetComponent<PlayerPicker>().TryPickUpBalls(_ballPrefab);
                ballsGenerated.Remove(package);
                Destroy(package.gameObject);
            }
        }
    }
}


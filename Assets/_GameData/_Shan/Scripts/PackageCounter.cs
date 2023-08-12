using System.Collections;
using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;

namespace ArcadeIdle.Shan
{
    public class PackageCounter : MonoBehaviour
    {
        [BoxGroup("Basket")] [SerializeField] GameObject _filledBasketPrefab;
        [BoxGroup("Basket")] [SerializeField] Transform[] _packagePlacePoints;
        [BoxGroup("Use Event")] [SerializeField] PackageMaker _packageMaker;

        [SerializeField] Transform _gateAway;

        private readonly List<GameObject> _spawnedPackage = new List<GameObject>();
        private GameObject player;
        private AnimateBalls _Animation;

        private int SpawnPackageCount
        {
            get => _spawnedPackage.Count;
        }
        // Start is called before the first frame update
        void Start()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            var trigger = GetComponent<Trigger>();
            trigger.OnEnterTrigger.AddListener(OnPlayerEnter);
            _packageMaker.PackageCompleted += SpawnPackageOnCounter;
            _Animation = GetComponent<AnimateBalls>();
        }
        private void SpawnPackageOnCounter()
        {
            bool shown = SaveSystem.Instance.Data.PickUpPackage;
            if (!shown)
            {
                SaveSystem.Instance.Data.PickUpPackage = true;
                SaveSystem.Instance.SaveData();
                PlayerController.Instance.ShowNextTargetNavmesh(transform);
            }
            var package = Instantiate(_filledBasketPrefab, _packagePlacePoints[_spawnedPackage.Count]);
            package.gameObject.SetActive(true);
            _spawnedPackage.Add(package);
        }

        private void OnPlayerEnter(GameObject gameObject)
        {
            if (gameObject.tag == "Player")
            {
                this.player = gameObject;
                PickPackagesByPlayer();
            }
        }
        private void PickPackagesByPlayer()
        {
            if(SpawnPackageCount > 0)
                StartCoroutine(AddItemsToPlayerPack());
            bool shownCounter = SaveSystem.Instance.Data.ShownCounter;
            if (!shownCounter)
            {
                SaveSystem.Instance.Data.ShownCounter = true;
                bool shown = SaveSystem.Instance.Data.PickUpPackage;
                if (shown)
                {
                    PlayerController.Instance.ShowNextTargetNavmesh(_gateAway);
                }
                SaveSystem.Instance.SaveData();
            }

        }
        private IEnumerator AddItemsToPlayerPack()
        {
            PlayerPicker playerPicker = player.GetComponent<PlayerPicker>();
            SpecialBackpack playerBackpack = playerPicker._specialBackpack;

            int playerCapacity = playerPicker.PlayerCapacityCount();
            while (SpawnPackageCount > 0 && playerBackpack.ItemsCount < playerCapacity)
            {
                AnimationState _animationState = AnimationState.Running;
                playerPicker.HideMaxText();
                _spawnedPackage[SpawnPackageCount - 1].SetActive(false);
                var package = Instantiate(_filledBasketPrefab, this.transform).transform;

                _Animation.ParabolicAnimation(package, transform, playerPicker.AnimationEndPoint(), () => {

                    OnPickupPackage(_spawnedPackage[SpawnPackageCount - 1]);
                    _animationState = AnimationState.Complete;
                });

                yield return new WaitWhile(() => _animationState != AnimationState.Complete);
                yield return new WaitForEndOfFrame();
            }

            if (playerBackpack.ItemsCount == playerCapacity)
                playerPicker.ShowMaxText();
            yield return null;
        }

        private void OnPickupPackage(GameObject package)
        {
            if (package != null)
            {
                player.GetComponent<PlayerPicker>().TryPickUp(package);
                _spawnedPackage.Remove(package);
                Destroy(package);
            }
        }
    }
}

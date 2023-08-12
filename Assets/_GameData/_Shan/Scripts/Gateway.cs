using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

namespace ArcadeIdle.Shan
{
    public class Gateway : MonoBehaviour
    {
        [BoxGroup("LINKS")]
        [SerializeField]
        private Transform[] _packageSpawnPoints;

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
        [BoxGroup("Next Target")] [SerializeField] Transform _nextTarget;
        //Private Data Fields

        private AnimateBalls _Animation;

        private GameObject player;
        private GameObject vehicle;
        //private GameObject employee;

        private int playerCapacity;

        private int spawnNotes = 8;

        private bool _fillingStock = false;
        private bool _salingStock = false;

        #endregion ____________Use_On_Player_Enter____________

        [BoxGroup("SETTINGS")]
        [SerializeField]
        private GameObject _packagePrefab;
        [BoxGroup("SETTINGS")]
        [SerializeField]
        private GameObject _packagePrefabToAnimate;
        [BoxGroup("Money Pack")]
        [SerializeField]
        private bool _moneyPackAvailable;
        [BoxGroup("Money Pack")]
        [ShowIf("_moneyPackAvailable")]
        [SerializeField]
        private MoneyPack moneyPack;

        private readonly List<GameObject> _spawnedPackages = new List<GameObject>();

        // Change  CustomersToDeal customersToDeal;
        private Cashier cashier;

        public int SpawnPackageCount
        {
            get => _spawnedPackages.Count;
        }

        private bool vehicleTriggered;
        private bool playerTriggered;

        public bool VehicleTriggered
        {
            get => vehicleTriggered;
            set => vehicleTriggered = value;
        }
        public bool PlayerTriggered
        {
            get => playerTriggered;
            set => playerTriggered = value;
        }
        // Start is called before the first frame update
        void Start()
        {
            SubscribeEvents();

        }

        private void SubscribeEvents()
        {
            var trigger = GetComponent<Trigger>();
            trigger.OnEnterTrigger.AddListener(OnObjectEnter);
            trigger.OnExitTrigger.AddListener(OnObjectExit);
            _Animation = GetComponent<AnimateBalls>();
        }
        private void OnObjectEnter(GameObject gameObject)
        {
            if (gameObject.tag == "Vehicle")
            {
                vehicleTriggered = true; 
                this.vehicle = gameObject;
                StartCoroutine(WaitingQueue());
            }
            else if (gameObject.tag == "Player")
            {
                player = gameObject;
                playerTriggered = true;
                ManagePackagesByPlayer();
                bool UnlockCounter = SaveSystem.Instance.Data.UnlockCounter;
                if (!UnlockCounter)
                {
                    SaveSystem.Instance.Data.UnlockCounter = true;
                    PlayerController.Instance.ShowNextTargetNavmesh(_nextTarget);
                    SaveSystem.Instance.SaveData();
                }
            }
        }
        private void OnObjectExit(GameObject gameObject)
        {
            if (gameObject.tag == "Vehicle")
            {
                vehicleTriggered = false;
            }
            else if (gameObject.tag == "Player")
            {
                playerTriggered = false;
                _fillingStock = false;
            }
        }

        private void SpawnPackage()
        {
            var package = Instantiate(_packagePrefab, _packageSpawnPoints[SpawnPackageCount]);
            _spawnedPackages.Add(package);
        }

        private void ManagePackagesByPlayer()
        {
            SpecialBackpack playerBackpack = player.GetComponent<PlayerPicker>()._specialBackpack;
            if (playerBackpack.ItemsCount > 0 && playerBackpack.ShowedItemsType == _packagePrefab.GetComponent<Fruit>().resourceType)
            {
                StartCoroutine(AddPackagesToCounterByPlayer());
                bool gatewayCounter = SaveSystem.Instance.Data.AddPackagesToGateway;
                if (!gatewayCounter)
                {
                    SaveSystem.Instance.Data.AddPackagesToGateway = true;
                    PlayerController.Instance.DisableNavmeshDraw();
                    SaveSystem.Instance.SaveData();
                }
            }
        }
        /// <summary>
        /// Items add at counter
        /// </summary>
        /// <returns></returns>
        private IEnumerator AddPackagesToCounterByPlayer()
        {
            PlayerPicker playerPicker = player.GetComponent<PlayerPicker>();
            SpecialBackpack playerBackpack = playerPicker._specialBackpack;

            int packageCount = playerPicker.PlayerItemsCount();

            yield return new WaitWhile(() => _salingStock);

            yield return new WaitForSeconds(_delayTime);
            while (packageCount > 0 && SpawnPackageCount < _packageSpawnPoints.Length)
            {
                _fillingStock = true;
                _animationState = AnimationState.Running;
                var package = Instantiate(_packagePrefabToAnimate, this.transform).transform;

                playerBackpack.RemoveItems(1);
                _Animation.ParabolicAnimation(package, playerPicker.AnimationEndPoint(), _packageSpawnPoints[_spawnedPackages.Count], ()=> {

                    SpawnPackage();
                    _animationState = AnimationState.Complete;
                });

                yield return new WaitWhile(() => _animationState != AnimationState.Complete);
                playerPicker.HideMaxText();
                packageCount = playerPicker.PlayerItemsCount();
            }
            if (playerBackpack.ItemsCount < playerCapacity)
                playerPicker.HideMaxText();
            packageCount = playerPicker.PlayerItemsCount();
            if (packageCount == 0)
            {
                playerPicker.PlayerAnimatorUpdate(false);
            }

            yield return null;
            _fillingStock = false;
        }

        private void StopWalkingVehicle()
        {
            var navMeshAgent = vehicle.GetComponent<NavMeshAgent>();
            navMeshAgent.enabled = false;
        }
        private IEnumerator WaitingQueue()
        {
            yield return new WaitWhile(() => _salingStock);
            VehicleGateway vehicleGateway = vehicle.GetComponent<VehicleGateway>();

            StartCoroutine(SaleItems(vehicleGateway));
            yield return null;
        }
        #region ________________Sale Items_____________
        private IEnumerator SaleItems(VehicleGateway vehicle)
        {
            SpecialBackpack vehicleBackpack = vehicle._specialBackpack;

            var vehicleCapacity = vehicle.CapacityToFillOrder;

            yield return new WaitWhile(() => _fillingStock);

            while (!vehicleBackpack.IsBackpackFull() && vehicleBackpack.ItemsCount != vehicleCapacity)
            {
                yield return new WaitForSeconds(1.5f);
                while (SpawnPackageCount > 0 && vehicleBackpack.ItemsCount < vehicleCapacity && !_fillingStock/* && Cashier._cashierAvailable*/)
                {
                    _salingStock = true;
                    _animationState = AnimationState.Running;
                    var package = Instantiate(_packagePrefabToAnimate, this.transform).transform;
                    _spawnedPackages[SpawnPackageCount - 1].SetActive(false);

                    _Animation.ParabolicAnimation(package, _packageSpawnPoints[_spawnedPackages.Count], vehicleBackpack.transform, () => {

                        vehicle.PurchaseItem(_spawnedPackages[SpawnPackageCount - 1]);
                        OnSale(_spawnedPackages[SpawnPackageCount - 1]);
                        _animationState = AnimationState.Complete;
                    });

                    yield return new WaitWhile(() => _animationState != AnimationState.Complete);
                    yield return new WaitForEndOfFrame();
                }
                _salingStock = false;
                vehicle.ShowRequirementText((vehicleBackpack.ItemsCount < vehicleCapacity),  vehicleCapacity - vehicleBackpack.ItemsCount);
            }
            vehicle.OrderStatus = OrderStatus.Full;
            vehicle.tag = "Finish";
            yield return new WaitForSeconds(1f);

            vehicle.OrderCompleted();

            if(_moneyPackAvailable)
                moneyPack.AnimateNotes(spawnNotes);
        }

        private void OnSale(GameObject package)
        {
            if (package != null)
            {
                _spawnedPackages.Remove(package);
                Destroy(package);
            }
        }
        #endregion
    }
}

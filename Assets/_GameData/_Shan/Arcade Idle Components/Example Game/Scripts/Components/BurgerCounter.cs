using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

namespace ArcadeIdle
{
    public class BurgerCounter : MonoBehaviour
    {
        [BoxGroup("LINKS")]
        [SerializeField]
        private Transform[] _burgerSpawnPoints;

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

        //Private Data Fields

        private PickDropAnimation pickDropAnimation;

        private GameObject player;
        private GameObject customer;
        private GameObject employee;

        private int playerCapacity;

        private int spawnNotes = 5;

        private bool _fillingStock = false;
        private bool _salingStock = false;

        #endregion ____________Use_On_Player_Enter____________

        [BoxGroup("SETTINGS")]
        [SerializeField]
        private GameObject _burgerPrefab;
        [BoxGroup("SETTINGS")]
        [SerializeField]
        private GameObject _burgerPrefabToAnimate;
        [BoxGroup("Money Pack")]
        [SerializeField]
        private bool _moneyPackAvailable;
        [BoxGroup("Money Pack")]
        [ShowIf("_moneyPackAvailable")]
        [SerializeField]
        private MoneyPack moneyPack;
        //Private Data fields

        private readonly List<Fruit> _spawnedBurger = new List<Fruit>();
        private Queue<GameObject> customerQueue = new Queue<GameObject>();

        CheckAvailableTables checkAvailableTables;
        CustomersToDeal customersToDeal;

        public int SpawnBurgerCount
        {
            get => _spawnedBurger.Count;
        }

        // Start is called before the first frame update
        void Start()
        {
            SubscribeEvents();
        }
        private void SpawnBurger()
        {
            var fruit = Instantiate(_burgerPrefab, _burgerSpawnPoints[_spawnedBurger.Count]).GetComponent<Fruit>();
            _spawnedBurger.Add(fruit);
        }
        #region _______Pick&DropBurgers________
        private void SubscribeEvents()
        {
            var trigger = GetComponent<Trigger>();
            //Subscribes trigger events by script because it more safety
            //Events in inspector can accidentally resets and you'll to set it again
            trigger.OnEnterTrigger.AddListener(OnPlayerEnter);
            trigger.OnExitTrigger.AddListener(OnPlayerExit);
            pickDropAnimation = GetComponent<PickDropAnimation>();
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
                ManageItemsByPlayer();
            }
            else if (gameObject.tag == "Customer" && !_salingStock)
            {
                this.customer = gameObject;
                StopWalkingCustomer();
            }
            else if (gameObject.tag == "Employee")
            {
                this.employee = gameObject;
                StopWalkingEmployee();
            }
        }
        private void StopWalkingCustomer()
        {
            var navMeshAgent = customer.GetComponent<NavMeshAgent>();
            var customerAnimator = customer.GetComponent<Animator>();
            navMeshAgent.enabled = false;
            customerAnimator.SetBool("Walk", false);
            customerAnimator.SetBool("Idle", true);
            customerQueue.Enqueue(customer);
            StartCoroutine(WaitingQueue());
        }
        private void StopWalkingEmployee()
        {
            var navMeshAgent = employee.GetComponent<NavMeshAgent>();
            var employeeAnimator = employee.GetComponent<Animator>();
            navMeshAgent.enabled = false;
            employeeAnimator.SetBool("StackWalk", false);
            employeeAnimator.SetBool("StackIdle", true);
        }
        private IEnumerator WaitingQueue()
        {
            while (customerQueue.Count == 1)
            {
                yield return new WaitWhile(() => _salingStock);
                GameObject customerObject = customerQueue.Dequeue();
                customersToDeal.customers.Remove(customerObject);
                StartCoroutine(SaleItems(customerObject.GetComponent<Customer>()));
            }
            yield return null;
        }

        /// <summary>
        /// The method is called when the player exits the placement's collider.
        /// </summary>
        /// <param name="player">Link to object left the collider.</param>
        public void OnPlayerExit(GameObject player)
        {
            _fillingStock = false;
        }

        private void ManageItemsByPlayer()
        {
            SpecialBackpack playerBackpack = player.GetComponent<PlayerPicker>()._specialBackpack;
            if (playerBackpack.ItemsCount > 0 && playerBackpack.ShowedItemsType == _burgerPrefab.GetComponent<Fruit>().resourceType)
                StartCoroutine(AddItemsToCounterByPlayer());
        }

        /// <summary>
        /// Items add at counter
        /// </summary>
        /// <returns></returns>
        private IEnumerator AddItemsToCounterByPlayer()
        {
            
            SpecialBackpack playerBackpack = player.GetComponent<PlayerPicker>()._specialBackpack;

            PlayerPicker playerPicker = player.GetComponent<PlayerPicker>();
            int fruitCount = playerPicker.PlayerItemsCount();
            fruitCount += _spawnedBurger.Count;


            while (_spawnedBurger.Count != fruitCount && _spawnedBurger.Count < _burgerSpawnPoints.Length)
            {
                _fillingStock = true;
                _animationState = AnimationState.Running;
                var burger = Instantiate(_burgerPrefabToAnimate, this.transform).transform;
                pickDropAnimation.ParabolicAnimation(burger, player.transform, _itemType, ChangeAnimationState, playerBackpack);

                yield return new WaitWhile(() => _animationState != AnimationState.Complete);
                playerPicker.HideMaxText();
                SpawnBurger();
            }
            if (playerBackpack.ItemsCount < playerCapacity)
                playerPicker.HideMaxText();
            fruitCount = playerPicker.PlayerItemsCount();
            if (fruitCount == 0)
            {
                playerPicker.PlayerAnimatorUpdate(false);
            }

            yield return null;
            _fillingStock = false;
        }
       
        #endregion

        #region ____Sale Items_____
        private IEnumerator SaleItems(Customer customer)
        {
            _salingStock = true;
            SpecialBackpack customerBackpack = customer._specialBackpack;

            var customerCapacity = customer.CapacityToEat;

            while (!customerBackpack.IsBackpackFull() && customerBackpack.ItemsCount != customerCapacity)
            {
                yield return new WaitForSeconds(_delayTime);
                while (SpawnBurgerCount > 0 && customerBackpack.ItemsCount < customerCapacity && !_fillingStock && Cashier._cashierAvailable)
                {
                    _animationState = AnimationState.Running;
                    AnimationType animationType = AnimationType.Sale;
                    var burger = Instantiate(_burgerPrefabToAnimate, this.transform).transform;
                    pickDropAnimation.ParabolicAnimation(burger, this.customer.transform, animationType, ChangeAnimationState);

                    yield return new WaitWhile(() => _animationState != AnimationState.Complete);
                    OnSale(customer, _spawnedBurger[SpawnBurgerCount - 1]);
                    yield return new WaitForEndOfFrame();
                }
                customer.ShowRequirementText((customerBackpack.ItemsCount < customerCapacity));
            }
            customer.HungerStatus = CustomerHungerStatus.Holding;

            yield return new WaitForSeconds(1f);
            bool tabeleAssigned = false;
            tabeleAssigned = checkAvailableTables.AssignAvailableTabels(this.customer);
            customer.tag = tabeleAssigned ? "CustomerStack" : "Customer";

            customer.ShowNoSeatText(!tabeleAssigned);
            while (tabeleAssigned != true)
            {
                tabeleAssigned = checkAvailableTables.AssignAvailableTabels(this.customer);
                customer.tag = tabeleAssigned ? "CustomerStack" : "Customer";
                customer.ShowNoSeatText(!tabeleAssigned);
                _salingStock = !tabeleAssigned;
                yield return new WaitForSeconds(3f);
            }
            _salingStock = false;
            moneyPack.SpawnSquareFormation(spawnNotes);
            //customer.ShowNoSeatText(!tabeleAssigned);
            //customer.tag = "CustomerStack";
            yield return new WaitForSeconds(1.5f);
            customersToDeal.MoveAllCustomerToCounter();
        }

        private void OnSale(Customer customer, Fruit fruit)
        {
            if (fruit != null)
            {
                customer.PurchaseItem(fruit.gameObject);
                _spawnedBurger.Remove(fruit);
                Destroy(fruit.gameObject);
            }
        }

        private void ChangeAnimationState()
        {
            _animationState = AnimationState.Complete;
        }
        #endregion
    }
}

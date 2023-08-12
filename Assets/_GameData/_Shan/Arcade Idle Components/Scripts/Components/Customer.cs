using NaughtyAttributes;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

namespace ArcadeIdle
{
    public class Customer : MonoBehaviour
    {
        /// <summary>
        /// Events Declaration
        /// </summary>
        public delegate void OnCustomerDestroy(GameObject gameObject);
        public event OnCustomerDestroy DestroyCustomer;

        [BoxGroup("Customer Backpack")]
        public SpecialBackpack _specialBackpack;
        [BoxGroup("Capacity To Eat")]
        [SerializeField]
        private int[] _Capacity;
        [BoxGroup("Hunger Status")]
        [SerializeField]
        private CustomerHungerStatus hungerStatus;
        [BoxGroup("CanvasGroup Text")]
        [SerializeField] CanvasGroup _noSeatText;
        [BoxGroup("CanvasGroup Text")]
        [SerializeField] CanvasGroup _requiredText;
        [BoxGroup("CanvasGroup Text")]
        [SerializeField] TextMeshProUGUI _text;

        private NavMeshAgent customerNavmeshAgent;
        private Animator customerAnimator;

        private int capacityToEat;

        private int chairNumber;

        private Transform finalDestination;
        private Transform currentDestination;

        /// <summary>
        /// Encapsulation
        /// </summary>
        public int CapacityToEat
        {
            get => capacityToEat;
            set => capacityToEat = value;
        }
        /// <summary>
        /// Chair Number to use later
        /// </summary>

        public int ChairNumber
        {
            get => chairNumber;
            set => chairNumber = value;
        }
        public CustomerHungerStatus HungerStatus
        {
            get => hungerStatus;
            set => hungerStatus = value;
        }

        private void Awake()
        {
            customerNavmeshAgent = GetComponent<NavMeshAgent>();
            customerAnimator = GetComponent<Animator>();
        }

        // Start is called before the first frame update
        void Start()
        {
            var trigger = GetComponent<Trigger>();
            trigger.OnEnterTrigger.AddListener(OnCustomerTriggerEnter);
            //trigger.OnExitTrigger.AddListener(OnCustomerTriggerExit);
            SetCapacity();
        }

        private void OnCustomerTriggerEnter(GameObject other)
        {
            if (hungerStatus == CustomerHungerStatus.Hungry)
            {
                StopWalking();
                customerNavmeshAgent.enabled = false;
            }
        }
        private void OnCustomerTriggerExit(GameObject other)
        {
            var Customer = other.GetComponent<Customer>();
            if (other.CompareTag("Customer") && Customer.HungerStatus == CustomerHungerStatus.Hungry)
            {
                Customer.MoveToPrevDestinantion();
            }
        }

        private void StopWalking()
        {
            customerAnimator.SetBool("Walk",false);
            customerAnimator.SetBool("Idle",true);
        }
        private void StartWalking()
        {
            customerAnimator.SetBool("Walk", true);
            customerAnimator.SetBool("Idle", false);
        }
        private void HoldingStack()
        {
            customerAnimator.SetBool("Stack",true);
            customerAnimator.SetBool("Idle", false); 
        }
        private void SetCapacity()
        {
            var count = Random.Range(0, _Capacity.Length);
            CapacityToEat = _Capacity[count];
            _text.text = $"{_Capacity[count]+" Burgers" }";
        }

        //Public fields
        public void PurchaseItem(GameObject go)
        {
            if (_specialBackpack.IsBackpackFull()) return;

            var fruit = go.GetComponent<Fruit>();
            if ( _specialBackpack.ShowedItemsType == fruit.resourceType || _specialBackpack.ItemsCount == 0)
            {
                _specialBackpack.AddItems(1, fruit.resourceType);
                HoldingStack();
            }
        }

        public void SetDestination(Transform destination)
        {
            customerNavmeshAgent.enabled = true;
            customerNavmeshAgent.SetDestination(destination.position);
            currentDestination = destination;
        }
        public void SetStartDestination(Transform destination)
        {
            customerNavmeshAgent.enabled = true;
            customerNavmeshAgent.SetDestination(destination.position);
            customerAnimator.SetBool("Walk", true);
            customerAnimator.SetTrigger("Simple");
            finalDestination = destination;
        }
        public void GoHome()
        {
            customerAnimator.SetBool("Sitting", false);
            customerAnimator.SetTrigger("Finish");
            customerNavmeshAgent.enabled = true;
            customerNavmeshAgent.SetDestination(finalDestination.position);
            customerNavmeshAgent.speed = 2.5f;
        }
        public void MoveToPrevDestinantion()
        {
            customerNavmeshAgent.enabled = true;
            customerNavmeshAgent.SetDestination(currentDestination.position);
            StartWalking();
        }
        public void DestroyGameObject()
        {
            DestroyCustomer(gameObject);
        }
        public void ShowNoSeatText(bool avaialable)
        {
            _noSeatText.alpha = avaialable ? 1 : 0;
        }
        public void ShowRequirementText(bool show)
        {
            _requiredText.alpha = show == true ? 1 : 0;
        }
    }
}

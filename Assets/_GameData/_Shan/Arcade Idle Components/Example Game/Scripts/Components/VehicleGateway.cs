using NaughtyAttributes;
using UnityEngine;
using TMPro;
using UnityEngine.AI;

namespace ArcadeIdle
{
    public class VehicleGateway : MonoBehaviour
    {
        /// <summary>
        /// Events Declaration
        /// </summary>
        public delegate void OnVehicleOrderFill(GameObject gameObject);
        public event OnVehicleOrderFill OrderFill;

        [BoxGroup("Vehicle Backpack")]
        public SpecialBackpack _specialBackpack;
        [BoxGroup("Capacity To Have Packages")]
        [SerializeField]
        private int[] _Capacity;
        [BoxGroup("Package Status")]
        [SerializeField]
        private OrderStatus _orderStatus;
        [BoxGroup("CanvasGroup Text")]
        [SerializeField] CanvasGroup _requiredText;
        [BoxGroup("CanvasGroup Text")]
        [SerializeField] TextMeshProUGUI _text;

        private NavMeshAgent vehicleNavmeshAgent;
        private int capacityToFillOrder;
        /// <summary>
        /// Encapsulation
        /// </summary>
        public int CapacityToFillOrder
        {
            get => capacityToFillOrder;
            set => capacityToFillOrder = value;
        }

        public OrderStatus OrderStatus
        {
            get => _orderStatus;
            set => _orderStatus = value;
        }

        private void Awake()
        {
            vehicleNavmeshAgent = GetComponent<NavMeshAgent>();
        }
        // Start is called before the first frame update
        void Start()
        {
            SetTrigger();
            SetCapacity();
        }

        private void SetTrigger()
        {
            var Trigger = GetComponent<Trigger>();
            Trigger.OnEnterTrigger.AddListener(OnDestination);
        }

        private void OnDestination(GameObject gameObject)
        {
            vehicleNavmeshAgent.enabled = false;
        }

        private void SetCapacity()
        {
            var count = Random.Range(0, _Capacity.Length);
            CapacityToFillOrder = _Capacity[count];
            _text.text = $"{_Capacity[count] + " Packages" }";
        }

        //Public fields
        public void PurchaseItem(GameObject go)
        {
            if (_specialBackpack.IsBackpackFull()) return;

            var fruit = go.GetComponent<Fruit>();
            if (_specialBackpack.ShowedItemsType == fruit.resourceType || _specialBackpack.ItemsCount == 0)
            {
                _specialBackpack.AddItems(1, fruit.resourceType);
            }
        }
        public void OrderCompleted()
        {
            OrderFill(gameObject);
        }
        public void ShowRequirementText(bool show, int number)
        {
            _requiredText.alpha = show == true ? 1 : 0;

            _text.text = $"{number + " Packages of tennis balls" }";
        }
        public void SetDestination(Transform destination)
        {
            vehicleNavmeshAgent.enabled = true;
            vehicleNavmeshAgent.SetDestination(destination.position);
        }
        public void SetDestination(Transform destination,float speed)
        {
            vehicleNavmeshAgent.enabled = true;
            vehicleNavmeshAgent.speed = speed;
            vehicleNavmeshAgent.SetDestination(destination.position);
        }
    }
}

using UnityEngine;
using UnityEngine.AI;
using DG.Tweening;
using NaughtyAttributes;

namespace ArcadeIdle.Shan
{
    public class StoppingPoint : MonoBehaviour
    {
        [HideInInspector]
        public GameObject _objectToAssign;
        [SerializeField] bool _isParking;
        [ShowIf("_isParking")]
        [SerializeField] Transform _moveTowards;
        // Start is called before the first frame update
        void Start()
        {
            SubscribeEvents();
        }
        private void SubscribeEvents()
        {
            var trigger = GetComponent<Trigger>();
            trigger.OnEnterTrigger.AddListener(OnVehicleEnter);
        }

        private void OnVehicleEnter(GameObject gameObject)
        {
            if (gameObject == _objectToAssign)
            {
                var vehicleNavmeshAgent = gameObject.GetComponent<NavMeshAgent>();
                vehicleNavmeshAgent.enabled = false;
                if (_isParking)
                    Parking();
            }
        }
        private void Parking()
        {
            TweenParams tParms = new TweenParams().SetEase(Ease.Linear);
            _objectToAssign.transform.DORotate(new Vector3(0, 90f, 0), 0.75f).SetAs(tParms).OnComplete(() => {

                _objectToAssign.transform.DOMove(_moveTowards.position, 1f);
            });
        }
    }
}

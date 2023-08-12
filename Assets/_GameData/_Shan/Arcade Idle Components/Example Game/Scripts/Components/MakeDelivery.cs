using NaughtyAttributes;
using UnityEngine;

namespace ArcadeIdle.Shan
{
    public class MakeDelivery : MonoBehaviour
    {
        Gateway gateway;
        // Start is called before the first frame update
        void Start()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            var trigger = GetComponent<Trigger>();
            trigger.OnEnterTrigger.AddListener(OnPlayerEnterToDelivery);
        }
        private void OnPlayerEnterToDelivery(GameObject gameObject)
        {
            if (gameObject.tag == "Gateway")
            {
                gateway = gameObject.GetComponent<Gateway>();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ArcadeIdle.Shan
{
    public class DestroyVehicle : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            SubscribeEvents();
        }
        private void SubscribeEvents()
        {
            var trigger = GetComponent<Trigger>();
            trigger.OnEnterTrigger.AddListener(OnObjectEnter);
        }
        private void OnObjectEnter(GameObject gameObject)
        {
            if (gameObject.tag == "Finish")
            {
                Destroy(gameObject);
            }
        }
    }
}

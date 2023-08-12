using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace ArcadeIdle
{
    public class Cashier : MonoBehaviour
    {
        public static bool _cashierAvailable;
        [SerializeField] GameObject _arrow;
        [SerializeField] GameObject _enterImage;
        [SerializeField] GameObject _leaveImage;
        // Start is called before the first frame update
        void Start()
        {
            SubscribeEvents();
        }
        private void SubscribeEvents()
        {
            var trigger = GetComponent<Trigger>();
            //Subscribes trigger events by script because it more safety
            //Events in inspector can accidentally resets and you'll to set it again
            trigger.OnEnterTrigger.AddListener(OnPlayerEnter);
            trigger.OnStayTrigger.AddListener(OnPlayerStay);
            trigger.OnExitTrigger.AddListener(OnPlayerExit);
        }
        public void OnPlayerEnter(GameObject gameObject)
        {
            
            _cashierAvailable = true;
            _enterImage.SetActive(true);
        }
        public void OnPlayerStay(GameObject gameObject)
        {
            _cashierAvailable = true;
            _enterImage.SetActive(true);
        }
        public void OnPlayerExit(GameObject gameObject)
        { 
            _cashierAvailable = false;
            _enterImage.SetActive(false);
        }
    }
}

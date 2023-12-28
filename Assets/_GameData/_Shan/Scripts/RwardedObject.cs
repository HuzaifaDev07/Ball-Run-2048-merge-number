using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcadeIdle.Shan
{
    public class RwardedObject : MonoBehaviour
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
            if (gameObject.tag == "Player")
            {
                ResourcesSystem.Instance.AddResourceCount(ResourcesSystem.ResourceType.Banknotes, 50);
                SaveSystem.Instance.SaveData();
                PlayerController.Instance.DisableNavmeshDraw();
                RewardedAdScenario.Instance.CallRewardedAdFunction();
                this.transform.parent.gameObject.SetActive(false);
            }
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace ArcadeIdle.Shan
{
    public class RewardedAdScenario : MonoBehaviour
    {
        public static RewardedAdScenario Instance;
        [SerializeField] GameObject _rewardedObject;
        [SerializeField] Transform[] _places;

        [SerializeField] float _waitTime;
        int randomNumber = -1;

        private void Awake()
        {
            Instance = this;
        }
        // Start is called before the first frame update
        void Start()
        {
            bool gatewayCounter = SaveSystem.Instance.Data.AddPackagesToGateway;
            if (gatewayCounter)
            {
                StartCoroutine(ShowRewardedObject());
            }
        }

        
        private IEnumerator ShowRewardedObject()
        {
            yield return new WaitForSeconds(_waitTime);
            randomNumber = Random.Range(0, _places.Length - 1);
            _rewardedObject.transform.localPosition = _places[randomNumber].position;
            _rewardedObject.transform.localRotation = _places[randomNumber].rotation;
            _rewardedObject.SetActive(true);
            PlayerController.Instance.ShowNextTargetNavmesh(_rewardedObject.transform);
        }
        public void CallRewardedAdFunction()
        {
            StartCoroutine(ShowRewardedObject());
        }
    }
}

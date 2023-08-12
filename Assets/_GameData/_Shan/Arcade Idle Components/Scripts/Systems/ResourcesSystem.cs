using System;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace ArcadeIdle
{
    public class ResourcesSystem : Initializable
    {
        public enum ResourceType
        {
            Banknotes,
            BanknotesForPreview,
        }

        [Serializable]
        public struct ResourceSettings
        {
            public ResourceType Type;
            public ResourceCounter Counter;
        }

        [BoxGroup("SETTINGS")] [SerializeField] 
        private ResourceSettings[] _resources;
        [BoxGroup("SETTINGS")] [SerializeField] [OnValueChanged("OnTestModeValueChanged")]
        private bool _testMode;
        [BoxGroup("SETTINGS")] [SerializeField] [ShowIf("_testMode")]
        private int[] _testResourcesCounts;

        public static ResourcesSystem Instance;

        private void OnTestModeValueChanged()
        {
            if (_testMode)
            {
                _testResourcesCounts = new int[_resources.Length];
            }
        }

        public override void Initialize()
        {
            Instance = this;

            if (SaveSystem.Instance.Data.ResourcesCounts == null)
            {
                SaveSystem.Instance.Data.ResourcesCounts = new int[_resources.Length];
                SaveSystem.Instance.SaveData();
            }
            else if(SaveSystem.Instance.Data.ResourcesCounts.Length < _resources.Length)
            {
                SaveSystem.Instance.Data.ResourcesCounts = new int[_resources.Length];
                SaveSystem.Instance.SaveData();
            }
        }

        private void Start()
        {
            CheckForErrors();

            foreach (var resource in _resources)
            {
                var resourceId = (int) resource.Type;

                if (_testMode)
                {
                    SaveSystem.Instance.Data.ResourcesCounts[resourceId] = _testResourcesCounts[resourceId];
                    SaveSystem.Instance.SaveData();
                }
                
                resource.Counter.SetValue(SaveSystem.Instance.Data.ResourcesCounts[resourceId]);   
            }
        }

        private void CheckForErrors()
        {
            if (Resources.Load<Settings>(Constants.SETTINGS).ShowWarnings)
            {
                if (_resources.Length == 0)
                {
                    Debug.LogError("Resources System: resources settings is empty.");
                }
            }
        }

        public int GetResourceCount(ResourceType type)
        {
            var resourceId = (int) type;
            return SaveSystem.Instance.Data.ResourcesCounts[resourceId];
        }
        
        public void AddResourceCount(ResourceType type, int value)
        {
            var resourceId = (int) type;
            var resourceCount = SaveSystem.Instance.Data.ResourcesCounts[resourceId];
            var clampedValue = Mathf.Clamp(resourceCount + value, 0, int.MaxValue);
            
            SaveSystem.Instance.Data.ResourcesCounts[resourceId] = clampedValue;
            SaveSystem.Instance.SaveData();

            var counter = _resources.First(m => m.Type.Equals(type)).Counter;
            counter.SetValue(clampedValue);
        }

        /// <summary>
        /// Subtracts the price from the number of banknotes if they are enough and returns it through action.
        /// </summary>
        /// <param name="type">What resource will pay for the purchase.</param>
        /// <param name="price">The value to be deducted from the total number of resource.</param>
        /// <param name="onComplete">An event that is called if there are enough resource.</param>
        public void TryToBuy(ResourceType type, int price, Action onComplete)
        {
            var resourceId = (int) type;
            if (SaveSystem.Instance.Data.ResourcesCounts[resourceId] >= price)
            {
                AddResourceCount(type, -price);
                onComplete?.Invoke();
            }
        }
    }
}
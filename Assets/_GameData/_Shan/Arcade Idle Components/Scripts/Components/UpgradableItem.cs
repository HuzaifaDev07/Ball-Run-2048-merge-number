using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace ArcadeIdle
{
    [AddComponentMenu("Arcade Idle Components/Upgradable Item", 0)]
    public class UpgradableItem : Initializable
    {
        private enum IncreaseType
        {
            Multiply,
            Add,
        }

        //[BoxGroup("BUTTON BUY")] [SerializeField] [Required("Drag and drop button buy here.")]
        //private ButtonBuy _buttonBuy;
        [BoxGroup("BUTTON BUY")] [SerializeField]
        private bool _usePriceText;
        [BoxGroup("BUTTON BUY")] [ShowIf("_usePriceText")] [SerializeField] 
        private string _buttonTextAfterMaxUpgrade;

        [BoxGroup("PLACEMENT")]
        [SerializeField]
        private bool _usePlacement;
        [BoxGroup("PLACEMENT")]
        [ShowIf("_usePlacement")]
        [SerializeField]
        private Placement _placement;
        [BoxGroup("PLACEMENT")]
        [ShowIf("_usePlacement")]
        [SerializeField]
        private bool _hidePlacementAfterUnlock;

        [BoxGroup("PLACEMENT REWARDED")]
        [SerializeField]
        private bool _useRewarded;

        [BoxGroup("PROGRESS BAR")] [SerializeField]
        private bool _useProgressBar;
        [BoxGroup("PROGRESS BAR")] [ShowIf("_useProgressBar")] [SerializeField] [Required("Drag and drop progress bar with items here.")]
        private ProgressBarWithItems _progressBar;

        [BoxGroup("SETTINGS")] [SerializeField]
        private int _id;
        [BoxGroup("SETTINGS")] [SerializeField]
        private int _maxUpgradeLevel;

        [BoxGroup("PRICE")] [SerializeField]
        private int _price;
        [BoxGroup("PRICE")] [SerializeField]
        private ResourcesSystem.ResourceType _paymentResource;
        
        [BoxGroup("PRICE")] [SerializeField] 
        private bool _increasePriceAfterUpgrade;
        [BoxGroup("PRICE")] [ShowIf("_increasePriceAfterUpgrade")] [SerializeField] 
        private IncreaseType _increaseType;
        [BoxGroup("PRICE")] [ShowIf("_increasePriceAfterUpgrade")] [SerializeField] 
        private float _increaseValue;

        [BoxGroup("EVENTS")]
        public UnityEvent<int> OnUpgradeItem;

        public int UpgradeLevel { get; private set; }

        private int _currentPrice;

        public override void Initialize()
        {
            LoadState();
        }

        private void Start()
        {
            _placement.UseRewarded = _useRewarded;
            _placement.OnEnterPlacement.AddListener(OnUpgrade);
            //_buttonBuy.Button.onClick.AddListener(TryToUpgradeItem);
            CheckForErrors();
            CalculatePrice();
            UpdateState();
        }

        private void CheckForErrors()
        {
            if (Resources.Load<Settings>(Constants.SETTINGS).ShowWarnings)
            {
                /*if (_buttonBuy == null)
                {
                    Debug.LogError("Unlockable Item: button buy is null.");
                }*/
                
                if (_useProgressBar && _progressBar == null)
                {
                    Debug.LogError("Unlockable Item: progress bar is null.");
                }
            }
        }
        
        private void LoadState()
        {
            var data = SaveSystem.Instance.Data.UpgradableItemsData.Find(item => item.Id == _id); //Tries to get this item data
            
            if (data == null) //If there aren't data, creates a new and saves
            {
                data = new UpgradableItemData {Id = _id, UpgradeLevel = UpgradeLevel};
                SaveSystem.Instance.Data.UpgradableItemsData.Add(data);
                SaveSystem.Instance.SaveData();
            }
            
            UpgradeLevel = data.UpgradeLevel;
        }

        private void UpdateState()
        {
            //_progressBar.SetProgress(UpgradeLevel);

            if (UpgradeLevel == _maxUpgradeLevel)
            {
                gameObject.SetActive(false);
                if (_usePriceText)
                {
                    //_buttonBuy.HideIcon();
                    //_buttonBuy.SetText(_buttonTextAfterMaxUpgrade);
                }
            }
            else
            {
                if (_usePriceText)
                {
                   // _buttonBuy.SetText(_currentPrice.ToString());
                }
            }
        }

        private void TryToUpgradeItem()
        {
            if (UpgradeLevel == _maxUpgradeLevel) return;
            ResourcesSystem.Instance.TryToBuy(_paymentResource, _currentPrice, OnUpgrade);  //Tries to pay price to upgrade
        }

        private void OnUpgrade()
        {
            var data = SaveSystem.Instance.Data.UpgradableItemsData.Find(item => item.Id == _id);
            data.UpgradeLevel++;
            SaveSystem.Instance.SaveData();

            UpgradeLevel = data.UpgradeLevel;

            OnUpgradeItem?.Invoke(data.UpgradeLevel);
            CalculatePrice();
            UpdateState();
        }
        private void OnUpgrade(GameObject gameObject)
        {
            var data = SaveSystem.Instance.Data.UpgradableItemsData.Find(item => item.Id == _id);
            data.UpgradeLevel++;
            SaveSystem.Instance.SaveData();

            UpgradeLevel = data.UpgradeLevel;

            OnUpgradeItem?.Invoke(data.UpgradeLevel);
            CalculatePrice();
            UpdateState();
            _placement.ResetProgressBar();
        }

        private void CalculatePrice()
        {
            _currentPrice = _price;

            if (_increasePriceAfterUpgrade)
            {
                for (var n = 0; n < UpgradeLevel; n++)
                {
                    if (_increaseType == IncreaseType.Multiply)
                    {
                        _currentPrice = (int) (_currentPrice * _increaseValue);
                    }
                    else
                    {
                        _currentPrice = (int) (_currentPrice + _increaseValue);
                    }
                }
            }
        }
    }
}
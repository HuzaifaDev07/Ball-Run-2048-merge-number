using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace ArcadeIdle
{
    [AddComponentMenu("Arcade Idle Components/Unlockable Item", 0)]
    public class UnlockableItem : Initializable
    {
        [BoxGroup("PLACEMENT")] [SerializeField]
        private bool _usePlacement;
        [BoxGroup("PLACEMENT")] [ShowIf("_usePlacement")] [SerializeField] [Required("Drag and drop placement here.")]
        private Placement _placement;
        [BoxGroup("PLACEMENT")] [ShowIf("_usePlacement")] [SerializeField] 
        private bool _hidePlacementAfterUnlock;

        [BoxGroup("PLACEMENT REWARDED")] [SerializeField]
        private bool _useRewarded;
        
        [BoxGroup("BUTTON BUY")] [SerializeField]
        private bool _useButtonBuy;
        [BoxGroup("BUTTON BUY")] [ShowIf("_useButtonBuy")] [SerializeField] [Required("Drag and drop button buy here.")]
        private ButtonBuy _buttonBuy;
        [BoxGroup("BUTTON BUY")] [ShowIf("_useButtonBuy")] [SerializeField] 
        private string _buttonTextAfterUnlock;
        
        [BoxGroup("PRICE TEXT")] [SerializeField]
        private bool _usePriceText;
        [BoxGroup("PRICE TEXT")] [ShowIf("_usePriceText")] [SerializeField] [Required("Drag and drop price text here.")]
        private TextMeshProUGUI _text;
        [BoxGroup("PRICE TEXT")] [ShowIf("_usePriceText")] [SerializeField] 
        private string _priceTextAfterUnlock;
        
        [BoxGroup("TUTORIAL TEXT")] [SerializeField]
        private bool _useTutorialText;
        [BoxGroup("TUTORIAL TEXT")] [ShowIf("_useTutorialText")] [SerializeField] 
        private string _tutorialTextToDisplay;
        
        [BoxGroup("SETTINGS")] [SerializeField]
        private int _id;
        [BoxGroup("SETTINGS")] [SerializeField]
        private bool _isUnlockedByDefault;
        [BoxGroup("SETTINGS")] [SerializeField] [HideIf("_isUnlockedByDefault")]
        private int _price;
        [BoxGroup("SETTINGS")] [SerializeField] [HideIf("_isUnlockedByDefault")]
        private ResourcesSystem.ResourceType _paymentResource;
        
        [BoxGroup("EVENTS")]
        public UnityEvent OnUnlockItem;

        public bool IsUnlocked { get; private set; }

        public override void Initialize()
        {
            LoadState();
        }

        private void Awake()
        {
            _placement.UseRewarded = _useRewarded;
            _placement.MoneyAdded += DecreasePrice;
            _placement.RequiredResources = _price;
        }
        private void Start()
        {
            CheckForErrors();
            SubscribeListeners();
            UpdateState();
        }

        private void CheckForErrors()
        {
            if (Resources.Load<Settings>(Constants.SETTINGS).ShowWarnings)
            {
                if (_usePlacement && _placement == null)
                {
                    Debug.LogError("Unlockable Item: placement is null.");
                }
                
                if (_useButtonBuy && _buttonBuy == null)
                {
                    Debug.LogError("Unlockable Item: button buy is null.");
                }
                
                if (_usePriceText && _text == null)
                {
                    Debug.LogError("Unlockable Item: price text is null.");
                }
            }
        }

        private void SubscribeListeners()
        {
            if (_useButtonBuy)
            {
                _buttonBuy.Button.onClick.AddListener(TryToUnlockItem);
            }

            if (_usePlacement && !_useRewarded)
            {
                _placement.OnEnterPlacement.AddListener(TryToUnlockItem);
            }
            else if (_useRewarded)
            {
                _placement.OnEnterPlacement.AddListener(OnUnlock);
            }
        }
        private void DecreasePrice(int price)
        {
            _price -= price;
        }
        private void LoadState()
        {
            var data = SaveSystem.Instance.Data.UnlockableItemsData.Find(item => item.Id == _id); //Tries to get this item data

            if (data == null) //If there aren't data, creates a new and saves
            {
                data = new UnlockableItemData {Id = _id, IsUnlocked = _isUnlockedByDefault};
                SaveSystem.Instance.Data.UnlockableItemsData.Add(data);
                SaveSystem.Instance.SaveData();
            }
            
            IsUnlocked = data.IsUnlocked;
        }

        private void UpdateState()
        {
            if (IsUnlocked)
            {
                if (_useButtonBuy)
                {
                    _buttonBuy.HideIcon();
                    _buttonBuy.SetText(_buttonTextAfterUnlock);
                }
                
                if (_usePlacement && _hidePlacementAfterUnlock)
                {
                    _placement.HideElement();
                }
                
                if (_usePriceText)
                {
                    _text.text = _priceTextAfterUnlock;
                }
            }
            else
            {
                if (_useButtonBuy)
                {
                    _buttonBuy.SetText(_price.ToString());
                }

                if (_usePriceText && !_useRewarded)
                {
                    _text.text = _price.ToString();
                }
            }
        }
        
        private void TryToUnlockItem()
        {
            if (IsUnlocked) return;
            ResourcesSystem.Instance.TryToBuy(_paymentResource, _price, OnUnlock);  //Tries to pay price to unlock
        }

        private void TryToUnlockItem(GameObject go)
        {
            if (IsUnlocked) return;
            ResourcesSystem.Instance.TryToBuy(_paymentResource, _price, OnUnlock);
        }

        private void OnUnlock()
        {
            var data = SaveSystem.Instance.Data.UnlockableItemsData.Find(item => item.Id == _id);
            data.IsUnlocked = true;
            SaveSystem.Instance.SaveData();

            IsUnlocked = true;
            OnUnlockItem?.Invoke();
            UpdateState();
        }
        private void OnUnlock(GameObject gameObject)
        {
            var data = SaveSystem.Instance.Data.UnlockableItemsData.Find(item => item.Id == _id);
            data.IsUnlocked = true;
            SaveSystem.Instance.SaveData();

            IsUnlocked = true;
            OnUnlockItem?.Invoke();
            UpdateState();
        }
    }
}
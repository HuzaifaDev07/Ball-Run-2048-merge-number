using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace ArcadeIdle
{
    [AddComponentMenu("Arcade Idle Components/Progress Bar with Items", 0)]
    public class ProgressBarWithItems : UIElement
    {
        [BoxGroup("LINKS")] [OnValueChanged("OnValueChanged")] [SerializeField] 
        private Image[] _items;

        [BoxGroup("SETTINGS")] [OnValueChanged("OnValueChanged")] [SerializeField] [Required("Drag and drop icon for unlocked item here.")] 
        private Sprite _unlockedItemIcon;
        [BoxGroup("SETTINGS")] [OnValueChanged("OnValueChanged")] [SerializeField] [Required("Drag and drop icon for locked item here.")] 
        private Sprite _lockedItemIcon;

        [BoxGroup("SETTINGS")] [OnValueChanged("OnValueChanged")] [SerializeField]
        private int _value;

        private void OnValueChanged()
        {
            _value = Mathf.Clamp(_value, 0, _items.Length);
            
            for (var n = 0; n < _value; n++)
            {
                _items[n].sprite = _unlockedItemIcon;
            }

            for (var n = _value; n < _items.Length; n++)
            {
                _items[n].sprite = _lockedItemIcon;
            }
        }

        private void Start()
        {
           CheckForErrors();
        }

        private void CheckForErrors()
        {
            if (Resources.Load<Settings>(Constants.SETTINGS).ShowWarnings)
            {
                if (_items.Length == 0)
                {
                    Debug.LogError("Progress Bar with Items: items length is 0.");
                }

                if (_unlockedItemIcon == null)
                {
                    Debug.LogError("Progress Bar with Items: unlocked item icon is null.");
                }

                if (_unlockedItemIcon == null)
                {
                    Debug.LogError("Progress Bar with Items: locked item icon is null.");
                }
            }
        }

        /// <summary>
        /// Sets a new progress bar value.
        /// </summary>
        public void SetProgress(int value)
        {
            _value = value;
            OnValueChanged();
        }
        
        /// <summary>
        /// Restores standard values.
        /// </summary>
        public void ResetValues()
        {
            _value = 0;
            OnValueChanged();
        }

        /// <summary>
        /// Returns progress bar items count.
        /// </summary>
        public int GetMaxValue()
        {
            return _items.Length;
        }

        protected override void OnShow() { }

        protected override void OnHide() {}
    }
}
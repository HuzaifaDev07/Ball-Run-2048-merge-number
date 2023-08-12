using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace ArcadeIdle
{
    [AddComponentMenu("Arcade Idle Components/Settings Panel", 0)]
    public class SettingsPanel : UIElement
    {
        [InfoBox("Button close is necessary only for big settings panel.")]
        [BoxGroup("LINKS")] [SerializeField] 
        private Button _close;
        [BoxGroup("LINKS")] [SerializeField] [Required("Drag and drop sounds button here.")] 
        private Button _sounds;
        [BoxGroup("LINKS")] [SerializeField] [Required("Drag and drop vibration button here.")] 
        private Button _vibration;

        [BoxGroup("SETTINGS")] [SerializeField] [Required("Drag and drop sounds on sprite here.")] 
        private Sprite _soundsOn;
        [BoxGroup("SETTINGS")] [SerializeField] [Required("Drag and drop sounds off sprite here.")] 
        private Sprite _soundsOff;
        [BoxGroup("SETTINGS")] [SerializeField] [Required("Drag and drop vibration on sprite here.")] 
        private Sprite _vibrationOn;
        [BoxGroup("SETTINGS")] [SerializeField] [Required("Drag and drop vibration off sprite here.")] 
        private Sprite _vibrationOff;

        private void Start()
        {
            AudioListener.volume = SaveSystem.Instance.Data.Sounds ? 1f : 0f;

            CheckForErrors();
            
            if (_close != null)
            {
                _close.onClick.AddListener(HideElement);
            }

            _sounds.onClick.AddListener(OnButtonSoundsClick);
            _vibration.onClick.AddListener(OnButtonVibrationClick);
        }

        private void CheckForErrors()
        {
            if (Resources.Load<Settings>(Constants.SETTINGS).ShowWarnings)
            {
                if (_sounds == null || _vibration == null)
                {
                    Debug.LogError("Settings Panel: it seems like one of settings button is null.");
                }

                if (_soundsOn == null || _soundsOff == null || _vibrationOn == null || _vibrationOff == null)
                {
                    Debug.LogError("Settings Panel: it seems like one of button's sprite is null.");
                }
            }
        }

        protected override void OnShow()
        {
            UpdateSprites();
        }

        protected override void OnHide() {}

        private void UpdateSprites()
        {
            if (SaveSystem.Instance == null) return;
            _sounds.image.sprite = SaveSystem.Instance.Data.Sounds ? _soundsOn : _soundsOff;
            _vibration.image.sprite = SaveSystem.Instance.Data.Vibration ? _vibrationOn : _vibrationOff;
        }

        private void OnButtonSoundsClick()
        {
            SaveSystem.Instance.Data.Sounds = !SaveSystem.Instance.Data.Sounds;
            SaveSystem.Instance.SaveData();

            AudioListener.volume = SaveSystem.Instance.Data.Sounds ? 1f : 0f;
            UpdateSprites();
        }
        
        private void OnButtonVibrationClick()
        {
            SaveSystem.Instance.Data.Vibration = !SaveSystem.Instance.Data.Vibration;
            SaveSystem.Instance.SaveData();
            
            UpdateSprites();
        }
    }
}
using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArcadeIdle
{
    [AddComponentMenu("Arcade Idle Components/Resources Counter", 0)]
    public class ResourceCounter : MonoBehaviour
    {
        [BoxGroup("LINKS")] [SerializeField] [Required("Drag and drop counter text here.")] 
        private TextMeshProUGUI _text;
        [BoxGroup("LINKS")] [SerializeField] [Required("Drag and drop resource icon image here.")] 
        private Image _image;

        [BoxGroup("SETTINGS")] [OnValueChanged("OnIconChanged")] [SerializeField] [Required("Drag and drop resource icon here.")] 
        private Sprite _icon;
        [BoxGroup("SETTINGS")] [OnValueChanged("OnValueChanged")] [SerializeField] 
        private int _value;

        private void OnIconChanged()
        {
            _image.sprite = _icon;
        }
        
        private void OnValueChanged()
        {
            _value = Mathf.Clamp(_value, 0, Int32.MaxValue);
            _text.text = _value.ToString();
        }

        private void Start()
        {
            CheckForErrors();
        }

        private void CheckForErrors()
        {
            if (Resources.Load<Settings>(Constants.SETTINGS).ShowWarnings)
            {
                if (_text == null)
                {
                    Debug.LogError("Resource Counter: counter text is null.");
                }

                if (_image == null)
                {
                    Debug.LogError("Resource Counter: icon image is null.");
                }
            
                if (_icon == null)
                {
                    Debug.LogError("Resource Counter: icon sprite is null.");
                }
            }
        }

        /// <summary>
        /// Sets a new resource counter value.
        /// </summary>
        public void SetValue(int value)
        {
            _value = value;
            OnValueChanged();
        }
    }
}
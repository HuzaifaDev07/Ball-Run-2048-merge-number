using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArcadeIdle
{
    [AddComponentMenu("Arcade Idle Components/Progress Bar", 0)]
    public class ProgressBar : UIElement
    {
        [BoxGroup("PROGRESS LINE")] [SerializeField] 
        private bool _setImageManually;
        [BoxGroup("PROGRESS LINE")] [ShowIf("_setImageManually")] [SerializeField] [Required("Drag and drop progress line image here.")] 
        private Image _image;
        
        [BoxGroup("TEXT")] [SerializeField] 
        private bool _useText;
        [BoxGroup("TEXT")] [ShowIf("_useText")] [SerializeField] [Required("Drag and drop text mesh pro here.")] 
        private TextMeshProUGUI _text;
        [BoxGroup("TEXT")] [ShowIf("_useText")] [SerializeField] 
        private bool _isHealthBar;
        [BoxGroup("TEXT")] [ShowIf("_useText")] [SerializeField] [InfoBox("You can write any text you need, just do not touch the brackets, they are needed to format the text.")] 
        private string _textFormat = "{0}text";
        
        [BoxGroup("SETTINGS")] [OnValueChanged("OnMaxValueChanged")] [SerializeField] 
        private float _maxValue = 1f;
        [BoxGroup("SETTINGS")] [OnValueChanged("OnValueChanged")] [SerializeField] 
        private float _value = 1f;


        private float _width;


        private void OnMaxValueChanged()
        {
            _maxValue = Mathf.Clamp(_maxValue, 0, int.MaxValue);

            if (_value > _maxValue)
            {
                _value = _maxValue;
            }

            SetProgress(_value, _maxValue);
        }

        private void OnValueChanged()
        {
            _value = Mathf.Clamp(_value, 0, _maxValue);
            SetProgress(_value, _maxValue);
        }
        
        protected override void OnShow()
        {
            ResetValues();
        }

        protected override void OnHide()
        {
            ResetValues();
        }

        private void Start()
        {
            CheckForErrors();
            
            if(_setImageManually == false)
            {
                _image = GetComponent<Image>();
            }

            if (_image.type != Image.Type.Filled)
            {
                GetProgressLineWidth();
            }
        }

        private void CheckForErrors()
        {
            if (Resources.Load<Settings>(Constants.SETTINGS).ShowWarnings)
            {
                if (_setImageManually && _image == null)
                {
                    Debug.LogError("Progress Bar: progress line is null.");
                }

                if (_useText && _text == null)
                {
                    Debug.LogError("Progress Bar: text is null.");
                }
            }
        }

        private void GetProgressLineWidth()
        {
            _image.rectTransform.offsetMax = Vector2.zero;
            _width = _image.rectTransform.rect.width;
            
            OnValueChanged();
        }

        /// <summary>
        /// Sets a new value and updates the text if it is used.
        /// </summary>
        /// <param name="value">Current progress bar value.</param>
        /// <param name="maxValue">Progress bar maximum value.</param>
        public void SetProgress(float value, float maxValue)
        {
            var clampedValue = 1f / maxValue * value;

            if (_image.type == Image.Type.Filled)
            {
                _image.fillAmount = clampedValue;
            }
            else
            {
                if (_width == 0f)
                {
                    _width = _image.rectTransform.rect.width;
                }

                var offset = _width - _width * clampedValue;
                _image.rectTransform.offsetMax = new Vector2(-offset, 0f);
            }


            if (_text == null) return;
            
            if (_useText == false)
            {
                _text.text = string.Empty;
            }
            else
            {
                _text.text = _isHealthBar ? string.Format(_textFormat, (int) value, (int) maxValue) : string.Format(_textFormat, (int) value);
            }
        }

        /// <summary>
        /// Restores standard values.
        /// </summary>
        public void ResetValues()
        {
            if (_image != null)
            {
                _image.fillAmount = 0f;
            }
            if (_text != null)
            {
                _text.text = string.Empty;
            }
        }
    }
}
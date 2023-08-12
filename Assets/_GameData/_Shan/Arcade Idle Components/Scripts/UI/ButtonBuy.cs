using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ArcadeIdle
{
    public class ButtonBuy : MonoBehaviour
    {
        [BoxGroup("LINKS")] [SerializeField] [Required("Drag and drop price text here.")]
        private TextMeshProUGUI _text;
        [BoxGroup("LINKS")] [SerializeField]  [Required("Drag and drop price resource icon here.")]
        private GameObject _resourceIcon;

        public Button Button { get; private set; }

        private void Awake()
        {
            Button = GetComponent<Button>();
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
                    Debug.LogError("Button Buy: text mesh pro ugui is null.");
                }

                if (_resourceIcon == null)
                {
                    Debug.LogError("Button Buy: resources icon null.");
                }
            }
        }

        /// <summary>
        /// Just changes button text.
        /// </summary>
        public void SetText(string text)
        {
            _text.text = text;
        }

        /// <summary>
        /// Hides resource icon.
        /// </summary>
        public void HideIcon()
        {
            _resourceIcon.SetActive(false);
        }
    }
}
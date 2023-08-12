using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace ArcadeIdle
{
    public class UpgradesPanel : UIElement
    {
        [BoxGroup("LINKS")] [SerializeField] [Required("Drag and drop close button here.")]
        private Button _close;

        private void Start()
        {
            CheckForErrors();
            _close.onClick.AddListener(HideElement);
        }

        private void CheckForErrors()
        {
            if (Resources.Load<Settings>(Constants.SETTINGS).ShowWarnings)
            {
                if (_close == null)
                {
                    Debug.LogError("Upgrades Panel: button close is null.");
                }
            }
        }

        protected override void OnShow() {}
        protected override void OnHide() {}
    }
}
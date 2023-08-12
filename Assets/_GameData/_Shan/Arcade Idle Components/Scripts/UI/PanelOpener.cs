using NaughtyAttributes;
using UnityEngine;
using Button = UnityEngine.UI.Button;

namespace ArcadeIdle
{
    [RequireComponent(typeof(Animator))]
    [AddComponentMenu("Arcade Idle Components/Panel Opener", 0)]
    public class PanelOpener : MonoBehaviour
    {
        [BoxGroup("LINKS")] [SerializeField] [Required("Drag and drop ui panel here.")] 
        private UIElement _panel;

        [InfoBox("You can play the animation when you open and close the panel.")]
        [BoxGroup("BUTTON ANIMATION")] [SerializeField] 
        private bool _useButtonAnimations;
        [BoxGroup("BUTTON ANIMATION")] [ShowIf("_useButtonAnimations")] [SerializeField]
        private string _showPanelTriggerName;
        [BoxGroup("BUTTON ANIMATION")] [ShowIf("_useButtonAnimations")] [SerializeField]
        private string _hidePanelTriggerName;

        private Animator _animator;
        
        private void Start()
        {
            CheckForErrors();
            
            _animator = GetComponent<Animator>();
            GetComponent<Button>().onClick.AddListener(OnButtonClick);

            _panel.OnElementStateChanged += OnPanelStateChanged;
        }

        private void CheckForErrors()
        {
            if (Resources.Load<Settings>(Constants.SETTINGS).ShowWarnings)
            {
                if (_panel == null)
                {
                    Debug.LogError("Panel Opener: panel link is null.");
                }
            }
        }

        private void OnPanelStateChanged()
        {
            if (_useButtonAnimations == false) return;
            
            if (_panel.IsShowed)
            {
                if (_hidePanelTriggerName.Length != 0)
                {
                    _animator.SetTrigger(_hidePanelTriggerName);
                }
            }
            else
            {
                _animator.SetTrigger(_showPanelTriggerName);
            }
        }

        private void OnButtonClick()
        {
            if (_panel.IsShowed)
            {
                _panel.HideElement();
            }
            else
            {
                _panel.ShowElement();
            }
        }
    }
}
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace ArcadeIdle
{
    [RequireComponent(typeof(Animator))]
    [AddComponentMenu("Arcade Idle Components/Button Animations", 0)]
    public class ButtonAnimations : MonoBehaviour
    {
        [InfoBox("Clicking the button will set trigger by name you write in animator attached to the object.")]
        [BoxGroup("SETTINGS")] [SerializeField]
        private string _triggerName;
        private Animator _animator;
        
        private void Start()
        {
            _animator = GetComponent<Animator>();
            GetComponent<Button>().onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            _animator.SetTrigger(_triggerName);
        }
    }
}
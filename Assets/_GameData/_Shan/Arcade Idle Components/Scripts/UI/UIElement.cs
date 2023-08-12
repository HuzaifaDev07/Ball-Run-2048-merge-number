using System;
using NaughtyAttributes;
using UnityEngine;

namespace ArcadeIdle
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIElement : MonoBehaviour
    {
        [BoxGroup("LINKS")] [SerializeField] [Required("Drag and drop canvas group here.")] 
        private CanvasGroup _canvasGroup;

        public bool IsShowed { get; private set; }
        public event Action OnElementStateChanged;

        /// <summary>
        /// Makes the element visible and interactive.
        /// </summary>
        [Button]
        public void ShowElement()
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
            
            OnElementStateChanged?.Invoke();
            IsShowed = true;
            
            OnShow();
        }
        
        /// <summary>
        /// Hides the element and makes it non-interactive. GameObject remains active.
        /// </summary>
        [Button]
        public void HideElement()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            OnElementStateChanged?.Invoke();
            IsShowed = false;
            
            OnHide();
        }
        
        protected abstract void OnShow();
        protected abstract void OnHide();
    }
}
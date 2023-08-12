using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace ArcadeIdle
{
    [AddComponentMenu("Arcade Idle Components/Trigger", 0)]
    public class Trigger : MonoBehaviour
    {
        [InfoBox("You can limit the tags on which the trigger will work.")]
        [SerializeField] 
        private bool _useTagsLimit;
        [ShowIf("_useTagsLimit")] [Tag] [SerializeField] 
        private string[] _allowedTags;
        
        [BoxGroup("EVENT")]
        public UnityEvent<GameObject> OnEnterTrigger;
        [BoxGroup("EVENT")]
        public UnityEvent<GameObject> OnStayTrigger;
        [BoxGroup("EVENT")]
        public UnityEvent<GameObject> OnExitTrigger;

        private void OnCollisionEnter(Collision other)
        {
            OnEnter(other.gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            OnStay(other.gameObject);
        }

        private void OnCollisionExit(Collision other)
        {
            OnExit(other.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            OnEnter(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            OnExit(other.gameObject);
        }

        private void OnEnter(GameObject go)
        {
            if (_useTagsLimit && _allowedTags.Length != 0)
            {
                if (CheckGOTag(go))
                {
                    OnEnterTrigger?.Invoke(go);
                }
            }
            else
            {
                OnEnterTrigger?.Invoke(go);
            }
        }
        private void OnStay(GameObject go)
        {
            if (_useTagsLimit && _allowedTags.Length != 0)
            {
                if (CheckGOTag(go))
                {
                    OnStayTrigger?.Invoke(go);
                }
            }
            else
            {
                OnStayTrigger?.Invoke(go);
            }
        }
        
        private void OnExit(GameObject go)
        {
            if (_useTagsLimit && _allowedTags.Length != 0)
            {
                if (CheckGOTag(go))
                {
                    OnExitTrigger?.Invoke(go);
                }
            }
            else
            {
                OnExitTrigger?.Invoke(go);
            }
        }

        private bool CheckGOTag(GameObject go)
        {
            foreach (var tag in _allowedTags)
            {
                if (string.IsNullOrEmpty(tag)) continue; //Avoid exception if the tag field is None
                
                if (go.CompareTag(tag))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
using NaughtyAttributes;
using UnityEngine;
using DG.Tweening;

namespace ArcadeIdle.Shan
{
    public class TextureChange : MonoBehaviour
    {
        [BoxGroup("BoxMesh")] [SerializeField] GameObject _box;
        [BoxGroup("Material to Change")]
        public Material _newMaterial;

        private MeshRenderer objectRenderer;
        // Start is called before the first frame update
        void Start()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            var trigger = GetComponent<Trigger>();
            trigger.OnExitTrigger.AddListener(OnObjectExit);
            trigger.OnEnterTrigger.AddListener(OnObjectEenter);
        }
        private void OnObjectEenter(GameObject gameObject)
        {
            if (gameObject.tag == "Ball")
            {
                objectRenderer = gameObject.GetComponent<MeshRenderer>();

                TweenParams tParms = new TweenParams().SetEase(Ease.Linear);
                _box.transform.DOScale(new Vector3(1.25f, 1.25f, 1.25f), 0.65f).SetAs(tParms);
            }
        }
        private void OnObjectExit(GameObject gameObject)
        {
            if (gameObject.tag == "Ball")
            {
                // Check if a new material is assigned
                if (_newMaterial != null)
                {
                    // Change the texture of the object
                    objectRenderer.material = _newMaterial;

                    TweenParams tParms = new TweenParams().SetEase(Ease.OutBounce);
                    _box.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f).SetAs(tParms);
                }
            }
        }
    }
}

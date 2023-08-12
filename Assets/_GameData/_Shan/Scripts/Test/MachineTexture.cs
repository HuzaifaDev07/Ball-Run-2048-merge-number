using NaughtyAttributes;
using UnityEngine;

namespace ArcadeIdle.Shan
{
    public class MachineTexture : MonoBehaviour
    {
        [BoxGroup("Texture to Change")]
        public Texture _newTexture;

        [SerializeField]private MeshRenderer[] objectRenderer;
        [SerializeField]private MeshRenderer objectRendererSecond;
        // Start is called before the first frame update
        void Start()
        {
            foreach (MeshRenderer meshRenderer in objectRenderer)
            {
                meshRenderer.material.mainTexture = _newTexture;
            }
            objectRendererSecond.materials[1].mainTexture = _newTexture;
        }
    }
}

using UnityEngine;
using TMPro;
using Dreamteck.Splines;
using DG.Tweening;

namespace Hz.PlayerController
{
    public class PlayerController : MonoBehaviour
    {
        public TextMeshPro _ScoreText;
        public Rigidbody _Rb;
        public SplineFollower SplineFollower;
        [SerializeField] Renderer _MeshRenderer;
        [SerializeField] Color _Color;
        [SerializeField] float HorizontalSpeed;
        [SerializeField] float movement;
        [SerializeField] bool MoveNow;
        [SerializeField] DOTweenAnimation ScaleChanger;
        private void Start()
        {
            //MeshRenderer
            MaterialPropertyBlock Mpb = new();
            _MeshRenderer.SetPropertyBlock(Mpb);
            Mpb.SetColor(0, _Color);
            _MeshRenderer.SetPropertyBlock(Mpb);
        }
        public void Update()
        {
            if (MoveNow)
            {
                // Move the player horizontally based on mouse movement
                movement = Input.GetAxis("Horizontal") * HorizontalSpeed * Time.fixedDeltaTime;
                transform.Translate(new Vector3(movement, 0, 0));
            }
        }

        public void MovementCheck(bool check)
        {
            MoveNow = check;
        }

        public void DisableSplineY(bool check)
        {
            SplineFollower.motion.applyPositionY = check;
            if (check)
            {
                _Rb.constraints = RigidbodyConstraints.FreezePositionY;
            }
            else
            {
                _Rb.constraints = RigidbodyConstraints.None;
                _Rb.constraints = RigidbodyConstraints.FreezeRotation;
            }

        }
    }
}


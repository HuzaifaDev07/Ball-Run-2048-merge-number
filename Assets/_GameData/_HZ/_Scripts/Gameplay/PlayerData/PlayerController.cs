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

        [SerializeField] float HorizontalSpeed;
        [SerializeField] float movement;
        [SerializeField] bool MoveNow;
        public MaterialPropertyBlock _MyMpb;
        [SerializeField] DOTweenAnimation ScaleChanger;
        private void Start()
        {
            //MeshRenderer
            MaterialPropertyBlock Mpb = new();
            _MeshRenderer.SetPropertyBlock(Mpb);
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

        public void ControlBySplinePos(bool Check)
        {
            if (Check)
            {
                SplineFollower.motion.applyPositionX = true;
                SplineFollower.motion.applyPositionY = true;
                SplineFollower.motion.applyPositionZ = true;
            }
            else
            {
                SplineFollower.motion.applyPositionX = false;
                SplineFollower.motion.applyPositionY = false;
                SplineFollower.motion.applyPositionZ = false;
            }
        }
        public void ControlBySplinePosX(bool Check)
        {
            if (Check)
            {
                transform.rotation = Quaternion.Euler(0, -90, 0);
                SplineFollower.motion.applyPositionX = true;
                SplineFollower.motion.applyPositionZ = false;
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
                SplineFollower.motion.applyPositionX = false;
                SplineFollower.motion.applyPositionZ = true;
            }
        }
    }
}


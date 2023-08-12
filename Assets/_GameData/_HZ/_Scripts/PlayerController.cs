using UnityEngine;
using TMPro;
namespace Huzaifa
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] float maxXLimit = 4;
        public int score;
        public TextMeshPro scoreText;
        private Animator playerAnim;
        public float speed;
        [SerializeField] Rigidbody rb;
        public float forceLimit;
        public Vector3 direction = Vector3.forward;
        public int Score
        {
            get { return score; }
        }

        //void Start()
        //{
        //    playerAnim = GetComponent<Animator>();
        //    rb = GetComponent<Rigidbody>();
        //}

        private void FixedUpdate()
        {
            // Calculate the force to apply
            Vector3 force = direction.normalized * speed;

            // Limit the force to the specified force limit
            if (force.magnitude > forceLimit)
            {
                force = force.normalized * forceLimit;
            }

            // Apply the force
            rb.AddForce(force, ForceMode.Impulse);
            XBounds();
        }

        public void ScoreStatus()
        {
            scoreText.text = score.ToString();
        }

        void XBounds()
        {
            if (transform.position.x > maxXLimit)
            {
                transform.position = new Vector3(maxXLimit, transform.position.y, transform.position.z);
            }
            if (transform.position.x < -maxXLimit)
            {
                transform.position = new Vector3(-maxXLimit, transform.position.y, transform.position.z);
            }
        }
    }

}

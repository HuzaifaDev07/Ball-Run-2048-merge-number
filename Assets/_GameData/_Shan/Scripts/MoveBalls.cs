using UnityEngine;

namespace ArcadeIdle.Shan
{
    public class MoveBalls : MonoBehaviour
    {
        GameObject ball;
        // Start is called before the first frame update
        void Start()
        {
            SubscribeEvents();
        }
        private void SubscribeEvents()
        {
            var Trigger = GetComponent<Trigger>();
            Trigger.OnEnterTrigger.AddListener(GameObjectEnter);
        }
        private void GameObjectEnter(GameObject gameObject)
        {
            if (gameObject.tag == "Ball")
            {
                ball = gameObject;
                ApplyForce();
            }
        }

        private void ApplyForce()
        {
            var Rigidbody = ball.GetComponent<Rigidbody>();
            Rigidbody.AddForce(transform.forward * 100);
        }
    }
}

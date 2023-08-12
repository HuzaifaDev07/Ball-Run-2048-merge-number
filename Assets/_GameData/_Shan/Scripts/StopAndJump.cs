using System.Collections;
using UnityEngine;
using NaughtyAttributes;

namespace ArcadeIdle.Shan
{
    public class StopAndJump : MonoBehaviour
    {
        [BoxGroup("Animation")]
        [SerializeField] Transform _startPoint;
        [BoxGroup("Animation")]
        [SerializeField] Transform _endPoint;


        [SerializeField] private float speed = 15f;         // Speed of the animation
        [SerializeField] private float height = 2.75f;        // Height of the parabola

        GameObject ball;
        AnimateBalls _Animation;

        // Start is called before the first frame update
        void Start()
        {
            SubscribeEvents();
        }
        private void SubscribeEvents()
        {
            var Trigger = GetComponent<Trigger>();
            Trigger.OnEnterTrigger.AddListener(GameObjectEnter);
            _Animation = GetComponent<AnimateBalls>();
        }
        private void GameObjectEnter(GameObject gameObject)
        {
            if (gameObject.tag == "Ball")
            {
                ball = gameObject;
                StopAndAnimate();
            }
        }

        private void StopAndAnimate()
        {
            var Rigidbody = ball.GetComponent<Rigidbody>();
           /* Rigidbody.useGravity = false;
            Rigidbody.isKinematic = true;
            Rigidbody.velocity = Vector3.zero;*/
            //_Animation.ParabolicAnimation(ball.transform, _startPoint, _endPoint, speed, height);
        }

    }
}

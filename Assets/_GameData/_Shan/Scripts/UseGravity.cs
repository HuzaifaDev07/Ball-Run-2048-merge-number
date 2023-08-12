using UnityEngine;


namespace ArcadeIdle.Shan
{
    public class UseGravity : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var Trigger = GetComponent<Trigger>();
            Trigger.OnEnterTrigger.AddListener(OnEnterCollision);
        }
        private void OnEnterCollision(GameObject gameObject)
        {
            if (gameObject.CompareTag("Ball"))
            {
                Destroy(gameObject);
            }
        }
    }
}

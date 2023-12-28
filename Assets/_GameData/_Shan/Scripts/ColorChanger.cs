using UnityEngine;
using UnityEngine.UI;


namespace ArcadeIdle.Shan
{
    public class ColorChanger : MonoBehaviour
    {
        [SerializeField] Image colorFiller;
        // Start is called before the first frame update
        void Start()
        {
            SubscribeEvents();
        }
        private void SubscribeEvents()
        {
            var trigger = GetComponent<Trigger>();
            trigger.OnEnterTrigger.AddListener(OnObjectEnter);
            trigger.OnExitTrigger.AddListener(OnObjectExit);
        }
        private void OnObjectEnter(GameObject gameObject)
        {
            if (gameObject.tag == "Player")
            {
                colorFiller.fillAmount = 1;
            }
        }
        private void OnObjectExit(GameObject gameObject)
        {
            if (gameObject.tag == "Player")
            {
                colorFiller.fillAmount = 0;
            }
        }
    }
}

using NaughtyAttributes;
using UnityEngine;
using DG.Tweening;
using System.Collections;


namespace ArcadeIdle.Shan
{
    public class ColorMachine : MonoBehaviour
    {
        [BoxGroup("Color to Change")]
        public Color _newColor = Color.red;
        [BoxGroup("Color to Change")]
        [SerializeField]
        private float _delayTime = 0.1f;
        //[BoxGroup("Paint Object")] [SerializeField] GameObject _color;
        [BoxGroup("Paint Object")] [SerializeField] GameObject _colorParticle;
        //[BoxGroup("Color Pack")]   [SerializeField] GameObject _colorPackPrefab;
        [BoxGroup("Machine Animator")] [SerializeField] DOTweenVisualManager _machineAnimator;

        [BoxGroup("Ball Position InMas")] [SerializeField] Transform _ballPosition;


        private MeshRenderer objectRenderer;
        private MoveObjectInTrigger moveScript;
        private Rigidbody ballRigidbody;

        //private float colorValue;
        //private const float colorToDecrease = 0.15f;

        private GameObject ball;
        //private AnimateBalls _Animation;

        //public float ColorValue
        //{
        //    get => colorValue;
        //}

        // Start is called before the first frame update
        void Start()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            var trigger = GetComponent<Trigger>();
            //trigger.OnExitTrigger.AddListener(OnObjectExit);
            trigger.OnEnterTrigger.AddListener(OnObjectEenter);
            //colorValue = 1f;
            //_Animation = GetComponent<AnimateBalls>();
            //_machineAnimator.enabled = false;
        }
        private void OnObjectEenter(GameObject gameObject)
        {
            if (gameObject.tag == "Ball")
            {
                ball = gameObject;
                StartCoroutine(ChangeColor());
            }
            /*else if (gameObject.tag == "Player")
            {
                this.player = gameObject;
                PlayerPicker playerPicker = player.GetComponent<PlayerPicker>();

                if (playerPicker.ColorPaclCount > 0)
                    AddColorByPlayer();
            }*/
        }
        private IEnumerator ChangeColor()
        {
            _machineAnimator.enabled = true;
            objectRenderer = ball.GetComponent<MeshRenderer>();
            moveScript = ball.GetComponent<MoveObjectInTrigger>();
            ballRigidbody = ball.GetComponent<Rigidbody>();
            moveScript.enabled = false;
            ball.transform.position = _ballPosition.position;
            ball.transform.rotation = _ballPosition.rotation;
            ballRigidbody.constraints = RigidbodyConstraints.FreezeAll;

            _colorParticle.SetActive(true);
            yield return new WaitForSeconds(_delayTime / 2);
            objectRenderer.material.color = _newColor;

            yield return new WaitForSeconds(_delayTime / 2);
            ballRigidbody.constraints = RigidbodyConstraints.None;
            _colorParticle.SetActive(false);
            _machineAnimator.enabled = false;
        }


        private void OnObjectExit(GameObject gameObject)
        {
            if (gameObject.tag == "Ball")
            {
                // Check if a new material is assigned
                if (_newColor != null)
                {
                    // Change the material of the object
                    objectRenderer.material.color = _newColor;
                    //DecreaseColorValue();
                }
            }
        }
        //private void DecreaseColorValue()
        //{
        //    if (colorValue > 0)
        //    {
        //        colorValue -= colorToDecrease;
        //        colorValue = colorValue < 0 ? 0 : colorValue;
        //        _color.transform.DOScaleY(colorValue, 0.25f);
        //        if (colorValue <= 0)
        //        {
        //            _colorParticle.SetActive(false);
        //            _machineAnimator.enabled = false;
        //        }
        //    }
        //}
        //private void AddColorByPlayer()
        //{
        //    StartCoroutine(AddColorToMachine());
        //}
        //private IEnumerator AddColorToMachine()
        //{
        //    PlayerPicker playerPicker = player.GetComponent<PlayerPicker>();

        //    yield return new WaitForSeconds(0.25f);
        //    var Cube = Instantiate(_colorPackPrefab, this.transform).transform;
        //    _Animation.ParabolicAnimation(Cube, playerPicker.AnimationEndPoint(), _color.transform, () => {
        //        //playerPicker.ColorPackEmpty();
        //        _ballGenerator.ColorMachineCount = 1;
        //        colorValue = 1;
        //        _color.transform.DOScaleY(colorValue, 0.35f);
        //        _colorParticle.SetActive(true);
        //        _machineAnimator.enabled = true;
        //    });
        //    yield return null;
        //}
    }
}

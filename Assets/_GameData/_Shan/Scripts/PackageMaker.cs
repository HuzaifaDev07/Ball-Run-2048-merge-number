using System.Collections;
using UnityEngine;
using NaughtyAttributes;

namespace ArcadeIdle.Shan
{
    public class PackageMaker : MonoBehaviour
    {
        public delegate void OnPackageCompleted();
        public event OnPackageCompleted PackageCompleted;

        [BoxGroup("Basket")] [SerializeField] GameObject _emptyBasketPrefab;
        [BoxGroup("Basket")] [SerializeField] GameObject _filledBasketPrefab;
        [BoxGroup("Basket")] [SerializeField] Transform _spawnPoint;
        [BoxGroup("Basket")] [SerializeField] Transform _packagePoint;
        [BoxGroup("Rotate Roller")]
        [SerializeField] RotateRoller _rotateRoller;
        [BoxGroup("Machine")]
        [SerializeField] BallGeneratorMachine _ballGeneratorMachine;

        private GameObject basketObject;
        private AnimateBalls _Animation;

        int ballCount;
        void Start()
        {
            ballCount = 0;
            SubscribeEvents();
            StartCoroutine(SpawnEmptyBoxDelay());
        }
        private IEnumerator SpawnEmptyBoxDelay()
        {
            yield return new WaitForSeconds(1f);
            SpawnEmptyBasket();
        }
        private void SpawnEmptyBasket()
        {
            basketObject = Instantiate(_emptyBasketPrefab, transform);

            basketObject.SetActive(true);
            basketObject.transform.position = _spawnPoint.position;
            basketObject.transform.rotation = _spawnPoint.rotation;
        }
        private void SubscribeEvents()
        {
            var Trigger = GetComponent<Trigger>();
            Trigger.OnExitTrigger.AddListener(GameObjectEnter);
            _Animation = GetComponent<AnimateBalls>();
        }
        private void GameObjectEnter(GameObject gameObject)
        {
            if (gameObject.tag == "Ball")
            {
                if (_ballGeneratorMachine.SpawnBallsCount == 0)
                {
                    _rotateRoller.direction = 0;
                }
                
                gameObject.tag = "Fruit";
                var ball = gameObject;
                ball.transform.SetParent(basketObject.transform);
                ballCount += 1;
                if (ballCount == 1)
                {
                    ballCount = 0;
                    MakePackage();
                }
            }
        }

        private void MakePackage()
        {
            Destroy(basketObject);
            var FillBasket = Instantiate(_filledBasketPrefab, transform);
            FillBasket.SetActive(true);
            FillBasket.transform.position = _spawnPoint.position;
            FillBasket.transform.rotation = _spawnPoint.rotation;
            StartCoroutine(PlayAnimation(FillBasket.transform));
        }

        private IEnumerator PlayAnimation(Transform Object)
        {
            yield return new WaitForSeconds(0.5f);
            _Animation.ParabolicAnimation(Object, _spawnPoint, _packagePoint,()=> {
                PackageCompleted();
                SpawnEmptyBasket();
            });
        }

    }
}

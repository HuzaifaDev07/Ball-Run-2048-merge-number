using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;

namespace ArcadeIdle.Shan
{
    public class BallGeneratorMachine : MonoBehaviour
    {
        [BoxGroup("Ball Prefab")]
        [SerializeField] GameObject _emptyBallPrefab;
        [BoxGroup("Ball Prefab")]
        [SerializeField] GameObject _machineBallPrefab;
        [BoxGroup("Ball Prefab")]
        [SerializeField] GameObject _ballPrefabToAnimate;
        [BoxGroup("Ball Prefab")]
        [SerializeField] Transform _spawnPoint;
        [BoxGroup("Ball Prefab")]
        [SerializeField] Transform _spawnPointInPipe;
        [BoxGroup("Ball Prefab")]
        [SerializeField] private Transform[] _ballSpawnPoints;

        [BoxGroup("Ball Generator")]
        [SerializeField] private GameObject _ballGenerator;
        [BoxGroup("Ball Generator")]
        [SerializeField] private GameObject _ballGeneratorAtStart;
        [BoxGroup("Ball Generator")]
        [SerializeField] private GameObject _smoke;

        [BoxGroup("DELAY")]
        [ShowIf("_useDelay")]
        [SerializeField]
        private float _delayTime = 0.1f;

        [InfoBox("Delay is only used for OnEnterPlacement.")]
        [BoxGroup("DELAY")]
        [SerializeField]
        private bool _useDelay;

        [BoxGroup("Wait Time")]
        [SerializeField] float _waitTime;
        [BoxGroup("Wait Time")]
        [SerializeField] float _ballDropTime = 1f;
        [BoxGroup("Wait Time")]
        [SerializeField] float _ballGenerateTime = 1f;

        [BoxGroup("Machine")]
        [SerializeField] RotateObject _fan;
        [BoxGroup("Rotate Roller")]
        [SerializeField] RotateRoller _rotateRoller;
        [BoxGroup("Package Maker")]
        [SerializeField] GameObject _packageMaker;

        //Scale for bottle animation
        [SerializeField] Vector3 scale;

        private GameObject player;
        private readonly List<GameObject> _spawnedBalls = new List<GameObject>();
        private AnimateBalls _Animation;

        private Cashier cashier;
        private bool _fillingStock = false;

       // private float colorMachineCount;

        /*public float ColorMachineCount
        {
            get => colorMachineCount;
            set => colorMachineCount = value;
        }*/

        public bool instantiate = false;

        public int SpawnBallsCount
        {
            get => _spawnedBalls.Count;
        }
        // Start is called before the first frame update
        void Start()
        {
            SubscribeEvents();
            StartCoroutine(SpawnLoop());
        }

        private void SubscribeEvents()
        {
            var Trigger = GetComponent<Trigger>();
            Trigger.OnEnterTrigger.AddListener(GameObjectEnter);
            _Animation = GetComponent<AnimateBalls>();
            //colorMachineCount = 0;
            _packageMaker.SetActive(true);
        }
        private void GameObjectEnter(GameObject gameObject)
        {
            if (gameObject.tag == "Player")
            {
                player = gameObject;
                ManageBallsByPlayer();

                
            }
        }
        private IEnumerator SpawnLoop()
        {
            while (true)
            {
                _fan.rotationSpeed = 0;
                yield return new WaitWhile(() => SpawnBallsCount <= 0);
                _fan.rotationSpeed = -200;
                yield return new WaitForSeconds(_waitTime);

                _rotateRoller.direction = -1;
                TweenParams tParms = new TweenParams().SetEase(Ease.Linear);
                _ballGeneratorAtStart.transform.DOScale(scale, 0.9f).SetAs(tParms).OnComplete(() => {

                    OnNewBallGenerate(_spawnedBalls[SpawnBallsCount - 1]);

                    tParms = new TweenParams().SetEase(Ease.OutBounce);

                    _ballGeneratorAtStart.transform.DOScale(new Vector3(1f, 1f, 1f), 0.45f).SetAs(tParms).OnComplete(() =>
                    {
                        StartCoroutine(AnimateSecondMachine());
                    });
                });

                yield return new WaitForSeconds(_ballGenerateTime);
                SpawnBallPrefabInPipe();
                /*if(colorMachineCount > 0f)
                {
                    colorMachineCount -= 0.15f;
                }*/
            }
        }
        private IEnumerator AnimateSecondMachine()
        {
            yield return new WaitForSeconds(_waitTime);

            TweenParams tParms = new TweenParams().SetEase(Ease.Linear);
            _ballGenerator.transform.DOScale(new Vector3(1.2f, 0.65f, 1.2f), 0.8f).SetAs(tParms).OnComplete(() => {

                tParms = new TweenParams().SetEase(Ease.OutBounce);
                _smoke.SetActive(true);
                _ballGenerator.transform.DOScale(new Vector3(1f, 1f, 1f), 0.6f).SetAs(tParms).OnComplete(() =>
                {
                    _smoke.SetActive(false);
                });
            });

            yield return new WaitForSeconds(_ballDropTime);
            _spawnPoint.gameObject.SetActive(false);
            SpawnMachineBallPrefab();
            yield return new WaitForSeconds(_ballDropTime / 2);
            _spawnPoint.gameObject.SetActive(true);
        }
        private void SpawnMachineBallPrefab()
        {
            var ball = Instantiate(_machineBallPrefab, transform);
            ball.transform.position = _spawnPoint.position;
            ball.transform.rotation = _spawnPoint.rotation;
            ball.SetActive(true);
        }
        private GameObject SpawnBallPrefabInPipe()
        {
            var ball = Instantiate(_emptyBallPrefab, transform);
            ball.transform.position = _spawnPointInPipe.position;
            ball.transform.rotation = _spawnPointInPipe.rotation;
            ball.SetActive(true);
            return ball;
        }
        private void ManageBallsByPlayer()
        {
            SpecialBackpack playerBackpack = player.GetComponent<PlayerPicker>()._specialBackpackForBalls;
            if (playerBackpack.ItemsCount > 0 && playerBackpack.ShowedItemsType == _ballPrefabToAnimate.GetComponent<Fruit>().resourceType)
            {
                StartCoroutine(AddBallsToCounterByPlayer());
                bool AddBallsShown = SaveSystem.Instance.Data.AddBallsToMachine;
                if (!AddBallsShown)
                {
                    SaveSystem.Instance.Data.AddBallsToMachine = true;
                    SaveSystem.Instance.SaveData();
                    PlayerController.Instance.DisableNavmeshDraw();
                }
            }
        }
        private IEnumerator AddBallsToCounterByPlayer()
        {
            PlayerPicker playerPicker = player.GetComponent<PlayerPicker>();
            SpecialBackpack playerBackpack = playerPicker._specialBackpackForBalls;

            int ballCount = playerPicker.PlayerBallsCount();
            //ballCount += SpawnBallsCount;

            int playerCapacity = playerPicker.PlayerCapacityCount();

            yield return new WaitForSeconds(_delayTime);
            while (ballCount > 0 && SpawnBallsCount < _ballSpawnPoints.Length)
            {
                _fillingStock = true;
                AnimationState _animationState = AnimationState.Running;
                var ball = Instantiate(_ballPrefabToAnimate, this.transform).transform;

                playerBackpack.RemoveItems(1);
                _Animation.ParabolicAnimation(ball, playerPicker.AnimationEndPoint(), _ballSpawnPoints[SpawnBallsCount], () => {

                    SpawnBall();
                    _animationState = AnimationState.Complete;
                });

                yield return new WaitWhile(() => _animationState != AnimationState.Complete);
                playerPicker.HideMaxText();
                ballCount = playerPicker.PlayerBallsCount();
            }
            if (playerBackpack.ItemsCount < playerCapacity)
                playerPicker.HideMaxText();
            ballCount = playerPicker.PlayerBallsCount();
            if (ballCount == 0)
            {
                playerPicker.PlayerAnimatorUpdate(false);
            }

            yield return null;
            _fillingStock = false;
        }
        private void SpawnBall()
        {
            var package = Instantiate(_ballPrefabToAnimate, _ballSpawnPoints[SpawnBallsCount]);
            _spawnedBalls.Add(package);
        }
        private void OnNewBallGenerate(GameObject ball)
        {
            if (ball != null)
            {
                _spawnedBalls.Remove(ball);
                Destroy(ball);
            }
        }
        /*private void OnValidate()
        {
            if (instantiate)
            {
                SpawnNumberPrefab();
            }
        }*/
    }
}

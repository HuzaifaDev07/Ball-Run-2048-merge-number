using System.Collections;
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;

namespace ArcadeIdle.Shan
{
    public class BallsGenerator : MonoBehaviour
    {
        [BoxGroup("Ball Prefab")]
        [SerializeField] GameObject _ballPrefab;
        [BoxGroup("Ball Prefab")]
        [SerializeField] Transform _spawnPoint;
        [BoxGroup("Ball Prefab Count")]
        [SerializeField] int _ballsCount;

        [BoxGroup("Wait Time")]
        [SerializeField] float _waitTime;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(SpawnLoop());
        }

        private IEnumerator SpawnLoop()
        {
            Rigidbody ballRigidBody = null;
            while (true)
            {
                yield return new WaitForSeconds(_waitTime);
                ballRigidBody = SpawnNumberPrefab().GetComponent<Rigidbody>();
                ballRigidBody.useGravity = true;

            }
        }

        private GameObject SpawnNumberPrefab()
        {
            var ball = Instantiate(_ballPrefab, transform);
            ball.transform.position = _spawnPoint.position;
            ball.transform.rotation = _spawnPoint.rotation;
            ball.SetActive(true);
            return ball;
        }
    }
}

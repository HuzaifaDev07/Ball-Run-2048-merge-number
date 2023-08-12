using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;
using PathCreation;
using PathCreation.Examples;

namespace ArcadeIdle.Shan
{
    public class MachineBox : MonoBehaviour
    {
        [BoxGroup("Next Number Prefab")]
        [SerializeField] GameObject _nextNumberPrefab;
        [BoxGroup("Next Number Prefab")]
        [SerializeField]
        private Transform _spawnPoint;
        [BoxGroup("Next Number Target")]
        [SerializeField]
        private Transform _targetPoint;
        [BoxGroup("Path to Follow")]
        [SerializeField]
        private PathCreator _pathToFollow;

        public bool play = false;
        public Vector3 scale;

        private List<GameObject> numbersAtMachine = new List<GameObject>();

        // Start is called before the first frame update
        void Start()
        {

        }

        public void SpawnNextNumber()
        {

            TweenParams tParms = new TweenParams().SetEase(Ease.OutBounce);

            transform.DOScale(scale, 0.175f).OnComplete(()=> {
                transform.DOScale(new Vector3(1,1,1), 0.175f).OnComplete(() =>
                {
                    var ball = Instantiate(_nextNumberPrefab, _spawnPoint);
                    TweenParams tParms = new TweenParams().SetEase(Ease.Linear);
                    ball.transform.DOMove(_targetPoint.position, 1).SetAs(tParms).OnComplete(() =>
                    {
                        var PathFollower = ball.GetComponent<PathFollower>();
                        PathFollower.pathCreator = _pathToFollow;
                        PathFollower.enabled = true;
                    });
                });
            });



        }
        /*private void OnValidate()
        {
            if(play)
                SpawnNextNumber();
        }*/
    }
}

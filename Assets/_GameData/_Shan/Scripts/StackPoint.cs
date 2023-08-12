using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace ArcadeIdle.Shan
{
    public class StackPoint : MonoBehaviour
    {
        [BoxGroup("Number Prefab")]
        [SerializeField] GameObject _numberPrefab;
        [BoxGroup("Number Prefab")]
        [SerializeField]
        private Transform[] _spawnPoints;

        [BoxGroup("Paths To Assigns")]
        [SerializeField] Transform[] _paths;

        [BoxGroup("Machine Start Roller")]
        [SerializeField] MachineRoller machineRoller;

        [BoxGroup("Wait Time")]
        [SerializeField] float _waitTime;

        [BoxGroup("Start Stack")]
        [SerializeField] bool _startStack;

        private List<GameObject> numbersAtStack = new List<GameObject>();



        // Start is called before the first frame update
        void Start()
        {
            SubscribeEvents();
            StartCoroutine(StackMerge());
            if(_startStack)
                StartCoroutine(SpawnLoop());
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
                SpawnNumberPrefab();
                Destroy(gameObject);
            }
        }
        private IEnumerator SpawnLoop()
        {
            while (true)
            {
                while (numbersAtStack.Count < _spawnPoints.Length)
                {
                    SpawnNumberPrefab();
                    yield return new WaitForSeconds(_waitTime / 6);
                }
                yield return new WaitForSeconds(_waitTime);
            }
        }
        private void SpawnNumberPrefab()
        {
            var number = Instantiate(_numberPrefab, _spawnPoints[numbersAtStack.Count]);
            numbersAtStack.Add(number);
        }

        private IEnumerator StackMerge()
        {
            while (true)
            {
                yield return new WaitForSeconds(_waitTime);
                if (numbersAtStack.Count >= 2)
                {
                    foreach (Transform transform in _paths)
                    {
                        var number = numbersAtStack[numbersAtStack.Count - 1];
                        number.transform.position = transform.position;
                        numbersAtStack.Remove(number);
                        machineRoller.MoveToMachine(number);
                    }
                }
            }
        }
    }
}

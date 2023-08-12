using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace ArcadeIdle.Shan
{
    public class MoneyPack : MonoBehaviour
    {
        [BoxGroup("Parent Object")]
        [SerializeField] Transform _parentObject;
        [BoxGroup("Item Type")]
        [SerializeField] AnimationType itemType;
        [BoxGroup("Note Prefab to Spawn")]
        [SerializeField] GameObject _notePrefab; // The prefab to be instantiated
        [BoxGroup("Note Prefab to Spawn")]
        [SerializeField] GameObject _notePrefabToAnimate; // The prefab to be instantiated
        [BoxGroup("Animation Related")]
        [SerializeField] Transform _startPoint;
        Transform _endPoint;
        [BoxGroup("Points To Add Cash")]
        [SerializeField] Transform _pointOne;
        [BoxGroup("Points To Add Cash")]
        [SerializeField] Transform _pointTwo;

        private readonly List<GameObject> notePrefabList = new();

        private AnimateBalls _Animation;

        GameObject player;


        [SerializeField] int rows = 3;      // Number of rows in the square formation
        [SerializeField] int columns = 3;   // Number of columns in the square formation
        [SerializeField] float rowSpacing = 0.255f; // Spacing between objects in the square
        [SerializeField] float colSpacing = 0.465f; // Spacing between objects in the square

        float prefabHeight;
        [SerializeField]
        public float animationSpeed = 25f;
        [SerializeField]
        public float animationHeight = 3f;
        [SerializeField]
        public float delayTime = 0.5f;
        public bool instantiate = false;
        const int countAtMainDoor = 16;



        int totalResources = 0;
        int reserveResources = 0;
        int rowNum;
        int colNum;
        bool gameStartPriceGiven = false;

        /*private void OnValidate()
        {
            reserveResources = totalResources = 0;
            prefabHeight = _notePrefab.GetComponent<Renderer>().bounds.size.y;
            if (instantiate)
            {
                SpawnSquareFormation(countAtMainDoor);
                notePrefabList.Clear();
            }
        }*/
        // Start is called before the first frame update
        void Start()
        {
            totalResources = reserveResources = 0;
            prefabHeight = _notePrefab.GetComponent<Renderer>().bounds.size.y;
            SubscribeEvents();
        }
        private void SubscribeEvents()
        {
            _Animation = GetComponent<AnimateBalls>();
            GetComponent<Trigger>().OnEnterTrigger.AddListener(OnEnterTrigger);
        }

        private void OnEnterTrigger(GameObject other)
        {
            if (notePrefabList.Count > 0 && other.tag == "Player")
            {
                player = other;
                StartCoroutine(AddCash());
                StartCoroutine(RemoveNotesFromPack());
            }
            else if (other.tag == "Ball")
            {
                Destroy(other);
                SpawnSquareFormation(8);
            }
        }

        public void SpawnSquareFormation(int numberOfNotes)
        {
            reserveResources = totalResources;
            totalResources += numberOfNotes;
            int reserveResourceCount = 0;
            //  float yOffset = 0;
            float loopCount = 0;
            while (notePrefabList.Count < totalResources - 1)
            {
                for (int row = 0; row < rows; row++)
                {
                    rowNum = row;
                    for (int col = 0; col < columns; col++)
                    {
                        colNum = col;
                        if (reserveResourceCount >= reserveResources)
                        {
                            Vector3 spawnPosition = new(-col * colSpacing, transform.localPosition.y + (prefabHeight * loopCount), -row * rowSpacing);
                            GameObject noteObject = Instantiate(_notePrefab, transform.localPosition, transform.localRotation, _parentObject.transform);
                            noteObject.transform.localPosition = spawnPosition;
                            noteObject.transform.localRotation = transform.localRotation;
                            notePrefabList.Add(noteObject);
                            if (notePrefabList.Count == numberOfNotes)
                                return;
                        }
                        reserveResourceCount++;
                    }
                    if (notePrefabList.Count < totalResources)
                        colNum = 0;
                }
                if (notePrefabList.Count < totalResources)
                    rowNum = 0;
                loopCount += 1.15f;
            }
        }

        private IEnumerator RemoveNotesFromPack()
        {
            int totalPrefabs = notePrefabList.Count;
            while (notePrefabList.Count > 0)
            {
                GameObject obj = notePrefabList[notePrefabList.Count - 1];
                notePrefabList.Remove(obj);
                Destroy(obj);
                totalResources = totalResources > 0 ? totalResources-- : 0;
                yield return new WaitForSeconds(0.075f);
            }
            yield return null;
        }

        private IEnumerator AddCash()
        {
            int revenue = notePrefabList.Count * 32;
            var playerPicker = player.GetComponent<PlayerPicker>();
            _endPoint = playerPicker.AnimationEndPoint();
            while (notePrefabList.Count > 0)
            {
                var note = Instantiate(_notePrefabToAnimate, _parentObject).transform;
                _Animation.ParabolicAnimation(note, _startPoint, _endPoint, () => {

                });
                //StartCoroutine(pickDropAnimation.AnimateParabola(note, _startPoint, _endPoint, animationSpeed, animationHeight));
                yield return new WaitForSeconds(0.075f);
            }
            colNum = 0;
            rowNum = 0;
            totalResources = 0;
            //SaveSystem.Instance.Data.GameStartPriceGiven = gameStartPriceGiven;
            //ResourcesSystem.Instance.AddResourceCount(ResourcesSystem.ResourceType.Banknotes, revenue);
            //SaveSystem.Instance.SaveData();
        }

        public void AnimateNotes(int numberOfNotes)
        {
            StartCoroutine(SpawnNotesOnly(numberOfNotes));
        }
        private IEnumerator SpawnNotesOnly(int numberOfNotes)
        {
            int notesCount = 0;
            while (notesCount < 4)
            {
                yield return new WaitForSeconds(0.05f);
                var note = Instantiate(_notePrefabToAnimate, this.transform);
                note.transform.position = _pointOne.position;
                note.transform.rotation = _pointOne.rotation;

                note.transform.DOMove(_pointTwo.position, 0.5f).OnComplete(() => {
                    DestroyObject(note);
                });
                notesCount++;
            }
            SpawnSquareFormation(numberOfNotes);
        }
        void DestroyObject(GameObject gameObject)
        {
            Destroy(gameObject);
        }

        /*private void ChangeAnimationState()
        {
            _animationState = AnimationState.Complete;
        }*/
        /*private int PriceMultiply()
        {
            ;
        }*/
        //Public fields

    }
}
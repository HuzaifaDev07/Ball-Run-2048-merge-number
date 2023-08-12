using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

namespace ArcadeIdle
{
    public class MoneyPack : MonoBehaviour
    {
        [BoxGroup("Parent Object")]
        [SerializeField] Transform parentObject;
        [BoxGroup("Money Type")]
        [SerializeField] MoneyAtPlace moneyAtPlace;
        [BoxGroup("Item Type")]
        [SerializeField] AnimationType itemType;
        [BoxGroup("Note Prefab to Spawn")]
        [SerializeField] GameObject _notePrefab; // The prefab to be instantiated
        [BoxGroup("Note Prefab to Spawn")]
        [SerializeField] GameObject _notePrefabToAnimate; // The prefab to be instantiated
        [BoxGroup("Animation Related")]
        [SerializeField] Transform _startPoint;
        Transform _endPoint;
        [BoxGroup("TUTORIAL TEXT")]
        [SerializeField]
        private bool _useTutorialText;
        [BoxGroup("TUTORIAL TEXT")]
        [ShowIf("_useTutorialText")]
        [SerializeField]
        private string _tutorialTextToDisplay;
        [ShowIf("_useTutorialText")]
        [BoxGroup("TUTORIAL TEXT")]
        [SerializeField]
        private GameObject[] _itemsToHide;
        [ShowIf("_useTutorialText")]
        [BoxGroup("TUTORIAL TEXT")]
        [SerializeField]
        private GameObject[] _itemsToShow;

        private readonly List<GameObject> notePrefabList = new List<GameObject>();

        PickDropAnimation pickDropAnimation;
        GameObject player;

        //private AnimationState _animationState;

        [SerializeField] int rows = 3;      // Number of rows in the square formation
        [SerializeField]int columns = 3;   // Number of columns in the square formation
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
        const int countAtMainDoor = 30;

        //float loopCount = 0;

        int totalResources = 0;
        int reserveResources = 0;
        int rowNum;
        int colNum;
        bool gameStartPriceGiven = false;

        /*private void OnValidate()
        {
            if (instantiate)
                SpawnSquareFormation(countAtMainDoor);
        }*/
        // Start is called before the first frame update
        void Start()
        {
            prefabHeight = _notePrefab.GetComponent<Renderer>().bounds.size.y;
            if (moneyAtPlace == MoneyAtPlace.MainDoor)
            {
                if (!gameStartPriceGiven)
                {
                    SpawnSquareFormation(countAtMainDoor);
                    gameStartPriceGiven = true;
                }
                else
                {
                    ShowItems();
                    HideItems();
                    gameObject.SetActive(false);
                }
            }

            SubscribeEvents();
        }
        private void SubscribeEvents()
        {
            pickDropAnimation = GetComponent<PickDropAnimation>();
            GetComponent<Trigger>().OnEnterTrigger.AddListener(OnEnterTrigger);
        }

        private void OnEnterTrigger(GameObject other)
        {
            if (notePrefabList.Count > 0)
            {
                player = other;
                StartCoroutine(AddCash());
                StartCoroutine(RemoveNotesFromPack());
            }
        }

        public void SpawnSquareFormation(int numberOfNotes)
        {
            /*reserveResources = totalResources;
            totalResources += numberOfNotes;
            int reserveResourceCount = 0;
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
                            Vector3 spawnPosition = new Vector3(-col * colSpacing, transform.localPosition.y + (prefabHeight * loopCount), -row * rowSpacing);
                            GameObject noteObject = Instantiate(_notePrefab, transform.localPosition, transform.localRotation, this.transform);
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
                if(notePrefabList.Count < totalResources)
                        rowNum = 0;
                loopCount += 1.35f;
            }*/
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
                yield return null;
            }
            yield return null;
        }

        private IEnumerator AddCash()
        {
            int revenue = notePrefabList.Count * PriceMultiply();
            var playerPicker = player.GetComponent<PlayerPicker>();
            _endPoint = playerPicker.AnimationEndPoint();
            while (notePrefabList.Count > 0)
            {
                var note = Instantiate(_notePrefabToAnimate, parentObject).transform;
                StartCoroutine(pickDropAnimation.AnimateParabola(note, _startPoint, _endPoint, animationSpeed, animationHeight));
                yield return new WaitForSeconds(delayTime);
            }
            colNum = 0;
            rowNum = 0;
            //loopCount = 0;
            totalResources = 0;
            SaveSystem.Instance.Data.GameStartPriceGiven = gameStartPriceGiven;
            ResourcesSystem.Instance.AddResourceCount(ResourcesSystem.ResourceType.Banknotes, revenue);
            SaveSystem.Instance.SaveData();
            if (_useTutorialText)
            {
                ShowItems();
                HideItems();
            }
        }

        /*private void ChangeAnimationState()
        {
            _animationState = AnimationState.Complete;
        }*/
        private int PriceMultiply()
        {
            if (moneyAtPlace == MoneyAtPlace.MainDoor)
                return 6;
            else
                return 1;
        }
        private void HideItems()
        {
            foreach (GameObject gameObject in _itemsToHide)
            {
                gameObject.SetActive(false);
            }
        }
        private void ShowItems()
        {
            foreach (GameObject gameObject in _itemsToShow)
            {
                gameObject.SetActive(true);
            }
        }
        //Public fields

    }
}
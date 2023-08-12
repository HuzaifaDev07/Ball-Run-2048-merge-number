using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

namespace ArcadeIdle
{
    [AddComponentMenu("Arcade Idle Components/Placement", 0)]
    public class Placement : UIElement
    {
        public delegate void OnAddingMoneyToPlacement(int price);
        public event OnAddingMoneyToPlacement MoneyAdded;

        [InfoBox("Delay is only used for OnEnterPlacement.")]
        [BoxGroup("DELAY")] [SerializeField] 
        private bool _useDelay;
        [BoxGroup("DELAY")] [ShowIf("_useDelay")] [SerializeField] 
        private float _delayTime;

        [BoxGroup("Note Prefab to Spawn")]
        [SerializeField] GameObject _notePrefabToAnimate; // The prefab to be instantiated
        [BoxGroup("Animation Related")]
        [SerializeField] Transform _endPoint; 

        [BoxGroup("PROGRESS BAR")] [SerializeField] 
        private bool _useProgressBar;
        [BoxGroup("PROGRESS BAR")] [ShowIf("_useProgressBar")] [SerializeField] [Required("Drag and drop progress bar here.")]
        private ProgressBar _progressBar;
        [BoxGroup("PROGRESS BAR")] [ShowIf("_useProgressBar")] [SerializeField] 
        private bool _resetOnExit;

        [BoxGroup("PROGRESS BAR")]
        [SerializeField]
        private bool _toStoreValue;

        [BoxGroup("PROGRESS BAR")]
        [ShowIf("_toStoreValue")]
        [SerializeField]
        private string _saveProgress;


        [InfoBox("Placement subscribes to Trigger events automatically, you do not need to do it manually.")]
        [BoxGroup("EVENT")]
        public UnityEvent<GameObject> OnEnterPlacement;
        [BoxGroup("EVENT")]
        public UnityEvent<GameObject> OnExitPlacement;

        private Transform startPoint;


        int currentResources;
        int resourcesInHand;
        int requiredResources;
        public int RequiredResources
        {
            get => requiredResources;
            set => requiredResources = value;
        }
        private void Awake()
        {
            ShowElement();
        }

        private void Start()
        {
            CheckForErrors();
            SubscribeEvents();
            CheckForPreviousValue();
        }
        
        private void CheckForErrors()
        {
            //if (Resources.Load<Settings>(Constants.SETTINGS).ShowWarnings)
            {
                if (_useDelay && _delayTime == 0)
                {
                    Debug.LogWarning("Placement: time delay is 0.");
                }

                if (_useProgressBar && _progressBar == null)
                {
                    Debug.LogError("Placement: progress bar is null.");
                }
            }
        }

        private void CheckForPreviousValue()
        {
            if (PlayerPrefs.HasKey(_saveProgress))
            {
                var value = PlayerPrefs.GetInt(_saveProgress);
                if (_useProgressBar)
                {
                    _progressBar.SetProgress(value, requiredResources);
                }
                resourcesInHand = value;
            }

        }
        private void SubscribeEvents()
        {
            var trigger = GetComponent<Trigger>();
            //Subscribes trigger events by script because it more safety
            //Events in inspector can accidentally resets and you'll to set it again
            trigger.OnEnterTrigger.AddListener(OnPlayerEnter);
            trigger.OnExitTrigger.AddListener(OnPlayerExit);
        }

        /// <summary>
        /// The method is called when the player enters the placement's collider.
        /// </summary>
        /// <param name="player">Link to object entered in collider.</param>
        public void OnPlayerEnter(GameObject player)
        {
            var name = gameObject.name;
            if (IsShowed == false) return;

            startPoint = player.GetComponent<PlayerAnimations>().AnimationEndPoint();
            if (_useDelay)
            {
                currentResources = ResourcesSystem.Instance.GetResourceCount(ResourcesSystem.ResourceType.Banknotes);
                if (currentResources > 0)
                {
                    StartCoroutine(WaitDelay(() =>
                    {
                        OnEnterPlacement?.Invoke(player);
                    }));
                    StartCoroutine(AnimateNotes());
                }
            }
            else
            {
                OnEnterPlacement?.Invoke(player);
            }
        }
        
        /// <summary>
        /// The method is called when the player exits the placement's collider.
        /// </summary>
        /// <param name="player">Link to object left the collider.</param>
        public void OnPlayerExit(GameObject player)
        {
            StopAllCoroutines(); //Stops WaitDelay coroutine 
            
            if (_useProgressBar && _resetOnExit)
            {
                _progressBar.ResetValues();
            }
            
            OnExitPlacement?.Invoke(player);
        }

        private IEnumerator WaitDelay(Action onComplete)
        {
            var resourceAdd = 0;
            resourceAdd += resourcesInHand;
            var currResources = currentResources;
            int toReduce = 0;

            if (requiredResources >= 150)
                toReduce = 5;
            else
                toReduce = 1;
            while (resourceAdd < requiredResources && currentResources > 0)
            {
                resourceAdd += toReduce;
                resourcesInHand = resourceAdd;
                currentResources -= toReduce;
                if (_useProgressBar)
                {
                    _progressBar.SetProgress(resourceAdd, requiredResources);
                }
                /*if (currentResources >= 100 && requiredResources >= 100)
                {
                    resourceAdd += 50;
                    resourcesInHand = resourceAdd;
                    currentResources -= 50;
                    if (_useProgressBar)
                    {
                        _progressBar.SetProgress(resourceAdd, requiredResources);
                    }
                }
                else
                {
                    resourceAdd += 1;
                    resourcesInHand = resourceAdd;
                    currentResources--;
                    if (_useProgressBar)
                    {
                        _progressBar.SetProgress(resourceAdd, requiredResources);
                    }
                }*/
                if (resourceAdd >= requiredResources)
                {
                    onComplete?.Invoke();
                    break; // Exit the loop since unlocking is complete
                }
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.15f);
            if (resourceAdd == requiredResources)
                onComplete?.Invoke();
            else
            {
                MoneyAddedToUnlock(currResources);
            }
        }
        private void MoneyAddedToUnlock(int price)
        {
            ResourcesSystem.Instance.AddResourceCount(ResourcesSystem.ResourceType.Banknotes, -price);
            MoneyAdded(price);
        }
        private IEnumerator AnimateNotes()
        {
            while (resourcesInHand < requiredResources && currentResources > 0)
            {
                var note = Instantiate(_notePrefabToAnimate, transform).transform;
                StartCoroutine(AnimateParabola(note, startPoint, _endPoint));

                /*var note = Instantiate(_notePrefabToAnimate, this.transform);
                note.transform.position = startPoint.position;
                note.transform.rotation = startPoint.rotation;

                note.transform.DOMove(_endPoint.position, 0.5f).OnComplete(() => {
                    DestroyObject(note);
                });*/

                yield return new WaitForSeconds(_delayTime);
            }
        }
        void DestroyObject(GameObject gameObject)
        {
            Destroy(gameObject);
        }
        private IEnumerator AnimateParabola(Transform objToAnimate, Transform startPosition, Transform endPosition, float speed = 4f, float height = 1f)
        {

            float distance = Vector3.Distance(startPosition.position, endPosition.position);
            distance = distance == 0 ? 1 : distance;
            float startTime = Time.time;       // Record the start time
            bool isAnimating = true;        // Flag to indicate if the animation is in progress

            while (isAnimating)
            {
                // Calculate the elapsed time since the animation started
                float elapsedTime = Time.time - startTime;

                // Calculate the normalized distance covered by the animation
                float normalizedDistance = elapsedTime * speed / distance;

                // Calculate the y position using a parabolic equation
                float yOffset = height * 4f * normalizedDistance * (1f - normalizedDistance);

                // Interpolate the position between start and end points based on the normalized distance
                Vector3 newPosition = Vector3.Lerp(startPosition.position, endPosition.position, normalizedDistance);
                newPosition += Vector3.up * yOffset;   // Apply the y offset

                // Move the object to the new position
                if(objToAnimate)
                    objToAnimate.position = newPosition;

                // Check if the animation has reached the end point
                if (normalizedDistance >= 1f && objToAnimate)
                {
                    Destroy(objToAnimate.gameObject);
                    isAnimating = false;    // Set the animation flag to false

                }

                yield return null;
            }
        }

        /// <summary>
        /// To save Progress fro current item
        /// </summary>
        private void OnApplicationQuit()
        {
            if(_toStoreValue)
                PlayerPrefs.SetInt(_saveProgress, resourcesInHand);
        }

        protected override void OnShow() { }

        protected override void OnHide() { }
    }
}
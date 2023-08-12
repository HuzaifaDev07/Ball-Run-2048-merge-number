using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using ArcadeIdle.Shan;

namespace ArcadeIdle
{
    public class UnlockItem : MonoBehaviour
    {
        #region ______Lock & Unlockable Items_______

        [BoxGroup("Items to Show on Unlock")]
        [SerializeField] GameObject _itemToShow;
        [BoxGroup("Items to Show on Unlock")]
        [SerializeField] GameObject[] _toActiveObjects;
        TweenParams tParms = new TweenParams().SetEase(Ease.OutBounce);
        [BoxGroup("Items to Hide")]
        [SerializeField]
        private bool _hideItemsAvailable;
        [BoxGroup("Items to Hide")] [ShowIf("_hideItemsAvailable")] [SerializeField]
        private GameObject[] _itemsToHide;

        #endregion ______Lock & Unlockable Items_______

        #region ______Camera Animations_______

        [BoxGroup("Camera Animation")]
        [SerializeField]
        private bool _playCameraAnimation;
        [BoxGroup("Camera Animation")]
        [ShowIf("_playCameraAnimation")]
        [SerializeField] PlayerCamera playerCamera;
        [BoxGroup("Camera Animation")]
        [ShowIf("_playCameraAnimation")]
        [SerializeField] Transform targetPosition;
        [BoxGroup("Camera Animation")]
        [ShowIf("_playCameraAnimation")]
        [SerializeField] Transform nextTargetPosition;
        [BoxGroup("Camera Animation")]
        [ShowIf("_playCameraAnimation")]
        [SerializeField] float moveSpeed = 2f;
        [BoxGroup("Camera Animation")]
        [ShowIf("_playCameraAnimation")]
        [SerializeField] float rotationSpeed = 2f;

        #endregion ______Camera Animations_______

        #region ______Confetti Particles______

        [BoxGroup("Confetti Particles")]
        [SerializeField]
        private bool _playConfetti;
        [BoxGroup("Confetti Particles")]
        [ShowIf("_playConfetti")]
        [SerializeField]
        private GameObject _particlesConfetti;

        #endregion ______Confetti Particles______

        #region ______Ribbon Animation______

        [BoxGroup("Door Ribbon")][SerializeField]
        private bool _ribbonAnimation;
        [BoxGroup("Door Ribbon")]
        [ShowIf("_ribbonAnimation")]
        [SerializeField] GameObject _ribbon;
        [BoxGroup("Door Ribbon")]
        [ShowIf("_ribbonAnimation")]
        [SerializeField] Rigidbody[] _ribbonObjects;

        #endregion ______Ribbon Animation______

        #region ______Next_Target______

        [BoxGroup("Next Target")][SerializeField]
        private bool _showNextTarget;
        [BoxGroup("Next Target")]
        [ShowIf("_showNextTarget")]
        [SerializeField] Transform _nextTarget;

        #endregion ______Next_Target______

        [BoxGroup("Unlock At")]
        [SerializeField] UnlockItemAt unlockItemAt;

        Transform playerCamPosition;

        bool isMoving = false;

        private Vector3 initialPosition;
        private Quaternion initialRotation;
        // Start is called before the first frame update
        void Start()
        {
            var unlockableItem = GetComponent<UnlockableItem>();
            if (unlockableItem.IsUnlocked)
            {
                AlreadyUnlock();
            }
            else
            {
                unlockableItem.OnUnlockItem.AddListener(OnUnlock);
            }
        }

        private void OnUnlock()
        {
            _itemToShow.SetActive(true);
            _itemToShow.transform.DOScale(new Vector3(1, 1, 1), 1f).SetAs(tParms);
            if (_playConfetti)
            {
                _particlesConfetti.SetActive(true);
            }
            if (_hideItemsAvailable)
            {
                HideItems();
            }
            ShowItems();

            if (_showNextTarget)
                PlayerController.Instance.ShowNextTargetNavmesh(_nextTarget);
            else
                PlayerController.Instance.DisableNavmeshDraw();

        }
        private void AlreadyUnlock()
        {
            _itemToShow.SetActive(true);
            _itemToShow.transform.localScale = new  Vector3(1, 1, 1);
            ShowItems();
            if (_hideItemsAvailable)
            {
                HideItems();
            }
            if (unlockItemAt == UnlockItemAt.MainDoor)
            {
                _ribbon.SetActive(false);
            }
            PlayerController.Instance.DisableNavmeshDraw();
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
            foreach (GameObject gameObject in _toActiveObjects)
            {
                gameObject.SetActive(true);
            }
        }
        private IEnumerator ShowAnimationMove()
        {
            ///////////////Next Tutorial Play
            ShowItems();
            yield return new WaitForSeconds(0.55f);
            playerCamera.enabled = false;
            Transform targetToReAssign = playerCamera.Target;
            Vector3 Offset = playerCamera.Offset;

            Vector3 camFirstPosition = playerCamera.transform.position;
            Quaternion camFirstRotation = playerCamera.transform.rotation;

            playerCamPosition = playerCamera.transform;

            initialPosition = playerCamera.transform.position;
            initialRotation = playerCamera.transform.rotation;

            StartCoroutine(MoveCoroutine());
            isMoving = true;
            yield return new WaitWhile(() => isMoving);
            yield return new WaitForSeconds(1f);

            initialPosition = playerCamera.transform.position;
            initialRotation = playerCamera.transform.rotation;

            StartCoroutine(MoveAndRotateCoroutine(camFirstPosition, camFirstRotation));
            isMoving = true;
            yield return new WaitWhile(() => isMoving);
            yield return new WaitForSeconds(0.35f);
            playerCamera.Target = targetToReAssign;
            yield return null;
            playerCamera.enabled = true;
            //playerCamera.Offset = Offset;
        }
        private IEnumerator ShowAnimationMoveAndRotate()
        {
            playerCamera.enabled = false;
            Transform targetToReAssign = playerCamera.Target;
            Vector3 Offset = playerCamera.Offset;

            Vector3 camFirstPosition = playerCamera.transform.position;
            Quaternion camFirstRotation = playerCamera.transform.rotation;

            playerCamPosition = playerCamera.transform;

            initialPosition = playerCamera.transform.position;
            initialRotation = playerCamera.transform.rotation;
            //Move near Door
            StartCoroutine(MoveCoroutine());
            isMoving = true;
            yield return new WaitWhile(() => isMoving);
            yield return new WaitForSeconds(0.15f);
            //Rotate Towards Door
            StartCoroutine(RotateCoroutine());
            isMoving = true;
            yield return new WaitWhile(() => isMoving);

            StartCoroutine(PlayRibbonAnimation());
            _particlesConfetti.SetActive(true);
            yield return new WaitForSeconds(2.5f);

            initialPosition = playerCamera.transform.position;
            initialRotation = playerCamera.transform.rotation;
            //move back to normal position
            StartCoroutine(MoveAndRotateCoroutine(camFirstPosition, camFirstRotation));
            isMoving = true;
            yield return new WaitWhile(() => isMoving);
            _ribbon.SetActive(false);
            _particlesConfetti.SetActive(false);
            yield return new WaitForSeconds(0.35f);
            playerCamera.Offset = Offset;

            initialPosition = playerCamera.transform.position;
            initialRotation = playerCamera.transform.rotation;
            targetPosition = nextTargetPosition;
            moveSpeed *= 2;
            ///////////////Next Tutorial Play
            ShowItems();
            //move towards next object to unlock
            StartCoroutine(MoveCoroutine());
            isMoving = true;
            yield return new WaitWhile(() => isMoving);
            yield return new WaitForSeconds(1f);

            initialPosition = playerCamera.transform.position;
            initialRotation = playerCamera.transform.rotation;
            //move back to normal position
            StartCoroutine(MoveAndRotateCoroutine(camFirstPosition, camFirstRotation));
            isMoving = true;
            yield return new WaitWhile(() => isMoving);
            yield return new WaitForSeconds(0.35f);
            playerCamera.Target = targetToReAssign;
            yield return null;
            playerCamera.enabled = true;
            //playerCamera.Offset = Offset;
        }
        private IEnumerator MoveAndRotateCoroutine(Vector3 targetPosition, Quaternion targetRotation)
        {
            isMoving = true;
            float startTime = Time.time;
            float journeyLength = Vector3.Distance(initialPosition, targetPosition);


            while (isMoving)
            {
                float distCovered = (Time.time - startTime) * moveSpeed;
                float fractionOfJourney = distCovered / journeyLength;

                // Move the object using linear interpolation
                playerCamera.transform.position = Vector3.Lerp(initialPosition, targetPosition, fractionOfJourney);

                // Rotate the object using slerp for a smooth rotation
                playerCamera.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, fractionOfJourney);

                // Check if the object has reached the target position and rotation
                if (fractionOfJourney >= 1.0f)
                {
                    isMoving = false;
                }

                yield return null; // Wait for the next frame
            }
        }
        private IEnumerator MoveCoroutine()
        {
            isMoving = true;
            float startTime = Time.time;
            float journeyLength = Vector3.Distance(initialPosition, targetPosition.position);


            while (isMoving)
            {
                float distCovered = (Time.time - startTime) * moveSpeed;
                float fractionOfJourney = distCovered / journeyLength;

                // Move the object using linear interpolation
                playerCamera.transform.position = Vector3.Lerp(initialPosition, targetPosition.position, fractionOfJourney);

                // Check if the object has reached the target position and rotation
                if (fractionOfJourney >= 1.0f)
                {
                    isMoving = false;
                }

                yield return null; // Wait for the next frame
            }
        }
        private IEnumerator RotateCoroutine()
        {
            isMoving = true;
            float startTime = Time.time;
            float journeyLength = Vector3.Distance(initialPosition, targetPosition.position);


            while (isMoving)
            {
                float distCovered = (Time.time - startTime) * moveSpeed;
                float fractionOfJourney = distCovered / journeyLength;

                // Rotate the object using slerp for a smooth rotation
                playerCamera.transform.rotation = Quaternion.Slerp(initialRotation, targetPosition.rotation, fractionOfJourney);

                // Check if the object has reached the target position and rotation
                if (fractionOfJourney >= 1.0f)
                {
                    isMoving = false;
                }

                yield return null; // Wait for the next frame
            }
        }
        private IEnumerator PlayRibbonAnimation()
        {
            foreach (Rigidbody rigidbody in _ribbonObjects)
            {
                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;
                yield return new WaitForSeconds(0.175f);
            }
        }
    }
}
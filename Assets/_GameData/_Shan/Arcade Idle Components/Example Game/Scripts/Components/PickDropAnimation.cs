using System.Collections;
using UnityEngine;
using NaughtyAttributes;
using System;

namespace ArcadeIdle
{
    public class PickDropAnimation : MonoBehaviour
    {
        [BoxGroup("ANIMATION")]
        [SerializeField]
        private AnimationType _itemType;

        private Transform startPoint;     // Starting point of the animation
        private Transform endPoint;       // Ending point of the animation
        private Transform objectToAnimate;       // Object to animate
        private float speed = 15f;         // Speed of the animation
        private float height = 2.75f;        // Height of the parabola

        private float distance;          // Distance between the start and end points
        private float startTime;         // Start time of the animation
        private bool isAnimating;        // Flag to indicate if the animation is in progress
        private SpecialBackpack _specialBackpack;

        public void ParabolicAnimation(Transform objToAnimate, Transform endPosition, AnimationType animationType = AnimationType.None, Action onComplete = null, SpecialBackpack specialBackpack = null,float speed = 15f)
        {
            _itemType = animationType;
            if (_itemType == AnimationType.PickItem || _itemType == AnimationType.Sale)
            {
                startPoint = this.transform;
                endPoint = endPosition;
            }
            else
            {
                startPoint = endPosition;
                endPoint = this.transform;
                _specialBackpack = specialBackpack;
            }
            this.speed = speed;
            objectToAnimate = objToAnimate;
            distance = Vector3.Distance(startPoint.position, endPoint.position);  // Calculate the distance between start and end points
            distance = distance == 0 ? 1 : distance;
                                                                                  //startTime = Time.time;       // Record the start time
            isAnimating = true;          // Set the animation flag to true

            StartCoroutine(AnimateParabola(onComplete));
        }
        public void ParabolicAnimation(Transform objToAnimate, Transform startPosition, Transform endPosition, Action onComplete = null, SpecialBackpack specialBackpack = null,float speed = 15f)
        {
            startPoint = startPosition;
            endPoint = endPosition;
            _specialBackpack = specialBackpack;
            objectToAnimate = objToAnimate;
            distance = Vector3.Distance(startPoint.position, endPoint.position);
            distance = distance == 0 ? 1 : distance;
            isAnimating = true;
            this.speed = speed;
            StartCoroutine(AnimateParabola(onComplete));
        }
        public void ParabolicAnimation(Transform objToAnimate, Transform startPosition, Transform endPosition, AnimationType animationType = AnimationType.None, Action onComplete = null, SpecialBackpack specialBackpack = null,float speed = 15f)
        {
            _itemType = animationType;
            startPoint = startPosition;
            endPoint = endPosition;
            _specialBackpack = specialBackpack;
            objectToAnimate = objToAnimate;
            distance = Vector3.Distance(startPoint.position, endPoint.position);
            distance = distance == 0 ? 1 : distance;
            isAnimating = true;
            this.speed = speed;
            StartCoroutine(AnimateParabola(onComplete));
        }
        public void ParabolicAnimation(Transform objToAnimate, Transform startPosition, Transform endPosition, Action onComplete = null,float speed = 15f, AnimationType animationType = AnimationType.None)
        {
            _itemType = animationType;
            startPoint = startPosition;
            endPoint = endPosition;
            objectToAnimate = objToAnimate;
            distance = Vector3.Distance(startPoint.position, endPoint.position);
            distance = distance == 0 ? 1 : distance;
            isAnimating = true;
            this.speed = speed;
            StartCoroutine(AnimateParabola(onComplete));
        }
        private IEnumerator AnimateParabola(Action onComplete = null)
        {
            startTime = Time.time;       // Record the start time

            while (isAnimating)
            {
                // Calculate the elapsed time since the animation started
                float elapsedTime = Time.time - startTime;

                // Calculate the normalized distance covered by the animation
                float normalizedDistance = elapsedTime * speed / distance;

                // Calculate the y position using a parabolic equation
                float yOffset = height * 4f * normalizedDistance * (1f - normalizedDistance);

                // Interpolate the position between start and end points based on the normalized distance
                Vector3 newPosition = Vector3.Lerp(startPoint.position, endPoint.position, normalizedDistance);
                newPosition += Vector3.up * yOffset;   // Apply the y offset

                // Move the object to the new position
                objectToAnimate.position = newPosition;

                // Check if the animation has reached the end point
                if (normalizedDistance >= 1f)
                {
                    Destroy(objectToAnimate.gameObject);
                    isAnimating = false;    // Set the animation flag to false
                    if (_itemType == AnimationType.DropItem && _specialBackpack != null)
                    {
                        _specialBackpack.RemoveItems(1);
                    }

                    yield return new WaitForEndOfFrame();
                    onComplete?.Invoke();
                }

                yield return null;
            }
        }
        public IEnumerator AnimateParabola(Transform objToAnimate, Transform startPosition, Transform endPosition, float speed = 15f, float height = 1.5f)
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
                objToAnimate.position = newPosition;

                // Check if the animation has reached the end point
                if (normalizedDistance >= 1f)
                {
                    Destroy(objToAnimate.gameObject);
                    isAnimating = false;    // Set the animation flag to false

                }

                yield return null;
            }
        }
    }
}

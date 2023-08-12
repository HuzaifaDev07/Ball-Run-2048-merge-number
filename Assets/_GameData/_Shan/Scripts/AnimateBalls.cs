using System.Collections;
using System;
using UnityEngine;
using NaughtyAttributes;

namespace ArcadeIdle.Shan
{
    public class AnimateBalls : MonoBehaviour
    {
        private Transform startPoint;     // Starting point of the animation
        private Transform endPoint;       // Ending point of the animation
        private Transform objectToAnimate;       // Object to animate
        [BoxGroup("Animation")] [SerializeField] private float speed = 15f;         // Speed of the animation
        [BoxGroup("Animation")] [SerializeField] private float height = 2.75f;        // Height of the parabola

        private float distance;          // Distance between the start and end points
        private float startTime;         // Start time of the animation
        private bool isAnimating;        // Flag to indicate if the animation is in progress
        

        public void ParabolicAnimation(Transform objToAnimate, Transform startPosition, Transform endPosition, Action action = null)
        {
            startPoint = startPosition;
            endPoint = endPosition;
            objectToAnimate = objToAnimate;
            distance = Vector3.Distance(startPoint.position, endPoint.position);
            distance = distance == 0 ? 1 : distance;
            isAnimating = true;
            StartCoroutine(AnimateParabola(action));
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

                    yield return new WaitForEndOfFrame();
                    onComplete?.Invoke();
                }

                yield return null;
            }
        }
    }
}
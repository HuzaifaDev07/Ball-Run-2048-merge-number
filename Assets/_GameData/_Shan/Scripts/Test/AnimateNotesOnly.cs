using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using DG.Tweening;

namespace ArcadeIdle.Shan
{
    public class AnimateNotesOnly : MonoBehaviour
    {
        [SerializeField] GameObject _notePrefabToAnimate;
        [SerializeField] Transform _startPoint;
        [SerializeField] Transform _endPoint;

        private Queue<GameObject> objectToAnimate = new Queue<GameObject>();     // Object to animate

        private AnimateBalls _Animation;

        [SerializeField] private float speed = 3f;         // Speed of the animation
        [SerializeField] private float height = 1.5f;        // Height of the parabola

        private float distance;          // Distance between the start and end points
        private float startTime;         // Start time of the animation
        private bool isAnimating;        // Flag to indicate if the animation is in progress

        public bool instantiate = false;
        private void OnValidate()
        {
            if (instantiate)
                AnimateNotes(5);
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
                yield return new WaitForSeconds(0.1f);
                var note = Instantiate(_notePrefabToAnimate, this.transform);
                note.transform.position = _startPoint.position;
                note.transform.rotation = _startPoint.rotation;

                note.transform.DOMove(_endPoint.position, 0.5f).OnComplete(()=>{
                    DestroyObject(note);
                });
                notesCount++;
            }
            //SpawnSquareFormation(numberOfNotes);
        }
        void DestroyObject(GameObject gameObject)
        {
            Destroy(gameObject);
        }
        public void ParabolicAnimation()
        {
            GameObject go = objectToAnimate.Dequeue();
            distance = Vector3.Distance(_startPoint.position, _endPoint.position);
            distance = distance == 0 ? 1 : distance;
            isAnimating = true;
            StartCoroutine(AnimateParabola(go.transform));
        }
        private IEnumerator AnimateParabola(Transform objectToAnimate)
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
                Vector3 newPosition = Vector3.Lerp(_startPoint.position, _endPoint.position, normalizedDistance);
                newPosition += Vector3.up * yOffset;   // Apply the y offset

                // Move the object to the new position
                objectToAnimate.position = newPosition;

                // Check if the animation has reached the end point
                if (normalizedDistance >= 1f)
                {
                    Destroy(objectToAnimate.gameObject);
                    isAnimating = false;    // Set the animation flag to false

                    yield return new WaitForEndOfFrame();
                }

                yield return null;
            }
        }
    }
}

using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hz.PlayerMove
{
    public class PlayerMovement : MonoBehaviour
    {
        public static PlayerMovement instance;
        public Touch initTouch = new Touch();
        public Vector3 originPos;
        public float positionX, positionY;
        public float speed = 0.5f, computerSpeed, dir = -1f;
        public float rotationSpeed;
        public float mapWidth = 2f;
        public float Radius = 2f;
        public float objectMoveSpeed = 2f;
        public bool touching = false;
        public GameObject MyParent;
        public Rigidbody rb;
        private MergeData mergeData;
        Ray hit;
        [HideInInspector] public bool Booster = false;
        [HideInInspector] public bool Magnet = false;
        [HideInInspector] public bool SuperPower = false;
        public FollowPath FollowPath;
        #region Dotweens 
        [Header("******** tweens Assets ********")]
        public DOTweenAnimation MyScaleTween;
        public Jun_TweenRuntime WingsTween;
        #endregion

        #region ------- Particles -----------
        public GameObject BoosterParticle;
        #endregion
        private void Awake()
        {
            instance = this;

        }
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            positionX = 0f;
            positionY = transform.localPosition.y;
            originPos = transform.localPosition;
        }

        void Update()
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)        //if finger touches the screen
                {
                    if (touching == false)
                    {
                        touching = true;
                        positionY = transform.localPosition.y;
                        initTouch = touch;
                    }

                }
                else if (touch.phase == TouchPhase.Moved)       //if finger moves while touching the screen
                {
                    float deltaX = initTouch.position.x - touch.position.x;
                    positionX -= deltaX * Time.deltaTime * speed * dir;
                    positionX = Mathf.Clamp(positionX, -mapWidth, mapWidth);      //to set the boundaries of the player's position
                    transform.localPosition = new Vector3(positionX, positionY, 0f);
                    initTouch = touch;
                }
                else if (touch.phase == TouchPhase.Ended)       //if finger releases the screen
                {
                    initTouch = new Touch();
                    touching = false;
                }
            }
            //if you play on computer---------------------------------
            float x = Input.GetAxis("Horizontal") * Time.deltaTime * computerSpeed;     //you can move by pressing 'a' - 'd' or the arrow keys
            Vector3 newPosition = rb.transform.localPosition + Vector3.right * x;
            newPosition.x = Mathf.Clamp(newPosition.x, -mapWidth, mapWidth);
            transform.localPosition = newPosition;

            if (Magnet)
            {
                //--------------------------------------------------------
                RaycastHit[] hits = Physics.SphereCastAll(transform.position, Radius, Vector3.forward, 0f);

                // Loop through all the hits to find objects with the target tag
                foreach (RaycastHit hit in hits)
                {
                    MergeData MD = hit.collider.GetComponent<MergeData>();
                    if (MD != null && hit.collider.CompareTag("Ball"))
                    {
                        // Check if the object's BallIndex matches the player's BallIndex
                        if (MD.BallIndex == GetComponent<MergeData>().BallIndex)
                        {
                            Vector3 targetPos = transform.position + Vector3.up; // You can adjust the offset if needed
                            float duration = Vector3.Distance(MD.transform.position, targetPos) / objectMoveSpeed; // Calculate duration based on distance and speed

                            // Move the objects towards the player using DOTween
                            MD.gameObject.transform.DOMove(targetPos, duration);
                            break;
                        }
                    }
                }
            }
        }

        #region ---------------- PowerUpWorking ---------------------

        public void SuperPowerState(bool check)
        {
            if (check)
            {
                rb.isKinematic = true;
                WingsTween.Play();
            }
            else
            {
                rb.isKinematic = false;
                MyParent.transform.position = new Vector3(MyParent.transform.position.x, 0, MyParent.transform.position.z);
                WingsTween.StopPlay();
            }
        }

        public void BoosterState(bool check)
        {
            if (check)
            {
                FollowPath.movementSpeed = 25;
                BoosterParticle.SetActive(true);
            }
            else
            {
                BoosterParticle.SetActive(false);
                FollowPath.movementSpeed = 7;
            }
        }

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("PowerUp"))
            {
                other.GetComponent<PowerUp>().PowerCheck();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, Radius);
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace ArcadeIdle
{
    public class MainDoor : MonoBehaviour
    {
        [SerializeField] DoorObject _doorObject;
        [SerializeField] EnteringFrom _enteringFrom;

        [SerializeField] GameObject _doorLeft;
        [SerializeField] GameObject _doorRight;
        // Start is called before the first frame update
        void Start()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            var trigger = GetComponent<Trigger>();
            //Subscribes trigger events by script because it more safety
            //Events in inspector can accidentally resets and you'll to set it again
            trigger.OnEnterTrigger.AddListener(OnEnterTrigger);
            //trigger.OnExitTrigger.AddListener(OnExitTrigger);
        }

        void OnEnterTrigger(GameObject other)
        {
            /*note.transform.DOMove(_endPoint.position, animationSpeed).SetAs(tParms).OnComplete(() => {
                    Destroy(note);
                });*/
            if(_doorObject.doorState == DoorState.Close && _enteringFrom == EnteringFrom.Outside)
            {
                TweenParams tParms = new TweenParams().SetEase(Ease.Linear);
                _doorLeft.transform.DOLocalRotate(new Vector3(0, -90, 0), 0.25f).SetAs(tParms);
                _doorRight.transform.DOLocalRotate(new Vector3(0, 90, 0), 0.25f).SetAs(tParms);
                _doorObject.doorState = DoorState.Open;
            }
            else if (_doorObject.doorState == DoorState.Close && _enteringFrom == EnteringFrom.Inside)
            {
                TweenParams tParms = new TweenParams().SetEase(Ease.Linear);
                _doorLeft.transform.DOLocalRotate(new Vector3(0, 90, 0), 0.25f).SetAs(tParms);
                _doorRight.transform.DOLocalRotate(new Vector3(0, -90, 0), 0.25f).SetAs(tParms);
                _doorObject.doorState = DoorState.Open;
            }
            else if (_doorObject.doorState == DoorState.Open && _enteringFrom == EnteringFrom.ToClose)
            {
                TweenParams tParms = new TweenParams().SetEase(Ease.OutBounce);
                _doorLeft.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.25f).SetAs(tParms);
                _doorRight.transform.DOLocalRotate(new Vector3(0, 0, 0), 0.25f).SetAs(tParms);
                _doorObject.doorState = DoorState.Close;
            }
        }
    }
}

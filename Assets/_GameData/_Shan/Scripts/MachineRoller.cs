using UnityEngine;
using NaughtyAttributes;
using DG.Tweening;

namespace ArcadeIdle.Shan
{
    public class MachineRoller : MonoBehaviour
    {
        [BoxGroup("Number Movement")] [SerializeField] Transform[] _endPoint;
        [BoxGroup("Box Machine")] [SerializeField] MachineBox machineBox;

        int count = 0;

        public void MoveToMachine(GameObject ball)
        {
            TweenParams tParms = new TweenParams().SetEase(Ease.Linear);
            ball.transform.DOMove(_endPoint[count].position, 1).SetAs(tParms).OnComplete(() =>
            { 
                Destroy(ball);
                if (count > 1)
                {
                    count = 0;
                    machineBox.SpawnNextNumber();
                }
            });
            count++;
        }

    }
}

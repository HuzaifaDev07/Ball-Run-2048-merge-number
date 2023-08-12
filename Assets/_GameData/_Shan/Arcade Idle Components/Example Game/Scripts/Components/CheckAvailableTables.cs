using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

namespace ArcadeIdle
{
    public class CheckAvailableTables : MonoBehaviour
    {
        [BoxGroup("TABLES")]
        [SerializeField] private GameObject[] _availableTabels;


        public bool AssignAvailableTabels(GameObject customer)
        {
            NavMeshAgent customerNavAgent = customer.GetComponent<NavMeshAgent>();
            Animator animator = customer.GetComponent<Animator>();
            return false;
        }

        private void StackWalk(Animator animator)
        {
            animator.SetBool("StackWalk", true);
            animator.SetBool("Idle", false);
            animator.SetBool("Simple", false);
        }
    }
}

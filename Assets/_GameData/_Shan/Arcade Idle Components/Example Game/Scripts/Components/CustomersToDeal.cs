using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArcadeIdle
{
    public class CustomersToDeal : MonoBehaviour
    {
        public readonly List<GameObject> customers = new List<GameObject>();

        private Coroutine restartCustomers;
        public void MoveAllCustomerToCounter()
        {
            StartCoroutine(MoveCustomersToCounter());
        }
        IEnumerator MoveCustomersToCounter()
        {
            yield return null;

            if (restartCustomers != null)
            {
                StopCoroutine(restartCustomers);
            }
            for (int i = 0; i < customers.Count; i++)
            {
                var _Customer = customers[i].GetComponent<Customer>();
                _Customer.MoveToPrevDestinantion();
                yield return new WaitForSecondsRealtime(0.85f);
            }
            restartCustomers = StartCoroutine(RestartCustomerItself());
        }
        IEnumerator RestartCustomerItself()
        {
            yield return new WaitForSeconds(50f);
            MoveAllCustomerToCounter();
            /*var _Customer = customers[0].GetComponent<Customer>();
            _Customer.MoveToPrevDestinantion();*/
        }
    }
}

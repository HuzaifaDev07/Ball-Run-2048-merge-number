using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnjectedBallControl : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(ResetMe());
    }

    IEnumerator ResetMe()
    {
        yield return new WaitForSeconds(2f);

        // Reset the object's position to its startpos
        Destroy(this.gameObject);
    }
}

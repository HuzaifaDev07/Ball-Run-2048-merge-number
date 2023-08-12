using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinSpin : MonoBehaviour
{
    [SerializeField] Text ClaimText;
    public int multiplyAmount;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("2x"))
        {
            multiplyAmount = 2;
        }
        else if (collision.CompareTag("4x"))
        {
            multiplyAmount = 4;
        }
        else if (collision.CompareTag("8x"))
        {
            multiplyAmount = 5;
        }
        Debug.Log(collision.gameObject.name);
        ClaimText.text = collision.gameObject.name;
    }
}

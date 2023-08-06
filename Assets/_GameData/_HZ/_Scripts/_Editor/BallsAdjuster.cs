using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallsAdjuster : MonoBehaviour
{
    public MergeData[] _balls;
    public Material[] _BallsMat;

    //private void OnValidate()
    //{
    //    SetBallMat();
    //}
    public void SetBallMat()
    {
        for (int i = 0; i < _balls.Length; i++)
        {
            for (int j = 0; j < _BallsMat.Length; j++)
            {
                if (_balls[i].BallIndex == j)
                {
                    _balls[i].GetComponent<MeshRenderer>().material = _BallsMat[j];
                    if (j == 0)
                    {
                        _balls[i].GetComponentInChildren<TextMeshPro>().text = 2.ToString();
                    }
                    if (j == 1)
                    {
                        _balls[i].GetComponentInChildren<TextMeshPro>().text = 4.ToString();
                    }
                     if (j == 2)
                    {
                        _balls[i].GetComponentInChildren<TextMeshPro>().text = 8.ToString();
                    }
                     if (j == 3)
                    {
                        _balls[i].GetComponentInChildren<TextMeshPro>().text = 16.ToString();
                    }
                     if (j == 4)
                    {
                        _balls[i].GetComponentInChildren<TextMeshPro>().text = 32.ToString();
                    }
                     if (j == 5)
                    {
                        _balls[i].GetComponentInChildren<TextMeshPro>().text = 64.ToString();
                    }
                     if (j == 6)
                    {
                        _balls[i].GetComponentInChildren<TextMeshPro>().text = 128.ToString();
                    }
                     if (j == 7)
                    {
                        _balls[i].GetComponentInChildren<TextMeshPro>().text = 256.ToString();
                    }
                     if (j == 8)
                    {
                        _balls[i].GetComponentInChildren<TextMeshPro>().text = 512.ToString();
                    }
                     if (j == 9)
                    {
                        _balls[i].GetComponentInChildren<TextMeshPro>().text = 1024.ToString();
                    }
                     if (j == 10)
                    {
                        _balls[i].GetComponentInChildren<TextMeshPro>().text = 2048.ToString();
                    }

                }
            }
        }
    }
    //else if (_balls[i].BallIndex == 1)
    //{
    //    _balls[i].GetComponent<MeshRenderer>().material = _BallsMat[0];
    //}
    //else if (_balls[i].BallIndex == 2)
    //{
    //    _balls[i].GetComponent<MeshRenderer>().material = _BallsMat[0];
    //}
    //else if (_balls[i].BallIndex == 3)
    //{
    //    _balls[i].GetComponent<MeshRenderer>().material = _BallsMat[0];
    //} 
    //else if (_balls[i].BallIndex == 4)
    //{
    //    _balls[i].GetComponent<MeshRenderer>().material = _BallsMat[0];
    //} 
    //else if (_balls[i].BallIndex == 5)
    //{
    //    _balls[i].GetComponent<MeshRenderer>().material = _BallsMat[0];
    //}
    //else if (_balls[i].BallIndex == 6)
    //{
    //    _balls[i].GetComponent<MeshRenderer>().material = _BallsMat[0];
    //} 
    //else if (_balls[i].BallIndex == 7)
    //{
    //    _balls[i].GetComponent<MeshRenderer>().material = _BallsMat[0];
    //}
    //else if (_balls[i].BallIndex == 8)
    //{
    //    _balls[i].GetComponent<MeshRenderer>().material = _BallsMat[0];
    //}
    //else if (_balls[i].BallIndex == 9)
    //{
    //     _balls[i].GetComponent<MeshRenderer>().material = _BallsMat[0]; 
    //}


}

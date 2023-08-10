using Hz.PlayerMove;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLineController : MonoBehaviour
{
    public GameObject[] FinishPoints;
    public MergeData PlayerMergeData;
    public GameObject PlayerCamera;
    public GameObject CutSceneCamera;
    public PlayerMovement PlayerMovement;
    public FollowPath Followpath;

    public void PlayerMergeDataForFinish()
    {
        PlayerCamera.SetActive(false);
        CutSceneCamera.SetActive(true);
        PlayerMovement.enabled = false;
        Followpath.enabled = false;
        for (int i = 0; i <= PlayerMergeData.BallIndex; i++)
        {
            FinishPoints[i].SetActive(true);
        }
    }

}

using Hz.PlayerMove;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class FinishLineController : MonoBehaviour
{
    public GameObject[] FinishPoints;
    public MergeData PlayerMergeData;
    public GameObject PlayerCamera;
    public GameObject CutSceneCamera;
    public PlayerMovement PlayerMovement;
    public FollowPath Followpath;
    public PlayableDirector CutSceneDirector;
    public int FinishPointCounter;
    public GameObject[] ReachedBall;
    public Cinemachine.CinemachineVirtualCamera CutSceneVirtualCamera;
    public AudioSource GlassAudioSource;
    public AudioClip GlassBreak;
    public AudioClip WinSound;
    public AudioClip ReachFinishEndPlatform;
    public AudioClip ReachFinishEndPoint;
    public AudioClip StairSound;
    public GameObject ConfettiParticles;

    public void PlayerMergeDataForFinish()
    {
    
        CutSceneDirector.gameObject.SetActive(true);
        PlayerCamera.SetActive(false);
        CutSceneCamera.SetActive(true);
        PlayerMovement.enabled = false;
        Followpath.enabled = false;
        for (int i = 0; i <= PlayerMergeData.BallIndex; i++)
        {
            FinishPoints[i].SetActive(true);
        }
     
        CutSceneDirector.Play();
    }

}

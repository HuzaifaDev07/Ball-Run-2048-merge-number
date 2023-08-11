using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassBroker : MonoBehaviour
{
    public Rigidbody[] MyGlassMeshes;
    public FinishLineController finishLineController;

    public void SpawnBalls()
    {
        this.GetComponent<Collider>().enabled = false;
        if (finishLineController.FinishPointCounter <= finishLineController.PlayerMergeData.BallIndex)
        {

            if (finishLineController.FinishPointCounter == finishLineController.PlayerMergeData.BallIndex)
            {
                finishLineController.GlassAudioSource.PlayOneShot(finishLineController.ReachFinishEndPlatform);
                finishLineController.CutSceneDirector.Stop();
                Hz.Gameplay.GameManager.instance.LevelCompleted();
                finishLineController.GlassAudioSource.PlayOneShot(finishLineController.WinSound);
                finishLineController.ConfettiParticles.SetActive(true);
            }
            finishLineController.ReachedBall[finishLineController.FinishPointCounter].SetActive(true);
            finishLineController.FinishPointCounter++;

            Debug.Log($"{finishLineController.FinishPointCounter}: Counter");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            SpawnBalls();
        }
    }



}

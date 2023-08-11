using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassBroker : MonoBehaviour
{
    public Rigidbody[] MyGlassMeshes;
    public FinishLineController finishLineController;

    public void BreakGlass()
    {
        this.GetComponent<Collider>().enabled = false;
        if (finishLineController.FinishPointCounter <= finishLineController.PlayerMergeData.BallIndex)
        {
            if (finishLineController.FinishPointCounter == finishLineController.PlayerMergeData.BallIndex)
            {
                finishLineController.GlassAudioSource.PlayOneShot(finishLineController.ReachFinishEndPlatform);
                finishLineController.CutSceneDirector.Stop();
                finishLineController.CutSceneCamera.GetComponent<Cinemachine.CinemachineVirtualCamera>().Follow = null;
                finishLineController.CutSceneCamera.GetComponent<Cinemachine.CinemachineVirtualCamera>().LookAt = null;
                Hz.Gameplay.GameManager.instance.LevelCompleted();
                finishLineController.GlassAudioSource.PlayOneShot(finishLineController.WinSound);
                finishLineController.ConfettiParticles.SetActive(true);
            }
            else
            {
                finishLineController.GlassAudioSource.PlayOneShot(finishLineController.GlassBreak);

                for (int i = 0; i < MyGlassMeshes.Length; i++)
                {
                    MyGlassMeshes[i].isKinematic = false;
                    MyGlassMeshes[i].AddForce(Vector3.down * Random.Range(350, 500));
                }
                StartCoroutine(RemoveGlass());
            }
            finishLineController.FinishPointCounter++;
            Debug.Log($"{finishLineController.FinishPointCounter}: Counter");
        }
    }


    IEnumerator RemoveGlass()
    {
        yield return new WaitForSeconds(.5f);
        for (int i = 0; i < MyGlassMeshes.Length; i++)
        {
            MyGlassMeshes[i].gameObject.SetActive(false);
        }
        this.gameObject.transform.parent.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {

            BreakGlass();
        }
    }



}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hz.PrefData;

//using Dreamteck.Splines;

namespace Hz.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        public Color[] TotalColors;
        public GameObject[] Levels;
        public PathCreation.PathCreator[] LevelsPath;
        public FollowPath PlayerFollowPath;
        public GameObject LevelCompletePanel;
        public GameObject LevelFailedPanel;
        public FinishLineController FinishLine;
        public LevelsBaseBallWorking LevelsBaseBallWorking;
        public Transform BallMainParentForNotFollow;
        public Transform BallObjectForFollow;
        public Cinemachine.CinemachineVirtualCamera VirtualCamera;
        #region ======= PlayerData ======

        #endregion
        private void Awake()
        {
            Time.timeScale = 1;
            instance = this;
        }
        private void Start()
        {
            //PrefData.PrefData.SetLevel(false, 14);
            Levels[PrefData.PrefData.GetLevels()].SetActive(true);
            PlayerFollowPath.path = LevelsPath[PrefData.PrefData.GetLevels()];
            PlayerMove.PlayerMovement.instance.mapWidth = LevelsBaseBallWorking.levelDatas[PrefData.PrefData.GetLevels()].mapWidth;
            if (LevelsBaseBallWorking.levelDatas[PrefData.PrefData.GetLevels()].FollowBall)
            {
                VirtualCamera.m_Follow = BallObjectForFollow;
                VirtualCamera.m_LookAt = BallObjectForFollow;
            }
            else
            {
                VirtualCamera.m_Follow = BallMainParentForNotFollow;
                VirtualCamera.m_LookAt = BallMainParentForNotFollow;
            }

        }

        public void StageClear()
        {
            FinishLine.PlayerMergeDataForFinish();


        }
        public void LevelCompleted()
        {
            StartCoroutine(DelayCallLevelComplet());
        }

        public void ReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


        public void StageFailed()
        {
            LevelFailedPanel.SetActive(true);
            Time.timeScale = 0;
        }


        IEnumerator DelayCallLevelComplet()
        {
            yield return new WaitForSeconds(2f);
            LevelCompletePanel.SetActive(true);
            PrefData.PrefData.SetLevel(true, 1);
            Time.timeScale = 0;
        }
        public void SwitchCamera(Transform transform)
        {
            FinishLine.CutSceneVirtualCamera.m_Follow = transform;
            FinishLine.CutSceneVirtualCamera.m_LookAt = transform;
        }
    }
}


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
        [System.Serializable]
        public class LevelsBalls
        {
            public MergeData[] LevelBalls;
        }
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
        public LevelsBalls[] _LevelBalls;
        int levelIndex;
        #region ======= PlayerData ======

        #endregion

        #region ------------ UI -----------------
        public GameObject MainScreen;

        #endregion


        private void Awake()
        {

            instance = this;
            //PrefData.PrefData.SetLevel(false, 14);
            if (PrefData.PrefData.GetLevels() < Levels.Length)
            {
                levelIndex = PrefData.PrefData.GetLevels();
            }
            else
            {
                levelIndex = Random.Range(0, Levels.Length);
            }

            if (levelIndex > Levels.Length)
            {
             
                PlayerFollowPath.path = LevelsPath[levelIndex];
                Levels[levelIndex].SetActive(true);
            }
            else
            {
                PlayerFollowPath.path = LevelsPath[levelIndex];
                Levels[levelIndex].SetActive(true);
            }
            if (LevelsBaseBallWorking.levelDatas[levelIndex].FollowBall)
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

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0.2f);
            PlayerMove.PlayerMovement.instance.mapWidth = LevelsBaseBallWorking.levelDatas[levelIndex].mapWidth;
            Time.timeScale = 0;
            PlayerFollowPath.enabled = false;
        }

        public void StartGamePlay()
        {
            MainScreen.SetActive(false);
            PlayerFollowPath.enabled = true;
            Time.timeScale = 1;
        }

        public void UpgradeBall()
        {
            for (int i = 0; i < _LevelBalls[levelIndex].LevelBalls.Length; i++)
            {
                _LevelBalls[levelIndex].LevelBalls[i].UpgrageBall();
            }
            PlayerFollowPath.enabled = true;
            Time.timeScale = 1;
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
            PlayerFollowPath.enabled = false;
            // Time.timeScale = 0;
        }


        IEnumerator DelayCallLevelComplet()
        {
            yield return new WaitForSeconds(2f);
            LevelCompletePanel.SetActive(true);
            PrefData.PrefData.SetLevel(true, 1);
            //   Time.timeScale = 0;
        }
        public void SwitchCamera(Transform transform)
        {
            FinishLine.CutSceneVirtualCamera.m_Follow = transform;
            FinishLine.CutSceneVirtualCamera.m_LookAt = transform;
        }
    }
}


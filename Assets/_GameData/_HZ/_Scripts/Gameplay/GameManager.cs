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
        #region ======= PlayerData ======

        #endregion
        private void Awake()
        {
            Time.timeScale = 1;
            instance = this;
        }
        private void Start()
        {
            PrefData.PrefData.SetLevel(false, 10);
            Levels[PrefData.PrefData.GetLevels()].SetActive(true);
            PlayerFollowPath.path = LevelsPath[PrefData.PrefData.GetLevels()];
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


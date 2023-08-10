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
            Levels[PrefData.PrefData.GetLevels()].SetActive(true);
        }

        public void StageClear()
        {
            LevelCompletePanel.SetActive(true);
            PrefData.PrefData.SetLevel(true, 1);
            FinishLine.PlayerMergeDataForFinish();
            //Time.timeScale = 0;
        }

        public void ReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


        public void StageFailed()
        {
            LevelFailedPanel.SetActive(true);
            Time.timeScale = 0;
        }


    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Hz.PrefData
{
    public static class PrefData
    {
        public static string LevelString = "Levels";

        public static int GetLevels()
        {
            PlayerPrefs.GetInt(LevelString);

            return PlayerPrefs.GetInt(LevelString);
        }

        public static int SetLevel(bool increase, int IncreaseAmount)
        {
            if (increase)
            {
                PlayerPrefs.SetInt(LevelString, GetLevels() + IncreaseAmount);
            }
            else
            {
                PlayerPrefs.SetInt(LevelString,IncreaseAmount);
            }
            return PrefData.GetLevels();
        }
    }
}

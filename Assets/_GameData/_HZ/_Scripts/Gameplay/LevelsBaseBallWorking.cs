using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelBaseBallControl ")]
public class LevelsBaseBallWorking : ScriptableObject
{
    [System.Serializable]
    public class LevelData
    {
        public int mapWidth;
        public bool FollowBall;
    }
    public LevelData[] levelDatas;
   
}

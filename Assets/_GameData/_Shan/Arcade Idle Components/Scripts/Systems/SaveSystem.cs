using UnityEngine;

namespace ArcadeIdle
{
    //Save system is a simple singleton, which created to create, load and save player data
    public class SaveSystem : Initializable
    {
        public static SaveSystem Instance;
        public PlayerData Data;

        public override void Initialize()
        {
            Instance = this;
            LoadData();
        }

        /// <summary>
        /// Loads data from player prefs or creates new and saves.
        /// </summary>
        public void LoadData()
        {
            if (PlayerPrefs.HasKey(Constants.DATA_KEY))
            {
                var json = PlayerPrefs.GetString(Constants.DATA_KEY);
                Data = JsonUtility.FromJson<PlayerData>(json);
            }
            else
            {
                Data = new PlayerData();
                SaveData();
            }
        }

        /// <summary>
        /// Saves data to player prefs.
        /// </summary>
        public void SaveData()
        {
            var json = JsonUtility.ToJson(Data);
            PlayerPrefs.SetString(Constants.DATA_KEY, json);
        }
    }
}
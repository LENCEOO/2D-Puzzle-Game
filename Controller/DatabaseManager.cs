using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace IG.Controller
{
    /// <summary>
    /// 레벨 진행 상태와 최고 점수를 JSON 파일로 저장 / 불러오기하는 기능을 담당.
    /// </summary>
    public class DatabaseManager : MonoBehaviour
    {
        private const string JsonFileName = "Level Data";
        private string _path;

        [Serializable]
        public class LevelData
        {
            public string name;
            public int level;
            public int topScore;
        }

        [Serializable]
        public class StoredData // 모든 레벨 정보 포함
        {
            [HideInInspector] public int lastPlayedLevel = 1; // 마지막으로 실행된 레벨
            public List<LevelData> levelDataList = new(); // 사용자가 클리어한 레벨들의 리스트
        }

        [SerializeField] private StoredData storedData;
        private LevelManager _levelManager;

        public int LastUnlockedLevel
        {
            get
            {
                // last unlocked level is the level which is not finished 
                // so it's data will not be in the database
                return storedData.levelDataList.Count + 1;
            }
        }

        //Get the last played level from stored data
        public int LastPlayedLevel
        {
            get
            {
                return storedData.lastPlayedLevel;
            }
            set
            {
                storedData.lastPlayedLevel = value;
                WriteData();
            }
        }

        private void OnEnable()
        {
            LevelManager.OnLevelLoaded += UpdateLastPlayedLevel;
        }

        private void OnDisable()
        {
            LevelManager.OnLevelLoaded -= UpdateLastPlayedLevel;
        }

        /// <summary>
        /// Initialize all data and returns last played level
        /// </summary>
        /// <param name="levelManager"></param>
        /// <returns></returns>
        public int Initialize(LevelManager levelManager)
        {
            _levelManager = levelManager;

            _path = Path.Combine(Application.persistentDataPath, JsonFileName);

            // If required json file for database exists at the persistentDataPath then load the data
            // otherwise create the file
            if (File.Exists(_path)) LoadData();
            else WriteData();

            return storedData.lastPlayedLevel;
        }

        // 기존 데이터에 레벨이 있으면 점수 갱신 ( 더 높을 때만 )
        public void SaveLevelData(int level, int score)
        {
            Debug.Log($"Saving data: Level {level}, Score {score}");

            var levelData = GetLevelData(level);

            if (levelData != null)
            {
                if (levelData.topScore < score)
                    levelData.topScore = score;
            }
            else
            {
                storedData.levelDataList.Add(new LevelData()
                {
                    name = "Level " + level,
                    level = level,
                    topScore = score
                });
            }

            WriteData(); // JSON에 반영
        }

        // Writes data to json file. If file does not exists, it creates
        private void WriteData()
        {
            Debug.Log("Writing Data..");
            var dataJson = JsonUtility.ToJson(storedData);
            File.WriteAllText(_path, dataJson);
        }

        // Loads the data from json file
        private void LoadData()
        {
            Debug.Log("Loading Data..");
            var jsonData = File.ReadAllText(_path);
            storedData = JsonUtility.FromJson<StoredData>(jsonData);
        }

        public LevelData GetLevelData(int level) // 특정 level에 대한 데이터 가져오기
        {
            if (storedData.levelDataList == null) return null;
            return storedData.levelDataList.Find(ld => ld.level == level);
        }

        private void UpdateLastPlayedLevel(int level, int _) // update last level
        {
            //Update last played level
            LastPlayedLevel = level;
        }
    }
}

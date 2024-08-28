using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class DataLoader
    {
        private const string LeaderboardFilePath = "Leaderboard";

        public static async Task<List<PlayerData>> LoadLeaderboardDataAsync()
        {
            await Task.Yield();

            TextAsset jsonData = Resources.Load<TextAsset>(LeaderboardFilePath);
            if (jsonData == null)
            {
                Debug.LogError("Could not find Leaderboard.json file in Resources folder");
                return new List<PlayerData>();
            }

            Leaderboard playerDataList = JsonUtility.FromJson<Leaderboard>(jsonData.text);
            return playerDataList.leaderboard;
        }
    }

    [System.Serializable]
    public class Leaderboard
    {
        public List<PlayerData> leaderboard;
    }
}

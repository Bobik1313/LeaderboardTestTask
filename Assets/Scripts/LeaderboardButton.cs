using SimplePopupManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts
{
    public class LeaderboardButton : MonoBehaviour
    {
        private Button _button;

        [Inject]
        private IPopupManagerService _popupManager;

        private void Start()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OpenLeaderboard);
        }

        private async void OpenLeaderboard()
        {
            List<PlayerData> players = await DataLoader.LoadLeaderboardDataAsync();
            _popupManager.OpenPopup("Assets/Prefabs/LeaderboardUI.prefab", players);
        }
    }
}

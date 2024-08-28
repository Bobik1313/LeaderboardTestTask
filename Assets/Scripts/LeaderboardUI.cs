using SimplePopupManager;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Assets.Scripts
{
    public class LeaderboardUI : MonoBehaviour, IPopupInitialization
    {
        [SerializeField]
        private Transform contentTransform;

        [SerializeField]
        private Button closeButton;

        [SerializeField]
        private LeaderboardElementUI elementPrefab;

        [Inject]
        private IPopupManagerService _popupManager;

        private List<PlayerData> players;
        private List<LeaderboardElementUI> elements = new List<LeaderboardElementUI>();

        private async void Start()
        {
            Canvas canvas = FindObjectOfType<Canvas>();

            if (canvas != null)
            {
                var rect = gameObject.GetComponent<RectTransform>();
                rect.SetParent(canvas.transform);
                rect.offsetMin = Vector2.zero;
                rect.offsetMax = Vector2.zero;
            }
            closeButton.onClick.AddListener(ClosePopup);
            await LoadAndFillLeaderboard();
        }

        public async Task Init(object param)
        {
            if (param is List<PlayerData> playerDataList)
            {
                players = playerDataList;
                await FillLeaderboard();
            }

            SceneContext sceneContext = FindObjectOfType<SceneContext>();
            sceneContext.Container.Inject(this);
        }

        private async Task LoadAndFillLeaderboard()
        {
            if (players == null)
                return;

            players = await DataLoader.LoadLeaderboardDataAsync();
            await FillLeaderboard();
        }

        private async Task FillLeaderboard()
        {
            gameObject.SetActive(true);
            ClearLeaderboard();

            foreach (var player in players) 
            {
                var element = Instantiate(elementPrefab, contentTransform);

                element.PlayerName.text = player.name;
                element.Score.text = player.score.ToString();
                StartCoroutine(WebRequestService.LoadAvatar(player.avatar, (sprite) =>
                {
                    if (sprite != null)
                    {
                        element.AvatarIcon.sprite = sprite;
                    }
                    else
                    {
                        Debug.LogWarning("Failed to load avatar for player: " + player.name);
                    }

                    element.LoadingText.gameObject.SetActive(false);
                    element.AvatarIcon.gameObject.SetActive(true);

                }));
                SetPlayerTypeVisual(element, player.type);

            }
        }

        private void ClearLeaderboard()
        {
            foreach(var elementUI in elements)
            {
                Destroy(elementUI);
            }
        }

        private void SetPlayerTypeVisual(LeaderboardElementUI item, string type)
        {
            switch (type)
            {
                case "Diamond":
                    item.Background.color = Color.cyan;
                    item.PlayerName.fontStyle = FontStyle.Bold;
                    item.Score.fontStyle = FontStyle.Bold;
                    break;
                case "Gold":
                    item.Background.color = Color.yellow;
                    item.PlayerName.fontStyle = FontStyle.Bold;
                    item.Score.fontStyle = FontStyle.Bold;
                    break;
                case "Silver":
                    item.Background.color = Color.gray;
                    item.PlayerName.fontStyle = FontStyle.Bold;
                    item.Score.fontStyle = FontStyle.Bold;
                    break;
                case "Bronze":
                    item.Background.color = Color.red;
                    item.PlayerName.fontStyle = FontStyle.Bold;
                    item.Score.fontStyle = FontStyle.Bold;
                    break;
                default:
                    item.Background.color = Color.white;

                    break;
            }
        }

        public void ClosePopup()
        {
            _popupManager.ClosePopup("Assets/Prefabs/LeaderboardUI.prefab"); 
        }
    }
}
 
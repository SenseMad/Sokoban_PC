using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using YG;

namespace Leaderboard
{
    public sealed class UILeaderboardMenu : MonoBehaviour
    {
        private const string LeaderboardBestTime = "BestTotalTime";
        private const string LeaderboardMostFood = "most_food_collected";

        [Header("Refs")]
        [SerializeField] private LeaderboardYG _leaderboard;
        [SerializeField] private TextMeshProUGUI _titleText;
        [SerializeField] private Button _buttonBestTime;
        [SerializeField] private Button _buttonMostFood;
        [SerializeField] private Button _buttonClose;
        [SerializeField] private GameObject _rootPanel;

        //--------------------------------------

        /*private void Awake()
        {
            _buttonBestTime.onClick.AddListener(ShowBestTime);
            _buttonMostFood.onClick.AddListener(ShowMostFood);

            if (_buttonClose != null)
                _buttonClose.onClick.AddListener(Close);
        }

        private void OnDestroy()
        {
            _buttonBestTime.onClick.RemoveListener(ShowBestTime);
            _buttonMostFood.onClick.RemoveListener(ShowMostFood);

            if (_buttonClose != null)
                _buttonClose.onClick.RemoveListener(Close);
        }*/

        //--------------------------------------

        public void Open()
        {
            if (_rootPanel != null)
                _rootPanel.SetActive(true);

            ShowBestTime();
        }

        public void Close()
        {
            if (_rootPanel != null)
                _rootPanel.SetActive(false);
        }

        public void ShowBestTime()
        {
            _leaderboard.nameLB = LeaderboardBestTime;
            _leaderboard.timeTypeConvert = true;

            if (_titleText != null)
                _titleText.text = "Best Total Time";

            _leaderboard.UpdateLB();
        }

        public void ShowMostFood()
        {
            _leaderboard.nameLB = LeaderboardMostFood;
            _leaderboard.timeTypeConvert = false;

            if (_titleText != null)
                _titleText.text = "Most Food Collected";

            _leaderboard.UpdateLB();
        }
    }
}
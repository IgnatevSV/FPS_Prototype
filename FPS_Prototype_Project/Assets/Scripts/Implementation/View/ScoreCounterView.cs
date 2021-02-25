using System;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace FPSProject.Impl.Views
{
    public class ScoreCounterView : MonoBehaviour
    {
        [SerializeField] private string _scoreInfoFormat = "{0} PTS";
        [SerializeField] private TextMeshProUGUI _scoreInfo;
        [SerializeField] private TextMeshProUGUI _bestScoreInfo;
        
        [Inject] private IScoreLogic _scoreLogic;

        private IDisposable _currentScoreSubscription;
        private IDisposable _bestScoreSubscription;
        
        private void Awake()
        {
            _currentScoreSubscription = _scoreLogic.CurrentScore.Subscribe(OnCurrentScoreChanged);
            _bestScoreSubscription = _scoreLogic.BestScore.Subscribe(OnBestScoreChanged);
        }

        private void OnCurrentScoreChanged(int currentScore)
        {
            ScoreValueChanged(_scoreInfo, currentScore);
        }
        
        private void OnBestScoreChanged(int bestScore)
        {
            ScoreValueChanged(_bestScoreInfo, bestScore);
        }

        private void ScoreValueChanged(TextMeshProUGUI valueInfo, int value)
        {
            valueInfo.text = string.Format(_scoreInfoFormat, value);
        }

        private void OnDestroy()
        {
            _currentScoreSubscription?.Dispose();
            _bestScoreSubscription?.Dispose();
        }
    }
}
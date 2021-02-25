using System;
using FPSProject.Impl.Configs;
using FPSProject.Impl.Saves;
using UniRx;
using UnityEngine;
using Zenject;

namespace FPSProject.Impl.Logic
{
    public class ScoreLogic : IScoreLogic, IDisposable
    {
        private readonly IDestroyableObjectsLogic _destroyableObjectsLogic;
        private readonly ISaves _savesLogic;
        private readonly WeaponsConfig _weaponsConfig;
        private readonly ScoreCounterConfig _scoreCounterConfig;

        private readonly ReactiveProperty<int> _currentScore = new ReactiveProperty<int>();
        private readonly ReactiveProperty<int> _bestScore = new ReactiveProperty<int>();

        private readonly ScoreSavesPart _scoreSavesPart;
        
        public IReadOnlyReactiveProperty<int> CurrentScore => _currentScore;
        public IReadOnlyReactiveProperty<int> BestScore => _bestScore;

        [Inject]
        public ScoreLogic(
            IDestroyableObjectsLogic destroyableObjectsLogic,
            ISaves savesLogic,
            WeaponsConfig weaponsConfig,
            ScoreCounterConfig scoreCounterConfig)
        {
            _destroyableObjectsLogic = destroyableObjectsLogic;
            _savesLogic = savesLogic;
            _weaponsConfig = weaponsConfig;
            _scoreCounterConfig = scoreCounterConfig;
            
            _scoreSavesPart = _savesLogic.GetSavesData<ScoreSavesPart>();
            _bestScore.Value = _scoreSavesPart.BestScore;
            
            Application.quitting += OnAppQuit;
            _destroyableObjectsLogic.OnDestroyableObjectKilled += OnDestroyableObjectKilled;
        }

        private void OnAppQuit()
        {
            Application.quitting -= OnAppQuit;
            _savesLogic.Save();
        }
        
        public void Dispose()
        {
            Application.quitting -= OnAppQuit;
            _destroyableObjectsLogic.OnDestroyableObjectKilled -= OnDestroyableObjectKilled;
        }

        private void OnDestroyableObjectKilled(DestroyMeta meta)
        {
            UpdateScore(meta);
        }

        private void UpdateScore(DestroyMeta meta)
        {
            int bulletId = _weaponsConfig.GetBulletId(meta.DestroyBullet.BulletConfigData);
            int score = _scoreCounterConfig.GetScoreDataByBulletId(bulletId).Score;

            _currentScore.Value += score;
            if (_currentScore.Value > _bestScore.Value)
            {
                _bestScore.Value = _currentScore.Value;
                _scoreSavesPart.BestScore = _bestScore.Value;
            }
        }
    }
}
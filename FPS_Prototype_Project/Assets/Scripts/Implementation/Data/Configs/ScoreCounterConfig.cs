using FPSProject.Impl.Configs.Extensions;
using UnityEngine;

namespace FPSProject.Impl.Configs
{
    [CreateAssetMenu(fileName = "ScoreCounterConfig", menuName = "Configs/ScoreCounterConfig")]
    public class ScoreCounterConfig : ScriptableObject
    {
        // On inspector click near field or arrow (but not on it) to expand
        [SerializeField] private ScoreDataDictionary _scoreRules;

        public IScoreData GetScoreDataByBulletId(int id) => _scoreRules.GetConfigDataById(id);
    }
}
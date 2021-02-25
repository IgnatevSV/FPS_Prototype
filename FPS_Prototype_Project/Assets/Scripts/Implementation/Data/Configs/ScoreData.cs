using System;
using UnityEngine;

namespace FPSProject.Impl.Configs
{
    
    [Serializable]
    public struct ScoreData : IScoreData
    {
        [SerializeField] private int _score;
        
        public int Score => _score;

        public ScoreData(int score)
        {
            _score = score;
        }
    }
}
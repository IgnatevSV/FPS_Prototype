using System;
using UnityEngine;

namespace FPSProject.Impl.Saves
{
    [Serializable]
    public class ScoreSavesPart : ISavesPart
    {
        [SerializeField] private int _bestScore;

        public int BestScore
        {
            get => _bestScore;
            set => _bestScore = value;
        }
    }
}
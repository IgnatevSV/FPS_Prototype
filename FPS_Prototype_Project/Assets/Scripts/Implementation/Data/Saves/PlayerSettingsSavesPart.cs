using System;
using UnityEngine;

namespace FPSProject.Impl.Saves
{
    [Serializable]
    public class PlayerSettingsSavesPart : ISavesPart
    {
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _rotationSpeed;

        public float MovementSpeed
        {
            get => _movementSpeed;
            set => _movementSpeed = value;
        }

        public float RotationSpeed
        {
            get => _rotationSpeed;
            set => _rotationSpeed = value;
        }
    }
}
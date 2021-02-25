using UnityEngine;

namespace FPSProject
{
    public struct BulletRuntimeMeta
    {
        public object Source { get; }
        public bool IsPlayerBullet { get; }
        public Vector3 InitialPosition { get; }
        public Vector3 WeaponLocalVelocity { get; }
        public Transform WeaponCameraTransform { get; }
        
        public BulletRuntimeMeta(
            object source,
            bool isPlayerBullet,
            Vector3 initialPosition,
            Vector3 weaponLocalVelocity,
            Transform weaponCameraTransform = null)
        {
            Source = source;
            
            IsPlayerBullet = isPlayerBullet;
            InitialPosition = initialPosition;
            WeaponLocalVelocity = weaponLocalVelocity;
            WeaponCameraTransform = weaponCameraTransform;
        }
    }
}
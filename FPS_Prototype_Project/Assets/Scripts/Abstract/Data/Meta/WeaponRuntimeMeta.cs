using UnityEngine;

namespace FPSProject
{
    public struct WeaponRuntimeMeta
    {
        public bool IsPlayerWeapon { get; }
        public Transform WeaponCameraTransform { get; }

        public WeaponRuntimeMeta(bool isPlayerWeapon, Transform weaponCameraTransform = null)
        {
            IsPlayerWeapon = isPlayerWeapon;
            WeaponCameraTransform = weaponCameraTransform;
        }
    }
}
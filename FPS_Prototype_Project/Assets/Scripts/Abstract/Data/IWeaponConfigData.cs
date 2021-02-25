using UnityEngine;

namespace FPSProject
{
    public interface IWeaponConfigData : IConfigData
    {
        IWeaponObject Prefab { get; }
        int BulletConfigDataId { get; }
        int MaxBullets { get; }
        float WeaponAccuracy { get; }
        GameObject CrosshairPrefab { get; }
        float ReloadTime { get; }
        bool AutoReload { get; }
        bool CanBreakReload { get; }
    }
}
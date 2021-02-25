using UniRx;
using UnityEngine;

namespace FPSProject
{
    public interface IWeaponObject
    {
        IReadOnlyReactiveProperty<int> CurrentAmmo { get; }
        IReadOnlyReactiveProperty<bool> IsReloading { get; }
        IWeaponConfigData WeaponConfigData { get; }
        IView WeaponView { get; }
        Transform Transform { get; }

        void TriggerShot();
        void Reload();

        void Init(IWeaponConfigData configData, IBulletConfigData bulletConfigData, WeaponRuntimeMeta weaponRuntimeMeta);
    }
}
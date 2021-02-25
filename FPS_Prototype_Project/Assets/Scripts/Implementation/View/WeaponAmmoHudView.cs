using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace FPSProject.Impl.Views
{
    public class WeaponAmmoHudView : CurrentToMaxCounter
    {
        [Inject] private IMainGameLogic _mainGameLogic;

        [SerializeField] private GameObject _reloadMarker;

        private IDisposable _weaponReloadStateSubscription;

        private void Awake()
        {
            UpdateReloadMarkerState(false);
            _mainGameLogic.ActiveWeapon.Subscribe(OnActiveWeaponChanged).AddTo(this);
        }

        private void OnDestroy()
        {
            _weaponReloadStateSubscription?.Dispose();
        }

        private void OnActiveWeaponChanged(IWeaponObject weaponObject)
        {
            if (weaponObject == null) return;
            Init(weaponObject.CurrentAmmo, weaponObject.WeaponConfigData.MaxBullets);

            _weaponReloadStateSubscription?.Dispose();
            _weaponReloadStateSubscription = weaponObject.IsReloading.Subscribe(UpdateReloadMarkerState);
        }

        private void UpdateReloadMarkerState(bool isReloading)
        {
            if (_reloadMarker != null) _reloadMarker.SetActive(isReloading);
        }
    }
}
using System.Collections;
using UnityEngine;

namespace FPSProject.Impl.Components
{
    public class RifleWeaponObject : BaseWeaponObject
    {
        protected override IEnumerator ReloadProcess()
        {
            _isReloading.Value = true;

            float delayPerBullet = WeaponConfigData.ReloadTime / WeaponConfigData.MaxBullets;

            while (_currentAmmo.Value < WeaponConfigData.MaxBullets)
            {
                yield return new WaitForSeconds(delayPerBullet);
                _currentAmmo.Value++;
            }

            _currentAmmo.Value = WeaponConfigData.MaxBullets;
            BreakReload();
        }
    }
}
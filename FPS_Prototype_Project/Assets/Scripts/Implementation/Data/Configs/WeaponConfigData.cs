using FPSProject.Impl.Components;
using UnityEngine;

namespace FPSProject.Impl.Configs
{
    [CreateAssetMenu(fileName = "WeaponConfigData", menuName = "Configs/Weapons/WeaponConfigData")]
    public class WeaponConfigData : ScriptableObject, IWeaponConfigData
    {
        [SerializeField] private BaseWeaponObject _prefab;
        [SerializeField] private int _bulletConfigDataId;
        [SerializeField] private int _maxBullets;
        [SerializeField] private float _weaponAccuracy;
        [SerializeField] private GameObject _crosshairPrefab;
        [SerializeField] private float _reloadTime;
        [SerializeField] private bool _autoReload;
        [SerializeField] private bool _canBreakReload;

        public IWeaponObject Prefab => _prefab;
        public int BulletConfigDataId => _bulletConfigDataId;
        public int MaxBullets => _maxBullets;
        public float WeaponAccuracy => _weaponAccuracy;
        public GameObject CrosshairPrefab => _crosshairPrefab;
        public float ReloadTime => _reloadTime;
        public bool AutoReload => _autoReload;
        public bool CanBreakReload => _canBreakReload;
    }
}
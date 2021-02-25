using System.Collections.Generic;
using System.Linq;
using FPSProject.Impl.Configs.Extensions;
using UnityEngine;

namespace FPSProject.Impl.Configs
{
    [CreateAssetMenu(fileName = "WeaponsConfig", menuName = "Configs/Weapons/WeaponsConfig")]
    public class WeaponsConfig : ScriptableObject
    {
        private const int DEFAULT_ID = -1;
        
        [SerializeField] private WeaponsDictionary _weaponConfigs;
        [SerializeField] private BulletDictionary _bulletConfigs;

        public IReadOnlyCollection<int> WeaponIds => _weaponConfigs.Keys;

        public IWeaponConfigData GetWeaponDataById(int id) => _weaponConfigs.GetConfigDataById(id);
        
        public IBulletConfigData GetBulletDataById(int id) => _bulletConfigs.GetConfigDataById(id);

        public int GetBulletId(IBulletConfigData bulletConfigData)
        {
            if (bulletConfigData is BulletConfigData bulletConfigDataImpl)
            {
                if (_weaponConfigs != null && _weaponConfigs.Any() && _bulletConfigs.ContainsValue(bulletConfigDataImpl))
                {
                    return _bulletConfigs.FirstOrDefault(kv => kv.Value == bulletConfigDataImpl).Key;
                }
            }

            Debug.LogWarning("BulletConfigData id not found!");
            
            return DEFAULT_ID;
        }
    }
}
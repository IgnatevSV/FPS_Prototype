using System;
using System.Collections.Generic;
using System.Linq;
using FPSProject.Impl.Components;
using FPSProject.Impl.Configs;
using UniRx;
using UnityEngine;
using Zenject;

namespace FPSProject.Impl.Controllers
{
    public class PlayerWeaponController : MonoBehaviour
    {
        [Inject] private IInputLogic _playerInputHandler;

        [SerializeField] private Transform _weaponCamera;

        [Inject] private IObjectsSpawnerLogic _poolLogic;
        [Inject] private IMainGameLogic _mainGameLogic;
        [Inject] private WeaponsConfig _weaponsConfig;
        
        [SerializeField] private Transform _weaponPosition;

        private readonly Dictionary<int, IWeaponObject> _currentWeapons = new Dictionary<int, IWeaponObject>();
        
        private int _currentWeaponId;
        
        private IWeaponObject ActiveWeapon => _mainGameLogic.ActiveWeapon?.Value;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            CreateWeapons();
            
            if (_currentWeapons.Any())
            {
                ArmWeapon(0);
            }
        }

        private void ArmWeapon(int weaponId)
        {
            if (!_currentWeapons.ContainsKey(weaponId)) return;
            
            IDisposable weaponHidedSubscription = null;
            
            void OnActiveWeaponHided()
            {
                _currentWeaponId = weaponId;
                _mainGameLogic.SetActiveWeapon(_currentWeapons[weaponId]);
                ActiveWeapon.WeaponView.Show();
                weaponHidedSubscription?.Dispose();
            }

            if (ActiveWeapon == null)
            {
                OnActiveWeaponHided();
            }
            else
            {
                weaponHidedSubscription = ActiveWeapon.WeaponView
                    .State
                    .Where(state => state == ViewState.Invisible)
                    .Subscribe(_ => OnActiveWeaponHided());
                
                ActiveWeapon.WeaponView.Hide();
            }
        }

        private void CreateWeapons()
        {
            foreach (int weaponId in _weaponsConfig.WeaponIds)
            {
                IWeaponConfigData weaponConfigData = _weaponsConfig.GetWeaponDataById(weaponId);
                CreateWeapon(weaponId, weaponConfigData);
            }
        }

        private void CreateWeapon(int id, IWeaponConfigData weaponConfigData)
        {
            IBulletConfigData bulletConfigData = _weaponsConfig.GetBulletDataById(weaponConfigData.BulletConfigDataId);
            WeaponRuntimeMeta weaponRuntimeMeta = new WeaponRuntimeMeta(true, _weaponCamera);

            if (weaponConfigData.Prefab is BaseWeaponObject baseWeaponObjectPrefab)
            {
                IWeaponObject weaponObject = _poolLogic.CreateNewMonoBehaviourInstance(baseWeaponObjectPrefab);
                weaponObject.Transform.parent = _weaponPosition;
                weaponObject.Transform.localPosition = Vector3.zero;
                weaponObject.Init(weaponConfigData, bulletConfigData, weaponRuntimeMeta);
                weaponObject.WeaponView.Init(ViewState.Invisible);
                _currentWeapons.Add(id, weaponObject);
            }
        }
        
        private void Update()
        {
            if (_playerInputHandler.GetFireInputDown())
            {
                ActiveWeapon.TriggerShot();
            }

            if (_playerInputHandler.GetReloadInputDown())
            {
                ActiveWeapon.Reload();
            }

            int selectedWeapon = _playerInputHandler.GetSelectWeaponInput();
            
            if (selectedWeapon >= 0)
            {
                OnWeaponSelectedHandler(selectedWeapon);
            }
            
            int switchWeaponIndex = _playerInputHandler.GetSwitchWeaponInput();
            
            if (switchWeaponIndex != 0)
            {
                OnWeaponSwitched(switchWeaponIndex);
            }
        }

        private void OnWeaponSelectedHandler(int weaponIndex)
        {
            ArmWeapon(weaponIndex);
        }

        private void OnWeaponSwitched(int index)
        {
            int weaponIndex = _currentWeaponId + index;
            if (weaponIndex >= 0)
            {
                OnWeaponSelectedHandler(weaponIndex);
            }
        }
    }
}
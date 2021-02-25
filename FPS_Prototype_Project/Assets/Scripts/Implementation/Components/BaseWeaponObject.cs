using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

namespace FPSProject.Impl.Components
{
    [RequireComponent(typeof(IView))]
    public class BaseWeaponObject : MonoBehaviour, IWeaponObject
    {
        public event Action OnShot;
        public event Action OnEmptyShotTrigger;
                
        protected readonly ReactiveProperty<int> _currentAmmo = new ReactiveProperty<int>();
        protected readonly ReactiveProperty<bool> _isReloading = new ReactiveProperty<bool>();
        
        [Inject] private IObjectsSpawnerLogic _poolManager;
        
        [SerializeField] private Transform _bulletSpawnPoint;
        
        private IWeaponConfigData _weaponConfigData;
        private IBulletConfigData _bulletConfigData;
        private WeaponRuntimeMeta _weaponRuntimeMeta;
        
        private Vector3 _weaponVelocity;
        private Vector3 _weaponLastKnownPosition;

        private IView _weaponView;
        
        private IDisposable _weaponViewSubscription;
        private IDisposable _weaponLocalVelocityCalculation;
        
        private Coroutine _currentReloadRoutine;
        private bool _isInited;

        public IReadOnlyReactiveProperty<int> CurrentAmmo => _currentAmmo;
        public IReadOnlyReactiveProperty<bool> IsReloading => _isReloading;

        public IWeaponConfigData WeaponConfigData => _weaponConfigData;
        public IView WeaponView => _weaponView ?? (_weaponView = GetComponent<IView>());
        public Transform Transform => transform;

        public void Init(IWeaponConfigData configData, IBulletConfigData bulletConfigData, WeaponRuntimeMeta weaponRuntimeMeta)
        {
            _weaponConfigData = configData;
            _bulletConfigData = bulletConfigData;
            _weaponRuntimeMeta = weaponRuntimeMeta;

            _currentAmmo.Value = _weaponConfigData.MaxBullets;
            
            _weaponViewSubscription?.Dispose();
            _weaponViewSubscription = WeaponView.State.Subscribe(OnWeaponViewStateChangedHandler);
            
            _isInited = true;
        }

        public void TriggerShot()
        {
            if (CanShoot())
            {
                ProcessShot();
            }
            else
            {
                if (_weaponConfigData.CanBreakReload && _currentAmmo.Value > 0)
                {
                    BreakReload();
                    ProcessShot();
                }
                else if (!_weaponConfigData.AutoReload)
                {
                    OnEmptyAmmoShotTrigger();
                }
            }
        }

        public virtual void Reload()
        {
            if (CanReload())
            {
                _currentReloadRoutine = StartCoroutine(ReloadProcess());
            }
        }
        
        protected virtual IEnumerator ReloadProcess()
        {
            _isReloading.Value = true;
            yield return new WaitForSeconds(_weaponConfigData.ReloadTime);
            _currentAmmo.Value = _weaponConfigData.MaxBullets;
            BreakReload();
        }

        protected void BreakReload()
        {
            if (_currentReloadRoutine != null)
            {
                StopCoroutine(_currentReloadRoutine);
            }
            
            _isReloading.Value = false;
            _currentReloadRoutine = null;
        }
        
        private void OnEnable()
        {
            if (_isInited)
            {
                CheckForAutoReload();
            }
        }

        private void OnDisable()
        {
            BreakReload();
        }

        private void OnDestroy()
        {
            _weaponViewSubscription?.Dispose();
            DisposeWeaponLocalVelocityCalucaltion();
        }

        private void OnWeaponViewStateChangedHandler(ViewState state)
        {
            if (state == ViewState.Hiding || state == ViewState.Invisible)
            {
                DisposeWeaponLocalVelocityCalucaltion();
            }
            else
            {
                InitWeaponLocalVelocityCalucaltion();
            }
        }

        private void CalculateWeaponLocalVelocity()
        {
            if (!(Time.deltaTime > 0)) return;
            Vector3 bulletSpawnPointPosition = _bulletSpawnPoint.position;
            _weaponVelocity = (bulletSpawnPointPosition - _weaponLastKnownPosition) / Time.deltaTime;
            _weaponLastKnownPosition = bulletSpawnPointPosition;
        }

        private void InitWeaponLocalVelocityCalucaltion()
        {
            _weaponLocalVelocityCalculation = Observable.EveryUpdate().Subscribe(_ => CalculateWeaponLocalVelocity());
        }

        private void DisposeWeaponLocalVelocityCalucaltion()
        {
            _weaponLocalVelocityCalculation?.Dispose();
        }

        private bool CanReload()
        {
            return !IsReloading.Value && _currentAmmo.Value < _weaponConfigData.MaxBullets;
        }

        private void OnEmptyAmmoShotTrigger()
        {
            OnEmptyShotTrigger?.Invoke();
        }

        private void ProcessShot()
        {
            _currentAmmo.Value--;
            
            CheckForAutoReload();

            SpawnBullet();
            ShowFireEffect();
            OnShot?.Invoke();
        }

        private void CheckForAutoReload()
        {
            if (_currentAmmo.Value == 0 && _weaponConfigData.AutoReload)
            {
                Reload();
            }
        }
        
        private bool CanShoot()
        {
            return _currentAmmo.Value > 0 && !IsReloading.Value;
        }
        
        private void SpawnBullet()
        {
            if (_bulletConfigData.Prefab is BaseBulletObject baseBulletObjectPrefab)
            {
                IBulletObject bullet = (IBulletObject) _poolManager.SpawnPoolObject(baseBulletObjectPrefab);
                bullet.Transform.SetPositionAndRotation(_bulletSpawnPoint.position, GetWeaponRotation());
                bullet.Init(_bulletConfigData, GetBulletRuntimeMeta());
            }
        }

        private BulletRuntimeMeta GetBulletRuntimeMeta()
        {
            return new BulletRuntimeMeta(this.gameObject,
                _weaponRuntimeMeta.IsPlayerWeapon,
                transform.position,
                _weaponVelocity,
                _weaponRuntimeMeta.WeaponCameraTransform);
        }

        private Quaternion GetWeaponRotation()
        {
            return Quaternion.LookRotation(GetShotDirectionWithinSpread());
        }

        private void ShowFireEffect()
        {
            
        }

        private Vector3 GetShotDirectionWithinSpread()
        {
            return Vector3.Slerp(
                _bulletSpawnPoint.forward,
                UnityEngine.Random.insideUnitSphere,
                Mathf.Abs(_weaponConfigData.WeaponAccuracy - 1f));
        }
    }
}
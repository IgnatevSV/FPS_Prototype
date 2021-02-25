using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

namespace FPSProject.Impl.Components
{
    public abstract class BaseBulletObject : MonoBehaviour, IBulletObject
    {
        private const float BULLET_LINE_FIX_ERROR = 0.1f;
        
        public event Action OnBulletHit;

        [Inject] private IObjectsSpawnerLogic _poolManager;
        [Inject] private IDestroyableObjectsLogic _destroyableObjectsLogic;
        
        [SerializeField] private GameObject _bulletEffectPrefab;
        
        private List<Collider> _ignoredColliders;
        private IDisposable _bulletFlyProcessSubscription;
        private BulletRuntimeMeta _bulletRuntimeMeta;

        private Vector3 _lastFramePosition;
        private Vector3 _currentVelocity;
        private Vector3 _targetBulletLineFix;
        private Vector3 _currentBulletLineFix;

        private IBulletConfigData _bulletConfigData;

        private float _distanceLastFrame;
        private float _distanceTotal;
        
        private bool _isBulletAimLineFixed;

        public Transform Transform => transform;
        public IPoolableObject Prefab => _bulletConfigData.Prefab;

        public bool IsInUse { get; set; }
        public IBulletConfigData BulletConfigData => _bulletConfigData;
        
        public virtual void Init(IBulletConfigData bulletConfigData, BulletRuntimeMeta metaData)
        {
            _bulletConfigData = bulletConfigData;
            _bulletRuntimeMeta = metaData;
            
            InitIgnoredColliders();
            InitPosition();

            if (metaData.IsPlayerBullet)
            {
                OnPlayerShoot();
            }
            
            StopBulletFlyProcess();

            _bulletFlyProcessSubscription = Observable.EveryUpdate().Subscribe(_ => BulletFlyProcess());
        }
        
        public virtual void ResetPoolableObject()
        {
            StopBulletFlyProcess();
        }
        
        protected abstract void DamageHittedObjects(RaycastHit hit);

        private void StopBulletFlyProcess()
        {
            _bulletFlyProcessSubscription?.Dispose();
        }
        
        private void OnDestroy()
        {
            StopBulletFlyProcess();
        }
        
        private void BulletFlyProcess()
        {
            UpdateFlyDistance();
            MoveBullet();
            FixPositionToCamera();
            AffectGravity();
            DetectHit();
            CheckFlyDistanceDestroy();
            _lastFramePosition = transform.position;
        }

        private void CheckFlyDistanceDestroy()
        {
            if (_distanceTotal > _bulletConfigData.MaxRange)
            {
                Destroy(this.gameObject);
            }
        }

        private void UpdateFlyDistance()
        {
            _distanceLastFrame = Vector3.Distance(transform.position, _lastFramePosition);
            _distanceTotal += _distanceLastFrame;
        }

        private void InitPosition()
        {
            _lastFramePosition = transform.position;
            _currentVelocity = transform.forward * _bulletConfigData.Speed;
            transform.position += _bulletRuntimeMeta.WeaponLocalVelocity * Time.deltaTime;
        }

        private void InitIgnoredColliders()
        {
            Collider[] ownerColliders = (_bulletRuntimeMeta.Source as GameObject).GetComponentsInChildren<Collider>();
            _ignoredColliders = new List<Collider>();
            _ignoredColliders.AddRange(ownerColliders);
        }

        private void OnPlayerShoot()
        {
            Vector3 cameraToMuzzle = _bulletRuntimeMeta.InitialPosition - _bulletRuntimeMeta.WeaponCameraTransform.position;

            _targetBulletLineFix = Vector3.ProjectOnPlane(-cameraToMuzzle,_bulletRuntimeMeta.WeaponCameraTransform.forward);

            if (IsBulletHitted(_bulletRuntimeMeta.WeaponCameraTransform, cameraToMuzzle, out RaycastHit hit) && CanBeHitted(hit))
            {
                ProcessHit(hit);
            }
        }

        private bool IsBulletHitted(Transform weaponCameraTransform, Vector3 cameraToMuzzle, out RaycastHit bulletHit)
        {
            bool isHitted = Physics.Raycast(
                weaponCameraTransform.position,
                cameraToMuzzle.normalized,
                out RaycastHit hit,
                cameraToMuzzle.magnitude,
                _bulletConfigData.HittableLayers,
                QueryTriggerInteraction.Collide);

            bulletHit = hit;

            return isHitted;
        }

        private void MoveBullet()
        {
            transform.position += (_currentVelocity + _bulletRuntimeMeta.WeaponLocalVelocity) * Time.deltaTime;
            transform.forward = _currentVelocity.normalized;
        }

        private void AffectGravity()
        {
            if (_bulletConfigData.GravityDownForce > 0)
            {
                _currentVelocity += Time.deltaTime * _bulletConfigData.GravityDownForce * Vector3.down;
            }
        }

        private Vector3 GetBulletLineFixAtFrame()
        {
            Vector3 bulletLineFixDiff = _targetBulletLineFix - _currentBulletLineFix;
            Vector3 bulletLineFixAtFrame = (_distanceLastFrame / _bulletConfigData.BulletAimLineFixRange) * _targetBulletLineFix;
            bulletLineFixAtFrame = Vector3.ClampMagnitude(bulletLineFixAtFrame, bulletLineFixDiff.magnitude);

            return bulletLineFixAtFrame;
        }
        
        private void FixPositionToCamera()
        {
            if (_isBulletAimLineFixed) return;

            float bulletLineFixDiff = _currentBulletLineFix.sqrMagnitude - _targetBulletLineFix.sqrMagnitude;
            
            if (bulletLineFixDiff <= 0f)
            {
                Vector3 bulletLineFixAtFrame = GetBulletLineFixAtFrame();
                _currentBulletLineFix += bulletLineFixAtFrame;

                if (Math.Abs(bulletLineFixDiff) < BULLET_LINE_FIX_ERROR)
                {
                    _isBulletAimLineFixed = true;
                }

                transform.position += bulletLineFixAtFrame;
            }
        }

        private RaycastHit[] GetBulletHits()
        {
            Vector3 displacementSinceLastFrame = transform.position - _lastFramePosition;
            
            return Physics.SphereCastAll(_lastFramePosition,
                _bulletConfigData.CollisionDetectError,
                displacementSinceLastFrame.normalized, 
                displacementSinceLastFrame.magnitude,
                _bulletConfigData.HittableLayers,
                QueryTriggerInteraction.Collide);
        }

        private RaycastHit GetClosestHit()
        {
            RaycastHit closestHit = new RaycastHit {distance = float.MaxValue};
            bool hitValid = false;
            
            RaycastHit[] hits = GetBulletHits();
            
            foreach (RaycastHit hit in hits)
            {
                if (hit.distance > closestHit.distance || !CanBeHitted(hit)) continue;
                closestHit = hit;
                hitValid = true;
            }
            
            if (hitValid)
            {
                if (closestHit.distance <= 0f)
                {
                    closestHit.point = transform.position;
                    closestHit.normal = -transform.forward;
                    closestHit.distance = 0f;
                }
            }
            else
            {
                closestHit = new RaycastHit {distance = float.MinValue};
            }

            return closestHit;
        }
        
        private void DetectHit()
        {
            RaycastHit hit = GetClosestHit();
            
            if (hit.distance >= 0)
            {
                ProcessHit(hit);
            }
        }

        private bool CanBeHitted(RaycastHit hit)
        {
            if (hit.collider.GetComponent<IDestroyIgnorableObject>() != null) return false;
            if (hit.collider.isTrigger && hit.collider.GetComponent<IDestroyableObject>() == null) return false;
            if (_ignoredColliders != null && _ignoredColliders.Contains(hit.collider)) return false;

            return true;
        }

        private void PlayEffects(RaycastHit hit)
        {
            if (_bulletEffectPrefab)
            {
                Instantiate(_bulletEffectPrefab, hit.point + hit.normal, Quaternion.LookRotation(hit.normal));
            }
        }

        private void ProcessHit(RaycastHit hit)
        {
            DamageHittedObjects(hit);
            PlayEffects(hit);
            
            OnBulletHit?.Invoke();

            _poolManager.DespawnPoolObject(this);
        }
    }
}
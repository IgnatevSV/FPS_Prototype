using System;
using UnityEngine;
using UniRx;
using Zenject;

namespace FPSProject.Impl.Components
{
    public abstract class BaseDestroyableObject : MonoBehaviour, IDestroyableObject
    {
        public event Action<DestroyMeta> OnDestroy;
        
        private readonly ReactiveProperty<float> _currentHealth = new ReactiveProperty<float>();
        
        [Inject] private IDestroyableObjectsLogic _destroyableObjectsLogic;

        [SerializeField] private GameObject _destroyEffect;

        private IDestroyableObjectData _data;

        public IDestroyableObjectData Data => _data;
        public IReadOnlyReactiveProperty<float> CurrentHealth => _currentHealth;
        public Transform Transform => transform;

        private readonly ReactiveProperty<bool> _isInited = new ReactiveProperty<bool>();
        public IReadOnlyReactiveProperty<bool> IsInited => _isInited;

        public virtual void Init(IDestroyableObjectData data)
        {
            _data = data;
            _currentHealth.Value = data.MaxHealth;
            _destroyableObjectsLogic.Register(this);
            _isInited.Value = true;
        }
        
        public virtual void SetDamage(IBulletObject bulletObject)
        {
            _currentHealth.Value -= _destroyableObjectsLogic.CalculateDamage(bulletObject);
            
            if (_currentHealth.Value <= 0)
            {
                OnKill(bulletObject);
            }
        }

        private void OnKill(IBulletObject bulletObject)
        {
            DestroyMeta destroyMeta = new DestroyMeta(this, bulletObject);
            
            PlayDestroyEffect();
            
            OnDestroy?.Invoke(destroyMeta);
            Destroy(this.gameObject);
        }
        
        private void PlayDestroyEffect()
        {
            Instantiate(_destroyEffect, transform.position, Quaternion.identity);
        }
    }
}
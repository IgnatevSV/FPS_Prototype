using System;
using UniRx;
using UnityEngine;

namespace FPSProject.Impl.Views
{
    public abstract class BaseHealthView : MonoBehaviour
    {
        private IDestroyableObject _destroyableObject;

        private IDisposable _destroyableObjectHealthSubscription;

        public virtual void Init(IDestroyableObject destroyableObject)
        {
            _destroyableObject = destroyableObject;
            _destroyableObjectHealthSubscription = _destroyableObject.CurrentHealth.Subscribe(OnDestroyableObjectHealthChangedHandler);
            _destroyableObject.OnDestroy += OnDestroyableDestroyedHandler;
            UpdateHealthView();
        }

        protected abstract void SetHealthValue(float value);

        private void OnDestroy()
        {
            UnsubEvents();
        }

        private void UnsubEvents()
        {
            _destroyableObjectHealthSubscription?.Dispose();
            _destroyableObject.OnDestroy -= OnDestroyableDestroyedHandler;
        }
        
        private void OnDestroyableObjectHealthChangedHandler(float health)
        {
            UpdateHealthView();
        }
        
        private void OnDestroyableDestroyedHandler(DestroyMeta meta)
        {
            UnsubEvents();
        }
        
        private void UpdateHealthView()
        {
            float healthValue = (float) _destroyableObject.CurrentHealth.Value / _destroyableObject.Data.MaxHealth;
            SetHealthValue(healthValue);
        }
    }
}
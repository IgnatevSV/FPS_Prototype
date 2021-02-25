using System;
using FPSProject.Impl.Components;
using UniRx;
using UnityEngine;

namespace FPSProject.Impl.Views
{
    [RequireComponent(typeof(ProgressBar))]
    public class HealthHudView : CurrentToMaxCounter
    {
        private readonly Vector3 _staticHorizontalRotation = new Vector3(0, 180, 0);
        
        [SerializeField] private BaseDestroyableObject _destroyableObject;
        [SerializeField] private Vector3 _positionOffset = new Vector3(0, 1,0);
        [SerializeField] private ProgressBar _progressBar;
        
        private Transform _currentTransform;
        private Transform _targetTransform;

        protected override void OnValueChanged(float currentValue)
        {
            base.OnValueChanged(currentValue);
            UpdateProgressBar(currentValue);
        }

        private void UpdateProgressBar(float currentValue)
        {
            float normalizedValue = currentValue / _destroyableObject.Data.MaxHealth;
            _progressBar.SetTargetProgress(normalizedValue);
        }

        private void Awake()
        {
            if (_destroyableObject == null) _destroyableObject = GetComponentInParent<BaseDestroyableObject>();
            if (_progressBar == null) _progressBar = GetComponent<ProgressBar>();
            
            _currentTransform = transform;
            _targetTransform = _destroyableObject.Transform;

            IDisposable destroyableObjectInited = null;
            destroyableObjectInited = _destroyableObject.IsInited.Subscribe(isInited =>
            {
                if (!isInited) return;
                Init(_destroyableObject.CurrentHealth, _destroyableObject.Data.MaxHealth);
                destroyableObjectInited?.Dispose();
            });
        }

        private void LateUpdate()
        {
            if (Camera.main != null) _currentTransform.LookAt(Camera.main.transform);
            _currentTransform.position = _targetTransform.position + _positionOffset;
            _currentTransform.Rotate(_staticHorizontalRotation);
        }
    }
}
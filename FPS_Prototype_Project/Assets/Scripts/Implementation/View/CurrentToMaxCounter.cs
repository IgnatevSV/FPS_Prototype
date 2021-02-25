using System;
using TMPro;
using UniRx;
using UnityEngine;

namespace FPSProject.Impl.Views
{
    public class CurrentToMaxCounter : MonoBehaviour
    {
        [SerializeField] private string _counterFormat = "{0} / {1}";
        
        [SerializeField] private TextMeshProUGUI _counterInfo;

        private float _maxValue;

        private IDisposable _counterUpdateSubscription;

        protected void Init(IReadOnlyReactiveProperty<float> currentValue, float maxValue)
        {
            InitBase(maxValue);
            _counterUpdateSubscription = currentValue.Subscribe(OnValueChanged);
        }

        protected void Init(IReadOnlyReactiveProperty<int> currentValue, int maxValue)
        {
            InitBase(maxValue);
            _counterUpdateSubscription = currentValue.Subscribe(OnValueChanged);
        }

        private void InitBase(float maxValue)
        {
            UnsubCounterUpdate();
            _maxValue = maxValue;
        }
        
        private void UnsubCounterUpdate()
        {
            _counterUpdateSubscription?.Dispose();
        }
        
        private void OnDestroy()
        {
            UnsubCounterUpdate();
        }
        
        private void OnValueChanged(float currentValue)
        {
            OnValueChanged((int) currentValue);
        }

        private void OnValueChanged(int currentValue)
        {
            _counterInfo.text = string.Format(_counterFormat, currentValue, _maxValue);
        }
    }
}
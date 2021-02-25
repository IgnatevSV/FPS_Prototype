using System;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace FPSProject.Impl.Views
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField] private Image _filler;
        [SerializeField] private float _updateDelay;

        private IDisposable _currentProgressBarUpdateProcess;
        
        private float _currentProgress;
        private float _targetProgress;
        private float _updateStep;

        public void SetTargetProgress(float targetProgress)
        {
            _targetProgress = targetProgress;
            if (_currentProgressBarUpdateProcess == null && !IsTargetProgressReached())
            {
                _updateStep = Time.deltaTime / _updateDelay;
                _currentProgressBarUpdateProcess = Observable.EveryUpdate().Subscribe(_ => ProgressBarUpdateProcess());
            }
        }

        private bool IsTargetProgressReached()
        {
            return Mathf.Approximately(_currentProgress, _targetProgress);
        }
        
        private void ProgressBarUpdateProcess()
        {
            if (IsTargetProgressReached())
            {
                _currentProgressBarUpdateProcess.Dispose();
                _currentProgressBarUpdateProcess = null;
            }
            else
            {
                float delta = _targetProgress - _currentProgress;
                if (Mathf.Abs(delta) > _updateStep)
                {
                    UpdateCurrentProgress(_currentProgress + (delta > 0 ? _updateStep : -_updateStep));
                }
                else
                {
                    UpdateCurrentProgress(_targetProgress);
                }
            }
        }
        
        private void UpdateCurrentProgress(float currentProgress)
        {
            if (_filler != null)
            {
                _filler.fillAmount = Mathf.Clamp01(currentProgress);
            }

            _currentProgress = currentProgress;
        }
    }
}
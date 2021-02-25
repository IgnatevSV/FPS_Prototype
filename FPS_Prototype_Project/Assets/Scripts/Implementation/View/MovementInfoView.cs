using UniRx;
using UnityEngine;
using Zenject;

namespace FPSProject.Impl.Views
{
    public class MovementInfoView : MonoBehaviour
    {
        [Inject] private IMainGameLogic _mainGameLogic;

        [SerializeField] private GameObject _crouchModeIcon;

        private void Awake()
        {
            _mainGameLogic.IsCrouchModeEnabled.Subscribe(OnCrouchModeChanged).AddTo(this);
        }

        private void OnCrouchModeChanged(bool isEnabled)
        {
            if(_crouchModeIcon != null) _crouchModeIcon.SetActive(isEnabled);
        }
    }
}
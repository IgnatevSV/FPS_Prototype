using FPSProject.Impl.Configs;
using FPSProject.Impl.Saves;
using UniRx;
using Zenject;

namespace FPSProject.Impl.Logic
{
    public class MainGameLogic : IMainGameLogic
    {
        private readonly PlayerMovementConfig _playerMovementConfig;
        private readonly ISaves _savesLogic;
        
        
        private readonly ReactiveProperty<bool> _isCrouchModeEnabled = new ReactiveProperty<bool>();
        private readonly ReactiveProperty<IWeaponObject> _activeWeapon = new ReactiveProperty<IWeaponObject>();
        private readonly ReactiveProperty<float> _movementSpeed = new ReactiveProperty<float>();
        private readonly ReactiveProperty<float> _rotationSpeed = new ReactiveProperty<float>();
        
        public IReadOnlyReactiveProperty<bool> IsCrouchModeEnabled => _isCrouchModeEnabled;
        public IReadOnlyReactiveProperty<IWeaponObject> ActiveWeapon => _activeWeapon;
        public IReadOnlyReactiveProperty<float> MovementSpeed => _movementSpeed;
        public IReadOnlyReactiveProperty<float> RotationSpeed => _rotationSpeed;

        private PlayerSettingsSavesPart _playerSettingsSavesPart;

        [Inject]
        public MainGameLogic(PlayerMovementConfig playerMovementConfig, ISaves savesLogic)
        {
            _playerMovementConfig = playerMovementConfig;
            _savesLogic = savesLogic;
            
            _playerSettingsSavesPart = _savesLogic.GetSavesData<PlayerSettingsSavesPart>();
            
            UpdateMovementSpeed();
            UpdateRotationSpeed();
        }

        public void SetCrouchModeActive(bool isActive)
        {
            _isCrouchModeEnabled.Value = isActive;
            UpdateMovementSpeed();
            UpdateRotationSpeed();
        }

        public void SetActiveWeapon(IWeaponObject weapon)
        {
            _activeWeapon.Value = weapon;
        }

        public void SaveMovementSpeed(float movementSpeed, bool forceUpdate = true)
        {
            _playerSettingsSavesPart.MovementSpeed = movementSpeed;
            
            if (forceUpdate)
            {
                UpdateMovementSpeed();
            }
            
            _savesLogic.Save();
        }

        public void SaveRotationSpeed(float rotationSpeed, bool forceUpdate = true)
        {
            _playerSettingsSavesPart.RotationSpeed = rotationSpeed;
            
            if (forceUpdate)
            {
                UpdateRotationSpeed();
            }
            
            _savesLogic.Save();
        }

        private void UpdateMovementSpeed()
        {
            float movementSpeed = _playerSettingsSavesPart.MovementSpeed > 0
                ? _playerSettingsSavesPart.MovementSpeed
                : _playerMovementConfig.DefaultMovementSpeedNormal;

            SaveMovementSpeed(movementSpeed, false);
            
            _movementSpeed.Value = GetMovementSpeedModifier() * movementSpeed;
        }

        private void UpdateRotationSpeed()
        {
            float rotationSpeed = _playerSettingsSavesPart.RotationSpeed > 0
                ? _playerSettingsSavesPart.RotationSpeed
                : _playerMovementConfig.DefaultRotationSpeedNormal;

            SaveRotationSpeed(rotationSpeed, false);
            
            float rotationModifier = _playerMovementConfig.CrouchModeAffectRotationSpeed ? GetMovementSpeedModifier() : 1f;
            
            _rotationSpeed.Value = rotationModifier * rotationSpeed;
        }

        private float GetMovementSpeedModifier()
        {
            return IsCrouchModeEnabled.Value ? _playerMovementConfig.MovementSpeedCrouchModeModifier : 1f;
        }
    }
}
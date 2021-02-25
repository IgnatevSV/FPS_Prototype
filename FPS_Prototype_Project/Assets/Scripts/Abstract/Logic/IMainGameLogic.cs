using UniRx;

namespace FPSProject
{
    public interface IMainGameLogic
    {
        IReadOnlyReactiveProperty<bool> IsCrouchModeEnabled { get; }
        IReadOnlyReactiveProperty<IWeaponObject> ActiveWeapon { get; }
        IReadOnlyReactiveProperty<float> MovementSpeed { get; }
        IReadOnlyReactiveProperty<float> RotationSpeed { get; }
        
        void SetCrouchModeActive(bool isActive);
        void SetActiveWeapon(IWeaponObject weapon);
        void SaveMovementSpeed(float movementSpeed, bool forceUpdate = true);
        void SaveRotationSpeed(float rotationSpeed, bool forceUpdate = true);
    }
}
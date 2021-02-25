using UniRx;
using UnityEngine;

namespace FPSProject
{
    public interface IInputLogic
    {
        IReadOnlyReactiveProperty<ControlsType> CurrentControlsType { get; }
        void SetControlsType(ControlsType type);

        Vector3 GetMoveInput();
        float GetLookInputsHorizontal();
        float GetLookInputsVertical();
        bool GetJumpInputDown();
        bool GetFireInputDown();
        bool GetReloadInputDown();
        bool GetCrouchModeInputDown();
        int GetSwitchWeaponInput();
        int GetSelectWeaponInput();
        bool GetPauseMenuInputDown();
    }
}
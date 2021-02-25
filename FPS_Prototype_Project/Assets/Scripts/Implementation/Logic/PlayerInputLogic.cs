using FPSProject.Impl.Configs;
using UniRx;
using UnityEngine;
using Zenject;

namespace FPSProject.Impl.Logic
{
    public class PlayerInputLogic : IInputLogic 
    {
        private readonly InputConfig _inputConfig;
        
        private readonly ReactiveProperty<ControlsType> _currentControlsType = new ReactiveProperty<ControlsType>();
        public IReadOnlyReactiveProperty<ControlsType> CurrentControlsType => _currentControlsType;

        public PlayerInputLogic(InputConfig inputConfig)
        {
            _inputConfig = inputConfig;
        }
        
        public void SetControlsType(ControlsType type)
        {
            _currentControlsType.Value = type;
            OnControlsTypeChanged(type);
        }

        public Vector3 GetMoveInput()
        {
            if (CanProcessInput())
            {
                Vector3 move = new Vector3(Input.GetAxisRaw(InputKeys.AXIS_HORIZONTAL), 0f, Input.GetAxisRaw(InputKeys.AXIS_VERTICAL));

                move = Vector3.ClampMagnitude(move, 1);

                return move;
            }

            return Vector3.zero;
        }

        public float GetLookInputsHorizontal()
        {
            return GetMouseLookAxis(InputKeys.AXIS_MOUSE_VERTICAL);
        }

        public float GetLookInputsVertical()
        {
            return GetMouseLookAxis(InputKeys.AXIS_MOUSE_HORIZONTAL);
        }

        public bool GetJumpInputDown()
        {
            if (CanProcessInput())
            {
                return Input.GetButtonDown(InputKeys.JUMP_BUTTON);
            }

            return false;
        }

        public bool GetFireInputDown()
        {
            return CanProcessInput() && Input.GetButtonDown(InputKeys.FIRE_BUTTON);
        }
        
        public bool GetReloadInputDown()
        {
            return CanProcessInput() && Input.GetButtonDown(InputKeys.RELOAD_BUTTON);
        }

        public bool GetCrouchModeInputDown()
        {
            return CanProcessInput() && Input.GetButtonDown(InputKeys.CROUCH_MODE_BUTTON);
        }
        
        public bool GetPauseMenuInputDown()
        {
            return CanProcessInput() && Input.GetButtonDown(InputKeys.PAUSE_MENU_BUTTON);
        }

        public int GetSwitchWeaponInput()
        {
            if (CanProcessInput())
            {
                if (Input.GetAxis(InputKeys.SWITCH_WEAPON_AXIS) > 0f) return -1;
                if (Input.GetAxis(InputKeys.SWITCH_WEAPON_AXIS) < 0f) return 1;
            }

            return 0;
        }

        public int GetSelectWeaponInput()
        {
            if (!CanProcessInput()) return -1;
            if (Input.GetKeyDown(KeyCode.Alpha1)) return 0;
            if (Input.GetKeyDown(KeyCode.Alpha2)) return 1;
            if (Input.GetKeyDown(KeyCode.Alpha3)) return 2;
            
            return -1;
        }

        private void OnControlsTypeChanged(ControlsType currentType)
        {
            switch (currentType)
            {
                case ControlsType.IngameLook:
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                    break;
                default: 
                    Cursor.lockState = CursorLockMode.Confined;
                    Cursor.visible = true;
                    break;
            }
        }
        
        private bool CanProcessInput()
        {
            return CurrentControlsType.Value == ControlsType.IngameLook;
        }
        
        private float GetMouseLookAxis(string mouseInputName)
        {
            if (!CanProcessInput()) return 0f;
            float inputValue = Input.GetAxisRaw(mouseInputName);
                
            if (_inputConfig.invertYAxis) inputValue *= -1f;
            
            inputValue *= _inputConfig.lookSensitivity * 0.01f;

            return inputValue;
        }
    }
}
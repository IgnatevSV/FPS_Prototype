using System;
using FPSProject.Impl.Configs;
using UnityEngine;
using Zenject;

namespace FPSProject.Impl.Controllers
{
    [RequireComponent(typeof(CharacterController))]
    public class MovementController : MonoBehaviour
    {
        private enum PositionState
        {
            Normal,
            InJump
        }
        
        private const float GROUND_CHECK_DISTANCE = 1f;
        private const float GROUND_CHECK_MIN_DISTANCE = 0.1f;
        
        public event Action OnJumped;
        public event Action OnLanded;
        
        [Inject] private IInputLogic _playerInputHandler;
        [Inject] private IMainGameLogic _mainGameLogic;
        [Inject] private PlayerMovementConfig _playerMovementConfig;
        [Inject] private WorldEnvironmentConfig _worldEnvironmentConfig;
        [Inject] private PlayerCameraConfig _playerCameraConfig;
        
        [SerializeField] private Camera _characterCamera;
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private LayerMask _groundCheckLayers = -1;
        
        
        
        
        private Transform _characterTransform;
        private Vector3 _groundNormal;
        private Vector3 _characterVelocity;
        private float _lastKnownJumpTime;
        private float _cameraAngleVertical;
        
        private PositionState _currentPositionState;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            //if (_playerInputHandler == null) _playerInputHandler = GetComponent<PlayerInput>();
            if (_characterController == null) _characterController = GetComponent<CharacterController>();

            _characterController.enableOverlapRecovery = true;
            _characterTransform = transform;
            
            _characterController.center = 0.5f * _characterController.height * Vector3.up;
            _characterCamera.transform.localPosition = Vector3.up * _characterController.height;
        }

        private void Update()
        {
            GroundCheck();
            
            if (_playerInputHandler.GetCrouchModeInputDown())
            {
                _mainGameLogic.SetCrouchModeActive(!_mainGameLogic.IsCrouchModeEnabled.Value);
            }
            
            UpdateMovement();
        }
        
        private void GroundCheck()
        {
            _groundNormal = Vector3.up;
            
            if (!IsJumpCooldownPassed()) return;
            if (!IsGrounded(out RaycastHit groundHit)) return;
            
            _groundNormal = groundHit.normal;

            if (IsLanded(groundHit))
            {
                _currentPositionState = PositionState.Normal;
                FixCharacterPositionAfterLand(groundHit);
                OnLanded?.Invoke();
            }
        }

        private void UpdateMovement()
        {
            UpdateCharacterRotation();
            UpdateCameraRotation();
            UpdateCharacterMovement();
            CalculateObstacleHit();
        }
        
        private void CalculateObstacleHit()
        {
            Vector3 capsuleBottomBeforeMove = GetCapsuleBottomHemisphere();
            Vector3 capsuleTopBeforeMove = GetCapsuleTopHemisphere(_characterController.height);
            _characterController.Move(_characterVelocity * Time.deltaTime);

            if (IsObstacleHit(capsuleBottomBeforeMove, capsuleTopBeforeMove, out RaycastHit obstacleHit))
            {
                _characterVelocity = Vector3.ProjectOnPlane(_characterVelocity, obstacleHit.normal);
            }
        }
        
        private bool IsObstacleHit(Vector3 capsuleBottom, Vector3 capsuleTop, out RaycastHit obstacleHit)
        {
            bool isObstacleHit = Physics.CapsuleCast(capsuleBottom,
                capsuleTop,
                _characterController.radius,
                _characterVelocity.normalized,
                out RaycastHit hit,
                _characterVelocity.magnitude * Time.deltaTime,
                -1,
                QueryTriggerInteraction.Ignore);

            obstacleHit = hit;

            return isObstacleHit;
        }
        
        private void UpdateCharacterMovement()
        {
            switch (_currentPositionState)
            {
                case PositionState.Normal:
                    CalculateNormalMovement();
                    break;
                case PositionState.InJump:
                    CalculateMovementInJump();
                    break;
            }
        }
        
        private void CalculateNormalMovement()
        {
            Vector3 targetVelocity = GetMovementInputData() * _mainGameLogic.MovementSpeed.Value;
            targetVelocity = GetDirectionReorientedOnSlope(targetVelocity.normalized, _groundNormal) * targetVelocity.magnitude;
            _characterVelocity = Vector3.Lerp(_characterVelocity, targetVelocity,_playerMovementConfig.MovementSharpnessOnGround * Time.deltaTime);

            if (CanPlayerJump() && _playerInputHandler.GetJumpInputDown())
            {
                _currentPositionState = PositionState.InJump;
                CalculateJumpInitialMovement();
            }
        }
        
        private bool CanPlayerJump()
        {
            return _currentPositionState == PositionState.Normal && !_mainGameLogic.IsCrouchModeEnabled.Value;
        }
        
        private void CalculateMovementInJump()
        {
            _characterVelocity += Time.deltaTime * _playerMovementConfig.AccelerationSpeedInAir * GetMovementInputData();

            float verticalVelocity = _characterVelocity.y;
            Vector3 horizontalVelocity = Vector3.ProjectOnPlane(_characterVelocity, Vector3.up);
            horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, _mainGameLogic.MovementSpeed.Value);
            _characterVelocity = horizontalVelocity + (Vector3.up * verticalVelocity);

            _characterVelocity += Time.deltaTime * _worldEnvironmentConfig.GravityForce * Vector3.down;
        }
        
        private Vector3 GetMovementInputData()
        {
            return _characterTransform.TransformVector(_playerInputHandler.GetMoveInput());
        }
        
        private void CalculateJumpInitialMovement()
        {
            _characterVelocity = new Vector3(_characterVelocity.x, 0f, _characterVelocity.z);
            _characterVelocity += Vector3.up * _playerMovementConfig.JumpForce;
            _lastKnownJumpTime = Time.time;
            _currentPositionState = PositionState.InJump;
            _groundNormal = Vector3.up;

            OnJumped?.Invoke();
        }

        private void UpdateCharacterRotation()
        {
            _characterTransform.Rotate(new Vector3(0f, GetHorizontalAxisRotation(), 0f), Space.Self);
        }
        
        private float GetHorizontalAxisRotation()
        {
            return _playerInputHandler.GetLookInputsHorizontal() * _mainGameLogic.RotationSpeed.Value;
        }

        private void UpdateCameraRotation()
        {
            _cameraAngleVertical += _playerInputHandler.GetLookInputsVertical() * _mainGameLogic.RotationSpeed.Value;
            _cameraAngleVertical = Mathf.Clamp(_cameraAngleVertical, _playerCameraConfig.CameraAngleMinVertical, _playerCameraConfig.CameraAngleMaxVertical);
            _characterCamera.transform.localEulerAngles = new Vector3(_cameraAngleVertical, 0, 0);
        }
        
        private Vector3 GetDirectionReorientedOnSlope(Vector3 direction, Vector3 slopeNormal)
        {
            Vector3 directionRight = Vector3.Cross(direction, transform.up);
            return Vector3.Cross(slopeNormal, directionRight).normalized;
        }
        
        private Vector3 GetCapsuleBottomHemisphere()
        {
            return _characterTransform.position + _characterTransform.up * _characterController.radius;
        }

        private Vector3 GetCapsuleTopHemisphere(float atHeight)
        {
            return _characterTransform.position + _characterTransform.up * (atHeight - _characterController.radius);
        }
        
        private bool IsNormalUnderSlopeLimit(Vector3 normal)
        {
            return Vector3.Angle(transform.up, normal) <= _characterController.slopeLimit;
        }

        private bool IsLanded(RaycastHit groundHit)
        {
            return Vector3.Dot(groundHit.normal, transform.up) > 0f && IsNormalUnderSlopeLimit(_groundNormal);
        }
        
        private bool IsJumpCooldownPassed()
        {
            return Time.time >= _lastKnownJumpTime + _playerMovementConfig.JumpCooldown;
        }
        
        private bool IsGrounded(out RaycastHit groundHit)
        {
            bool isGrounded = Physics.CapsuleCast(GetCapsuleBottomHemisphere(),
                GetCapsuleTopHemisphere(_characterController.height),
                _characterController.radius,
                Vector3.down,
                out RaycastHit hit,
                GetGroundCheckDistance(),
                _groundCheckLayers,
                QueryTriggerInteraction.Ignore);

            groundHit = hit;

            return isGrounded;
        }

        private float GetGroundCheckDistance()
        {
            return _currentPositionState == PositionState.Normal
                ? _characterController.skinWidth + GROUND_CHECK_DISTANCE
                : GROUND_CHECK_MIN_DISTANCE;
        }
        
        private void FixCharacterPositionAfterLand(RaycastHit groundHit)
        {
            if (groundHit.distance > _characterController.skinWidth)
            {
                _characterController.Move(Vector3.down * groundHit.distance);
            }
        }
    }
}
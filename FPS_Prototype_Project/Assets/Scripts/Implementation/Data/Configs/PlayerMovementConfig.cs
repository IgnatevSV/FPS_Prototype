using UnityEngine;

namespace FPSProject.Impl.Configs
{
    [CreateAssetMenu(fileName = "PlayerMovementConfig", menuName = "Configs/PlayerMovementConfig")]
    public class PlayerMovementConfig : ScriptableObject
    {
        [SerializeField] private float _defaultMovementSpeedNormal = 10f;
        [SerializeField] private float _minMovementSpeed = 1f;
        [SerializeField] private float _maxMovementSpeed = 50f;
        [SerializeField] private float _movementSpeedCrouchModeModifier = 0.5f;
        
        [SerializeField] private float _defaultRotationSpeedNormal = 200f;
        [SerializeField] private float _minRotationSpeed = 10f;
        [SerializeField] private float _maxRotationSpeed = 400f;
        
        [SerializeField] private bool _crouchModeAffectRotationSpeed = true;
        
        [SerializeField] private float _jumpForce = 9f;
        [SerializeField] [Min(0.1f)] private float _jumpCooldown = 1f;
        [SerializeField] private float _movementSharpnessOnGround = 15;
        [SerializeField] private float _accelerationSpeedInAir = 25f;

        public float DefaultMovementSpeedNormal => _defaultMovementSpeedNormal;
        public float MovementSpeedCrouchModeModifier => _movementSpeedCrouchModeModifier;
        public float DefaultRotationSpeedNormal => _defaultRotationSpeedNormal;
        public bool CrouchModeAffectRotationSpeed => _crouchModeAffectRotationSpeed;
        public float JumpCooldown => _jumpCooldown;
        public float AccelerationSpeedInAir => _accelerationSpeedInAir;
        public float MovementSharpnessOnGround => _movementSharpnessOnGround;
        public float JumpForce => _jumpForce;

        public float MinMovementSpeed => _minMovementSpeed;
        public float MaxMovementSpeed => _maxMovementSpeed;
        public float MinRotationSpeed => _minRotationSpeed;
        public float MaxRotationSpeed => _maxRotationSpeed;
    }
}
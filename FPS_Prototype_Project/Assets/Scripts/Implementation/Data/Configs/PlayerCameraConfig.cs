using UnityEngine;

namespace FPSProject.Impl.Configs
{
    [CreateAssetMenu(fileName = "PlayerCameraConfig", menuName = "Configs/PlayerCameraConfig")]
    public class PlayerCameraConfig : ScriptableObject
    {
        [SerializeField] private float _cameraAngleMinVertical = -70f;
        [SerializeField] private float _cameraAngleMaxVertical = 90f;
        
        public float CameraAngleMinVertical => _cameraAngleMinVertical;
        public float CameraAngleMaxVertical => _cameraAngleMaxVertical;
    }
}
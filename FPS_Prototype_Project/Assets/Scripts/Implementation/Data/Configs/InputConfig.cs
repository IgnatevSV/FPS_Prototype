using UnityEngine;

namespace FPSProject.Impl.Configs
{
    [CreateAssetMenu(fileName = "InputConfig", menuName = "Configs/InputConfig")]
    public class InputConfig : ScriptableObject
    {
        public float lookSensitivity = 1f;
        public float triggerAxisThreshold = 0.4f;
        public bool invertYAxis = false;
        public bool invertXAxis = false;
    }
}
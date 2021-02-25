using UnityEngine;

namespace FPSProject.Impl.Configs
{
    [CreateAssetMenu(fileName = "WorldEnvironmentConfig", menuName = "Configs/WorldEnvironmentConfig")]
    public class WorldEnvironmentConfig : ScriptableObject
    {
        [SerializeField] private float _gravityForce = 20f;
        
        public float GravityForce => _gravityForce;
    }
}
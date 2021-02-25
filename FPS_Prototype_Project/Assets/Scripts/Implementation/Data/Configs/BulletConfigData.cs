using UnityEngine;
using FPSProject.Impl.Components;

namespace FPSProject.Impl.Configs
{
    [CreateAssetMenu(fileName = "BulletConfigData", menuName = "Configs/Weapons/BulletConfigData")]
    public class BulletConfigData : ScriptableObject, IBulletConfigData
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _damage;
        [SerializeField] private float _gravityDownForce;

        [SerializeField] private float _maxRange;
        
        [SerializeField] private LayerMask _hittableLayers = -1;
        
        [SerializeField] private BaseBulletObject _prefab;
        
        [SerializeField] private float _bulletAimLineFixRange = 5f;

        [SerializeField] private float _collisionDetectError = 0.1f;
        
        public float CollisionDetectError => _collisionDetectError;

        public float Speed => _speed;
        public float Damage => _damage;
        public float GravityDownForce => _gravityDownForce;
        public float MaxRange => _maxRange;
        public LayerMask HittableLayers => _hittableLayers;
        
        public IBulletObject Prefab => _prefab;

        public float BulletAimLineFixRange => _bulletAimLineFixRange;
    }
}
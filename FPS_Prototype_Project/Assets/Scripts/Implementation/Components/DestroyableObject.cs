using FPSProject.Impl.Configs;
using UnityEngine;
using Zenject;

namespace FPSProject.Impl.Components
{
    [RequireComponent(typeof(Rigidbody))]
    public class DestroyableObject : BaseDestroyableObject, IPhysicsDestroyableObject
    {
        [Inject] private DestroyableObjectsConfig _destroyableObjectsConfig;

        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private int _destroyableObjectId;

        public Rigidbody Rigidbody => _rigidbody;
        
        private void Awake()
        {
            if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody>();
            
            IDestroyableObjectData data = _destroyableObjectsConfig.GetDestroyableObjectDataById(_destroyableObjectId);
            Init(data);
        }
    }
}
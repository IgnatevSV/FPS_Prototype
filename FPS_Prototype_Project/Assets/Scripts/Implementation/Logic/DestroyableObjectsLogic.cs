using System;
using System.Collections.Generic;
using FPSProject.Impl.Configs;
using Zenject;

namespace FPSProject.Impl.Logic
{
    public class DestroyableObjectsLogic : IDestroyableObjectsLogic
    {
        [Inject] private IMainGameLogic _mainGameLogic;
        [Inject] private DestroyableObjectsConfig _destroyableObjectsConfig;
        
        public event Action<DestroyMeta> OnDestroyableObjectKilled;

        private readonly HashSet<IDestroyableObject> _destroyableObjectsAtScene = new HashSet<IDestroyableObject>();

        public void Register(IDestroyableObject destroyableObject)
        {
            _destroyableObjectsAtScene.Add(destroyableObject);

            destroyableObject.OnDestroy += OnDestroyableObjectDestroyedHandler;
        }
        
        public float CalculateDamage(IBulletObject bulletObject)
        {
            float defaultDamage = bulletObject.BulletConfigData.Damage;

            return _mainGameLogic.IsCrouchModeEnabled.Value
                ? defaultDamage * _destroyableObjectsConfig.CrouchModeDamageModifier
                : defaultDamage;
        }

        private void OnDestroyableObjectDestroyedHandler(DestroyMeta destroyMeta)
        {
            destroyMeta.DestroyedObject.OnDestroy -= OnDestroyableObjectDestroyedHandler;

            _destroyableObjectsAtScene.Remove(destroyMeta.DestroyedObject);
            
            OnDestroyableObjectKilled?.Invoke(destroyMeta);
        }
    }
}
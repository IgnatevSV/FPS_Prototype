using System;

namespace FPSProject
{
    public interface IDestroyableObjectsLogic
    {
        event Action<DestroyMeta> OnDestroyableObjectKilled;
        void Register(IDestroyableObject destroyableObject);
        float CalculateDamage(IBulletObject bulletObject);
    }
}
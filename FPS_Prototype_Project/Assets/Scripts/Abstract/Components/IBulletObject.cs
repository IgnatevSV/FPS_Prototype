using UnityEngine;

namespace FPSProject
{
    public interface IBulletObject : IPoolableObject
    {
        IBulletConfigData BulletConfigData { get; }
        Transform Transform { get; }
        void Init(IBulletConfigData bulletConfigData, BulletRuntimeMeta metaData);
    }
}
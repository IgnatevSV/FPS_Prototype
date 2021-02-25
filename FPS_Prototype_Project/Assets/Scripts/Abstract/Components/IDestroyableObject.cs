using System;
using UniRx;
using UnityEngine;

namespace FPSProject
{
    public interface IDestroyableObject
    {
        event Action<DestroyMeta> OnDestroy;
        IDestroyableObjectData Data { get; }
        IReadOnlyReactiveProperty<float> CurrentHealth { get; }
        IReadOnlyReactiveProperty<bool> IsInited { get; }
        Transform Transform { get; }
        
        void Init(IDestroyableObjectData data);
        void SetDamage(IBulletObject bulletObject);
    }
}
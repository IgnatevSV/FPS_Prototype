using UnityEngine;

namespace FPSProject
{
    public interface IBulletConfigData : IConfigData
    {
        float CollisionDetectError { get; }
        float Speed { get; }
        float Damage { get; }
        float GravityDownForce { get; }
        float MaxRange { get; }
        LayerMask HittableLayers { get; }
        IBulletObject Prefab { get; }
        float BulletAimLineFixRange { get; }
    }
}
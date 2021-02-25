using UnityEngine;

namespace FPSProject
{
    public interface IPhysicsDestroyableObject : IDestroyableObject
    {
        Rigidbody Rigidbody { get; }
    }
}
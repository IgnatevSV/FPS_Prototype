using UnityEngine;

namespace FPSProject.Impl.Components
{
    public class PiercingBulletObject : BaseBulletObject
    {
        protected override void DamageHittedObjects(RaycastHit hit)
        {
            IDestroyableObject destroyableObject = hit.transform.GetComponent<IDestroyableObject>();
            destroyableObject?.SetDamage(this);
        }
    }
}
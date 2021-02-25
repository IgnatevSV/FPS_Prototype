using UnityEngine;
using FPSProject.Impl.Configs;

namespace FPSProject.Impl.Components
{
    public class ExplosionBulletObject : BaseBulletObject
    {
        private ExplosionBulletConfigData _explosionBulletConfigData;
        
        private Vector3 _positionAtExplode;
        
        public override void Init(IBulletConfigData bulletConfigData, BulletRuntimeMeta metaData)
        {
            base.Init(bulletConfigData, metaData);
            
            _explosionBulletConfigData = BulletConfigData as ExplosionBulletConfigData;
        }
        
        protected override void DamageHittedObjects(RaycastHit hit)
        {
            if (_explosionBulletConfigData != null)
            {
                InflictDamageInArea(hit.point);
            }
        }

        private void InflictDamageInArea(Vector3 explosionCenter)
        {
            _positionAtExplode = transform.position;

            Collider[] affectedColliders = GetHittedColliders(explosionCenter);

            foreach (Collider collider in affectedColliders)
            {
                IDestroyableObject destroyableObject = collider.GetComponent<IDestroyableObject>();

                if (destroyableObject == null) continue;
                
                destroyableObject.SetDamage(this);
                AffectForce(destroyableObject, collider.transform.position);
            }
        }

        private Collider[] GetHittedColliders(Vector3 center)
        {
            return Physics.OverlapSphere(center, _explosionBulletConfigData.ExlosionRadius, BulletConfigData.HittableLayers, QueryTriggerInteraction.Collide);
        }

        private void AffectForce(IDestroyableObject hittedObject, Vector3 hitPosition)
        {
            if (hittedObject is IPhysicsDestroyableObject physicsDestroyableObject && physicsDestroyableObject.Rigidbody != null)
            {
                physicsDestroyableObject.Rigidbody.AddExplosionForce(GetExplosionForce(hitPosition),
                    _positionAtExplode,
                    _explosionBulletConfigData.ExlosionRadius,
                    _explosionBulletConfigData.UpliftForce,
                    ForceMode.Impulse);
            }
        }

        private float GetExplosionForce(Vector3 hitPosition)
        {
            float explosionRadius = _explosionBulletConfigData.ExlosionRadius;
            float distance = Vector3.Distance(hitPosition, _positionAtExplode);
            float explosionModifier = _explosionBulletConfigData.ExplosionForceOverDistanceCurve.Evaluate(distance / explosionRadius);
            return _explosionBulletConfigData.ExplosionForce * explosionModifier;
        }
    }
}
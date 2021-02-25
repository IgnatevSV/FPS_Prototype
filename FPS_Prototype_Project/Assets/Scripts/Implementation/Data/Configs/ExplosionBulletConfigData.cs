using UnityEngine;

namespace FPSProject.Impl.Configs
{
    [CreateAssetMenu(fileName = "ExplosionBulletConfigData", menuName = "Configs/Weapons/ExplosionBulletConfigData")]
    public class ExplosionBulletConfigData : BulletConfigData
    {
        [SerializeField] private float _explosionForce = 40f;
        [SerializeField] private float _exlosionRadius = 5f;
        [SerializeField] private float _upliftForce = 10f;
        [SerializeField] private AnimationCurve _explosionForceOverDistanceCurve;

        public float ExplosionForce => _explosionForce;
        public float ExlosionRadius => _exlosionRadius;
        public float UpliftForce => _upliftForce;
        public AnimationCurve ExplosionForceOverDistanceCurve => _explosionForceOverDistanceCurve;
    }
}
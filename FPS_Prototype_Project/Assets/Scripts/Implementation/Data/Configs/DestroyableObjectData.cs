using UnityEngine;

namespace FPSProject.Impl.Configs
{
    [CreateAssetMenu(fileName = "DestroyableObjectData", menuName = "Configs/DestroyableObjects/DestroyableObjectData")]
    public class DestroyableObjectData : ScriptableObject, IDestroyableObjectData
    {
        [SerializeField] private float _maxHealth;
        public float MaxHealth => _maxHealth;
    }
}
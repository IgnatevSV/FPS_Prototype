using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FPSProject.Impl.Configs.Extensions;

namespace FPSProject.Impl.Configs
{
    [CreateAssetMenu(fileName = "DestroyableObjectsConfig", menuName = "Configs/DestroyableObjects/DestroyableObjectsConfig")]
    public class DestroyableObjectsConfig : ScriptableObject
    {
        [SerializeField] private float _crouchModeDamageModifier = 2f;
        [SerializeField] private DestroyableObjectsDictionary _destroyableObjects;
        [SerializeField] private PoolableObjectsDictionary _poolPrefabToPreloadInstances;
        
        public float CrouchModeDamageModifier => _crouchModeDamageModifier;

        public IDestroyableObjectData GetDestroyableObjectDataById(int id) => _destroyableObjects.GetConfigDataById(id);

        public IReadOnlyList<IPoolableObject> PoolableObjectPrefabs => _poolPrefabToPreloadInstances?.Keys.Cast<IPoolableObject>().ToList();

        public int GetPoolableObjectPrefabPreloadCount(IPoolableObject prefab)
        {
            MonoBehaviour monoBehaviourPrefab = prefab as MonoBehaviour;
            
            return monoBehaviourPrefab != null && _poolPrefabToPreloadInstances != null && _poolPrefabToPreloadInstances.ContainsKey(monoBehaviourPrefab)
                ? _poolPrefabToPreloadInstances[monoBehaviourPrefab]
                : 0;
        }
    }
}
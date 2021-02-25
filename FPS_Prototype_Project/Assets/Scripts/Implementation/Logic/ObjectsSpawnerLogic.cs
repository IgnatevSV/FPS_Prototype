using System.Collections.Generic;
using System.Linq;
using FPSProject.Impl.Configs;
using UnityEngine;
using Zenject;

namespace FPSProject.Impl.Logic
{
    public class ObjectsSpawnerLogic : IObjectsSpawnerLogic, IInitializable
    {
        [Inject] private DiContainer _diContainer;
        [Inject] private DestroyableObjectsConfig _destroyableObjectsConfig;
        
        private readonly Dictionary<IPoolableObject, List<IPoolableObject>> _prefabToInstancesPool = new Dictionary<IPoolableObject, List<IPoolableObject>>();
        
        public void Initialize()
        {
            InitPool();
        }
        
        public IPoolableObject SpawnPoolObject<T>(T objectPrefab) where T : Component, IPoolableObject
        {
            T spawnedObject;
            
            if (_prefabToInstancesPool.ContainsKey(objectPrefab))
            {
                if (_prefabToInstancesPool[objectPrefab] == null)
                {
                    _prefabToInstancesPool[objectPrefab] = new List<IPoolableObject>();
                }

                IPoolableObject poolableObject = _prefabToInstancesPool[objectPrefab].FirstOrDefault();

                if (poolableObject == null)
                {
                    spawnedObject = CreateNewMonoBehaviourInstance(objectPrefab);
                }
                else
                {
                    _prefabToInstancesPool[objectPrefab].Remove(poolableObject);
                    spawnedObject = (T) poolableObject;
                }
            }
            else
            {
                spawnedObject = CreateNewMonoBehaviourInstance(objectPrefab);
            }

            spawnedObject.IsInUse = true;
            spawnedObject.gameObject.SetActive(true);

            return spawnedObject;
        }
        
        public void DespawnPoolObject<T>(T objectInstance) where T : Component, IPoolableObject
        {
            objectInstance.IsInUse = false;
            objectInstance.ResetPoolableObject();
            objectInstance.gameObject.SetActive(false);

            IPoolableObject prefab = objectInstance.Prefab;
            
            if (!_prefabToInstancesPool.ContainsKey(prefab))
            {
                _prefabToInstancesPool.Add(prefab, new List<IPoolableObject>());
            }
            
            _prefabToInstancesPool[prefab].Add(objectInstance);
        }

        public T CreateNewMonoBehaviourInstance<T>(T prefab) where T : Component
        {
            return _diContainer.InstantiatePrefabForComponent<T>(prefab);
        }
        
        public GameObject CreateNewGameObjectInstance<T>(T prefab) where T : Object
        {
            return _diContainer.InstantiatePrefab(prefab);
        }
        
        private void InitPool()
        {
            foreach (IPoolableObject prefab in _destroyableObjectsConfig.PoolableObjectPrefabs)
            {
                int instancesCount = _destroyableObjectsConfig.GetPoolableObjectPrefabPreloadCount(prefab);

                _prefabToInstancesPool.Add(prefab, new List<IPoolableObject>(instancesCount));
                
                for (int i = 0; i < instancesCount; i++)
                {
                    if (!(prefab is MonoBehaviour prefabMonoBeh)) continue;
                    if (CreateNewMonoBehaviourInstance(prefabMonoBeh) is IPoolableObject instance)
                    {
                        instance.IsInUse = false;
                        ((MonoBehaviour) instance).gameObject.SetActive(false);
                        _prefabToInstancesPool[prefab].Add(instance);
                    }
                }
            }
        }
    }
}
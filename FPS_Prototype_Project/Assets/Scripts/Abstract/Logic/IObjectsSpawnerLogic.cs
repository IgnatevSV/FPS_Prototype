using UnityEngine;

namespace FPSProject
{
    public interface IObjectsSpawnerLogic
    {
        IPoolableObject SpawnPoolObject<T>(T objectPrefab) where T : Component, IPoolableObject;
        void DespawnPoolObject<T>(T objectInstance) where T : Component, IPoolableObject;
        T CreateNewMonoBehaviourInstance<T>(T prefab) where T : Component;
        GameObject CreateNewGameObjectInstance<T>(T prefab) where T : Object;
    }
}
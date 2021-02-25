using UnityEngine;
using Zenject;

namespace FPSProject.Impl.Controllers
{
    public class OptionsMenuController : MonoBehaviour
    {
        [Inject] private IInputLogic _inputLogic;
        [Inject] private IObjectsSpawnerLogic _objectsSpawnerLogic;

        [SerializeField] private GameObject _pauseMenuWindowPrefab;

        private GameObject _currentSpawnedObject;
        
        private void Update()
        {
            if (_inputLogic.GetPauseMenuInputDown() && _currentSpawnedObject == null)
            {
                _currentSpawnedObject = _objectsSpawnerLogic.CreateNewGameObjectInstance(_pauseMenuWindowPrefab);
                _currentSpawnedObject.transform.parent = transform;
                _currentSpawnedObject.transform.localPosition = Vector3.zero;
            }
        }
    }
}
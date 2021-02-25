using UnityEngine;
using Zenject;

namespace FPSProject.Impl.Utils
{
    public class SceneDefaultInputInitializer : MonoBehaviour
    {
        [Inject] private IInputLogic _inputLogic;

        [SerializeField] private ControlsType _sceneDefaultInput;
        
        private void Awake()
        {
            _inputLogic.SetControlsType(_sceneDefaultInput);
        }
    }
}
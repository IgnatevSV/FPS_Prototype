using UnityEngine;

namespace FPSProject.Impl.Configs
{
    [CreateAssetMenu(fileName = "ScenesConfig", menuName = "Configs/Main/ScenesConfig")]
    public class ScenesConfig : ScriptableObject
    {
#if UNITY_EDITOR
        [SerializeField] private UnityEditor.SceneAsset _mainGameScene;
        [SerializeField] private UnityEditor.SceneAsset _mainMenuScene;
        [SerializeField] private UnityEditor.SceneAsset _optionsScene;
#endif
        
        [SerializeField] [HideInInspector] private string _mainGameSceneName;
        [SerializeField] [HideInInspector] private string _mainMenuSceneName;
        [SerializeField] [HideInInspector] private string _optionsSceneName;
        
        public string MainGameSceneName => _mainGameSceneName;
        public string MainMenuSceneName => _mainMenuSceneName;
        public string OptionsSceneName => _optionsSceneName;

#if UNITY_EDITOR
        public void SaveScenesData()
        {
            _mainGameSceneName = _mainGameScene.name;
            _mainMenuSceneName = _mainMenuScene.name;
            _optionsSceneName = _optionsScene.name;
        }
#endif
    }
}
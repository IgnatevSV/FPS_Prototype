using UnityEditor;
using UnityEngine;

namespace FPSProject.Impl.Configs
{
    [CreateAssetMenu(fileName = "ScenesConfig", menuName = "Configs/Main/ScenesConfig")]
    public class ScenesConfig : ScriptableObject
    {
        [SerializeField] private SceneAsset _mainGameScene;
        [SerializeField] private SceneAsset _mainMenuScene;
        [SerializeField] private SceneAsset _optionsScene;

        public SceneAsset MainGameScene => _mainGameScene;
        public SceneAsset MainMenuScene => _mainMenuScene;
        public SceneAsset OptionsScene => _optionsScene;
    }
}
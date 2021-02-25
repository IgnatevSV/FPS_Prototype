using FPSProject.Impl.Configs;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace FPSProject.Impl.Logic
{
    public class ScenesLogic : IScenesLogic
    {
        private readonly ISaves _savesLogic;
        private readonly ScenesConfig _scenesConfig;

        [Inject]
        public ScenesLogic(ISaves savesLogic, ScenesConfig scenesConfig)
        {
            _savesLogic = savesLogic;
            _scenesConfig = scenesConfig;
        }
        
        public void StartGame()
        {
            LoadScene(_scenesConfig.MainGameScene);
        }

        public void OpenMainMenu()
        {
            LoadScene(_scenesConfig.MainMenuScene);
        }

        public void OpenOptionsMenu()
        {
            LoadScene(_scenesConfig.OptionsScene);
        }

        private void LoadScene(SceneAsset sceneAsset)
        {
            _savesLogic.Save();
            SceneManager.LoadScene(sceneAsset.name);
        }

        public void ExitGame()
        {
            _savesLogic.Save();
            Application.Quit();
        }
    }
}
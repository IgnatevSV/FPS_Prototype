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
            LoadScene(_scenesConfig.MainGameSceneName);
        }

        public void OpenMainMenu()
        {
            LoadScene(_scenesConfig.MainMenuSceneName);
        }

        public void OpenOptionsMenu()
        {
            LoadScene(_scenesConfig.OptionsSceneName);
        }

        private void LoadScene(string sceneAssetName)
        {
            _savesLogic.Save();
            SceneManager.LoadScene(sceneAssetName);
        }

        public void ExitGame()
        {
            _savesLogic.Save();
            Application.Quit();
        }
    }
}
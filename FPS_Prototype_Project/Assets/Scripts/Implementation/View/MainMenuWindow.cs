using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FPSProject.Impl.Views
{
    public class MainMenuWindow : MonoBehaviour
    {
        [Inject] private IScenesLogic _scenesLogic;

        [SerializeField] private Button _startButton;
        [SerializeField] private Button _optionsButton;
        [SerializeField] private Button _exitButton;

        private void Awake()
        {
            _startButton.onClick.AddListener(OnStartButtonClickHandler);
            _optionsButton.onClick.AddListener(OnOptionsButtonClickHandler);
            _exitButton.onClick.AddListener(OnExitButtonClickHandler);
        }

        private void OnDestroy()
        {
            _startButton.onClick.RemoveListener(OnStartButtonClickHandler);
            _optionsButton.onClick.RemoveListener(OnOptionsButtonClickHandler);
            _exitButton.onClick.RemoveListener(OnExitButtonClickHandler);
        }

        private void OnStartButtonClickHandler()
        {
            _scenesLogic.StartGame();
        }
        
        private void OnOptionsButtonClickHandler()
        {
            _scenesLogic.OpenOptionsMenu();
        }
        
        private void OnExitButtonClickHandler()
        {
            _scenesLogic.ExitGame();
        }
    }
}
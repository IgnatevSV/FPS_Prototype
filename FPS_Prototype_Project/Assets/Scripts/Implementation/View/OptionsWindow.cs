using System;
using FPSProject.Impl.Configs;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FPSProject.Impl.Views
{
    [RequireComponent(typeof(IView))]
    public class OptionsWindow : MonoBehaviour
    {
        [Inject] private IScenesLogic _scenesLogic;
        [Inject] private IInputLogic _inputLogic;
        [Inject] private IMainGameLogic _mainGameLogic;
        [Inject] private PlayerMovementConfig _playerMovementConfig;

        [SerializeField] private Slider _movementSpeedSlider;
        [SerializeField] private Slider _rotationSpeedSlider;
        [SerializeField] private TextMeshProUGUI _movementSpeedInfo;
        [SerializeField] private TextMeshProUGUI _rotationSpeedInfo;
        
        [SerializeField] private Button _revertButton;
        [SerializeField] private Button _exitToMainMenuButton;
        [SerializeField] private Button _exitGameButton;
        [SerializeField] private Button _closeButton;
        
        private IView _attachedView;

        private void Awake()
        {
            _attachedView = GetComponent<IView>();
            
            InitButtons();
            InitSlider();
        }
        
        private void InitButtons()
        {
            _revertButton.onClick.AddListener(OnRevertButtonClickHandler);
            _exitToMainMenuButton.onClick.AddListener(OnExitToMainMenuButtonClickHandler);
            _exitGameButton.onClick.AddListener(OnExitGameButtonClickHandler);
            _closeButton.onClick.AddListener(OnCloseButtonClickHandler);
        }
        
        private void OnEnable()
        {
            _inputLogic.SetControlsType(ControlsType.Ui);
        }

        private void OnDisable()
        {
            _inputLogic.SetControlsType(ControlsType.IngameLook);
        }
        
        private void OnDestroy()
        {
            _revertButton.onClick.AddListener(OnRevertButtonClickHandler);
            _exitToMainMenuButton.onClick.AddListener(OnExitToMainMenuButtonClickHandler);
            _exitGameButton.onClick.AddListener(OnExitGameButtonClickHandler);
            _closeButton.onClick.AddListener(OnCloseButtonClickHandler);
        }

        private void InitSlider()
        {
            _movementSpeedSlider.minValue = _playerMovementConfig.MinMovementSpeed;
            _movementSpeedSlider.maxValue = _playerMovementConfig.MaxMovementSpeed;

            _rotationSpeedSlider.minValue = _playerMovementConfig.MinRotationSpeed;
            _rotationSpeedSlider.maxValue = _playerMovementConfig.MaxRotationSpeed;

            _movementSpeedSlider.onValueChanged.AddListener(OnMovementSpeedSliderValueChangedHandler);
            _rotationSpeedSlider.onValueChanged.AddListener(OnRotationSpeedSliderValueChangedHandler);
            
            _movementSpeedSlider.value = _mainGameLogic.MovementSpeed.Value;
            _rotationSpeedSlider.value = _mainGameLogic.RotationSpeed.Value;

            OnMovementSpeedSliderValueChangedHandler(_mainGameLogic.MovementSpeed.Value);
            OnRotationSpeedSliderValueChangedHandler(_mainGameLogic.RotationSpeed.Value);
        }

        private void OnMovementSpeedSliderValueChangedHandler(float value)
        {
            UpdateSliderValueInfo(_movementSpeedInfo, value, _movementSpeedSlider.maxValue);
        }

        private void OnRotationSpeedSliderValueChangedHandler(float value)
        {
            UpdateSliderValueInfo(_rotationSpeedInfo, value, _rotationSpeedSlider.maxValue);
        }

        private void UpdateSliderValueInfo(TextMeshProUGUI infoText, float currentValue, float maxValue)
        {
            float normalizedValue = (currentValue / maxValue) * 100;

            infoText.text = normalizedValue.ToString("0");
        }
        
        private void OnCloseButtonClickHandler()
        {
            CloseWindow();
        }
        
        private void OnRevertButtonClickHandler()
        {
            _movementSpeedSlider.value = _playerMovementConfig.DefaultMovementSpeedNormal;
            _rotationSpeedSlider.value = _playerMovementConfig.DefaultRotationSpeedNormal;
        }

        private void CloseWindow()
        {
            SaveValues();
            
            IDisposable onViewHided = null;

            onViewHided = _attachedView.State.Where(s => s == ViewState.Invisible).Subscribe(_ =>
            {
                Destroy(this.gameObject);
                onViewHided?.Dispose();
            });
            
            _attachedView.Hide();
        }
        
        private void SaveValues()
        {
            float movementSettedValue = _movementSpeedSlider.value;
            float rotationSettedValue = _rotationSpeedSlider.value;

            if (movementSettedValue > 0f)
            {
                _mainGameLogic.SaveMovementSpeed(movementSettedValue);
            }

            if (rotationSettedValue > 0f)
            {
                _mainGameLogic.SaveRotationSpeed(rotationSettedValue);
            }
        }

        private void OnExitToMainMenuButtonClickHandler()
        {
            SaveValues();
            _scenesLogic.OpenMainMenu();
        }

        private void OnExitGameButtonClickHandler()
        {
            SaveValues();
            _scenesLogic.ExitGame();
        }
    }
}
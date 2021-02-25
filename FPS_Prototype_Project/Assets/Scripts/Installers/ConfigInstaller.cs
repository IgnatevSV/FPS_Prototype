using FPSProject.Impl.Configs;
using UnityEngine;
using Zenject;

namespace FPSProject.Installers
{
    [CreateAssetMenu(fileName = "ConfigInstaller", menuName = "Configs/Main/ConfigInstaller")]
    public class ConfigInstaller : ScriptableObjectInstaller
    {
        [SerializeField] private PlayerMovementConfig _playerMovementConfig;
        [SerializeField] private PlayerCameraConfig _playerCameraConfig;
        [SerializeField] private WorldEnvironmentConfig _worldEnvironmentConfig;
        [SerializeField] private InputConfig _inputConfig;
        [SerializeField] private ScoreCounterConfig _scoreCounterConfig;
        [SerializeField] private WeaponsConfig _weaponsConfig;
        [SerializeField] private DestroyableObjectsConfig _destroyableObjectsConfig;
        [SerializeField] private ScenesConfig _scenesConfig;
        
        public override void InstallBindings()
        {
            Container.BindInstance(_playerMovementConfig).AsSingle();
            Container.BindInstance(_playerCameraConfig).AsSingle();
            Container.BindInstance(_worldEnvironmentConfig).AsSingle();
            Container.BindInstance(_inputConfig).AsSingle();
            Container.BindInstance(_scoreCounterConfig).AsSingle();
            Container.BindInstance(_weaponsConfig).AsSingle();
            Container.BindInstance(_destroyableObjectsConfig).AsSingle();
            Container.BindInstance(_scenesConfig).AsSingle();
        }
    }
}
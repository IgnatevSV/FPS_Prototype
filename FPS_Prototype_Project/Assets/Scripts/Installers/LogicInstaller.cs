using FPSProject.Impl.Logic;
using Zenject;

namespace FPSProject.Installers
{
    public class LogicInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<MainGameLogic>().AsSingle();
            Container.BindInterfacesAndSelfTo<SavesLogic>().AsSingle();
            Container.BindInterfacesAndSelfTo<DestroyableObjectsLogic>().AsSingle();
            Container.BindInterfacesAndSelfTo<ScoreLogic>().AsSingle();
            Container.BindInterfacesAndSelfTo<ObjectsSpawnerLogic>().AsSingle();
            Container.BindInterfacesAndSelfTo<ScenesLogic>().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerInputLogic>().AsSingle();
        }
    }
}
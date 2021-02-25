using UniRx;

namespace FPSProject
{
    public interface IScoreLogic
    {
        IReadOnlyReactiveProperty<int> CurrentScore { get; }
        IReadOnlyReactiveProperty<int> BestScore { get; }
    }
}
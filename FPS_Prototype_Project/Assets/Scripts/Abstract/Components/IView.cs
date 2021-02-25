using UniRx;

namespace FPSProject
{
    public interface IView
    {
        IReadOnlyReactiveProperty<ViewState> State { get; }
        
        void Init(ViewState state);
        void Show();
        void Hide();
    }
}
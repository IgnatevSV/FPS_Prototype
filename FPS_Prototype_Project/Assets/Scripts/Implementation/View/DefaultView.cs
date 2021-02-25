using UniRx;
using UnityEngine;

namespace FPSProject.Impl.Views
{
    public class DefaultView : MonoBehaviour, IView
    {
        private ReactiveProperty<ViewState> _state = new ReactiveProperty<ViewState>();

        public IReadOnlyReactiveProperty<ViewState> State => _state;

        public void Init(ViewState state)
        {
            _state = new ReactiveProperty<ViewState>(state);

            if (state == ViewState.Hiding || state == ViewState.Invisible)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }
        
        public void Show()
        {
            gameObject.SetActive(true);
            _state.Value = ViewState.Visible;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            _state.Value = ViewState.Invisible;
        }
    }
}
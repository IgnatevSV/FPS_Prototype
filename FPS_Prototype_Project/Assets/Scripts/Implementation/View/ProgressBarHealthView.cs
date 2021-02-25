using UnityEditor.UIElements;
using UnityEngine;

namespace FPSProject.Impl.Views
{
    public class ProgressBarHealthView : BaseHealthView
    {
        [SerializeField] private ProgressBar _progressBar;

        protected override void SetHealthValue(float value)
        {
            _progressBar.value = value;
        }
    }
}
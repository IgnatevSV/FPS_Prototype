using UnityEngine;
using Zenject;

namespace FPSProject.Installers
{
    public class ContextInstaller : MonoBehaviour
    {
        [SerializeField] private SceneContext _defaultContext;

        private void Awake()
        {
            SceneContext curContext = FindObjectOfType<SceneContext>();
            if (curContext == null && _defaultContext != null)
            {
                Instantiate(_defaultContext);
            }
        }
    }
}
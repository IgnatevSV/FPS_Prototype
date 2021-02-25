using UnityEngine;

namespace FPSProject.Impl.Utils
{
    public class AutoDestroyableObject : MonoBehaviour
    {
        [SerializeField] [Min(0)] private float _destroyTime;

        private void Awake()
        {
            Destroy(this.gameObject, _destroyTime);
        }
    }
}
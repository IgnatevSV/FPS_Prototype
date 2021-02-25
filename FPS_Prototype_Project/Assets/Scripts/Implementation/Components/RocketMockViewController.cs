using UniRx;
using UnityEngine;

namespace FPSProject.Impl.Components
{
    [RequireComponent(typeof(BaseWeaponObject))]
    public class RocketMockViewController : MonoBehaviour
    {
        [SerializeField] private BaseWeaponObject _baseWeaponObject;
        [SerializeField] private GameObject _rocketMock;

        private void Awake()
        {
            if (_baseWeaponObject == null) _baseWeaponObject = GetComponent<BaseWeaponObject>();
            _baseWeaponObject.CurrentAmmo.Subscribe(OnAmmoChanged).AddTo(this);
        }

        private void OnAmmoChanged(int currentAmmo)
        {
            _rocketMock.SetActive(currentAmmo > 0);
        }
    }
}
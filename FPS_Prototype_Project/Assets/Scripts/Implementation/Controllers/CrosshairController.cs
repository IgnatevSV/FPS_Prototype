using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

namespace FPSProject.Impl.Controllers
{
    public class CrosshairController : MonoBehaviour
    {
        private readonly Dictionary<GameObject, GameObject> _cachedCrosshairs = new Dictionary<GameObject, GameObject>();
        
        [Inject] private IMainGameLogic _mainGameLogic;

        [SerializeField] private Transform _crosshairPosition;

        private GameObject _activeCrosshair;
        
        private void Awake()
        {
            _mainGameLogic.ActiveWeapon.Subscribe(UpdateCrosshair).AddTo(this);
        }

        private void UpdateCrosshair(IWeaponObject weaponObject)
        {
            if (weaponObject == null) return;

            if (_activeCrosshair != null) _activeCrosshair.SetActive(false);
            
            GameObject crosshairPrefab = weaponObject.WeaponConfigData.CrosshairPrefab;

            if (crosshairPrefab == null) return;
            
            if (!_cachedCrosshairs.ContainsKey(crosshairPrefab))
            {
                GameObject newCrosshairInstance = Instantiate(crosshairPrefab, _crosshairPosition);
                _cachedCrosshairs.Add(crosshairPrefab, newCrosshairInstance);
            }

            _activeCrosshair = _cachedCrosshairs[crosshairPrefab];
            _activeCrosshair.SetActive(true);
        }
    }
}
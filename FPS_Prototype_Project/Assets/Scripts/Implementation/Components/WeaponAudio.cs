using UnityEngine;

namespace FPSProject.Impl.Components
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(BaseWeaponObject))]
    public class WeaponAudio : MonoBehaviour
    {
        [SerializeField] private BaseWeaponObject _weaponObject;
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private AudioClip _shotClip;
        [SerializeField] private AudioClip _emptyShotClip;
        
        private void Init()
        {
            if (_weaponObject == null) _weaponObject = GetComponent<BaseWeaponObject>();
            if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
            
            _weaponObject.OnShot += OnWeaponShotHandler;
            _weaponObject.OnEmptyShotTrigger += OnEmptyShotTriggerHandler;
        }

        private void OnDestroy()
        {
            _weaponObject.OnShot -= OnWeaponShotHandler;
            _weaponObject.OnEmptyShotTrigger -= OnEmptyShotTriggerHandler;
        }

        private void OnWeaponShotHandler()
        {
            _audioSource.PlayOneShot(_shotClip);
        }
        
        private void OnEmptyShotTriggerHandler()
        {
            _audioSource.PlayOneShot(_emptyShotClip);
        }
    }
}
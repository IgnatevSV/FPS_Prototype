using UnityEngine;

namespace FPSProject.Impl.Controllers
{
    [RequireComponent(typeof(MovementController))]
    [RequireComponent(typeof(AudioSource))]
    public class MovementAudio : MonoBehaviour
    {
        [SerializeField] private MovementController _movementController;

        [SerializeField] private AudioSource _audioSource;
        
        [SerializeField] private AudioClip _stepmoveClip;
        [SerializeField] private AudioClip _jumpClip;
        [SerializeField] private AudioClip _landClip;

        private void Awake()
        {
            Init();
            SubscribeControllerEvents();
        }

        private void Init()
        {
            if (_movementController == null) _movementController = GetComponent<MovementController>();
            if (_audioSource == null) _audioSource = GetComponent<AudioSource>();
        }
        
        private void SubscribeControllerEvents()
        {
            _movementController.OnJumped += OnJumpedHandler;
            _movementController.OnLanded += OnLandedHandler;
        }

        private void UnsubscribeControllerEvents()
        {
            _movementController.OnJumped -= OnJumpedHandler;
            _movementController.OnLanded -= OnLandedHandler;
        }
        
        private void OnDestroy()
        {
            UnsubscribeControllerEvents();
        }

        private void OnJumpedHandler()
        {
            _audioSource.PlayOneShot(_jumpClip);
        }
        
        private void OnLandedHandler()
        {
            _audioSource.PlayOneShot(_landClip);
        }
    }
}
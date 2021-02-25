using UnityEngine;

namespace FPSProject.Impl.Controllers
{
    public class BulletAudioController : MonoBehaviour
    {
        [Tooltip("Clip to play on impact")] public AudioClip impactSFXClip;

        private void OnHit()
        {
            // impact sfx
            if (impactSFXClip)
            {
                //AudioUtility.CreateSFX(impactSFXClip, point, AudioUtility.AudioGroups.Impact, 1f, 3f);
            }
        }
    }
}
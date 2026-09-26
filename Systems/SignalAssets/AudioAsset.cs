using UnityEngine;

namespace Utility {
    [CreateAssetMenu(fileName = "AudioAsset", menuName = "DoctorRon/Unity Utils/Audio Asset")]
    public class AudioAsset : SignalAsset {
        public AudioClip[] clips;
        public float volume = 1f;
        public Vector2 pitch = new Vector2(1f, 1f);
    }
}

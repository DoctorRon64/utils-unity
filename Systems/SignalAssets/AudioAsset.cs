using UnityEngine;

namespace Utility {
    [CreateAssetMenu(menuName = FileStructure.SignalFolder + nameof(AudioAsset))]
    public class AudioAsset : SignalAsset, ISignal {
        public AudioClip[] clips;
        public float volume = 1f;
        public Vector2 pitch = new Vector2(1f, 1f);
    }
}

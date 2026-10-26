using UnityEngine;

namespace Utility {
    [CreateAssetMenu(menuName = FileStructure.SignalFolder + nameof(FloatSignalAsset))]
    public class FloatSignalAsset : SignalAsset<float>, ISignal<float> { }
}

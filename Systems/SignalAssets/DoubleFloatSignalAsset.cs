using UnityEngine;

namespace Utility {
    [CreateAssetMenu(menuName = FileStructure.SignalFolder + nameof(DoubleFloatSignalAsset))]
    public class DoubleFloatSignalAsset : DoubleDataSignalAsset<float, float>, ISignal<float, float> { }
}
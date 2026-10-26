using UnityEngine;

namespace Utility {
    [CreateAssetMenu(menuName = FileStructure.SignalFolder + nameof(DoubleIntSignalAsset))]
    public class DoubleIntSignalAsset : DoubleDataSignalAsset<int, int>, ISignal<int, int> { }
}

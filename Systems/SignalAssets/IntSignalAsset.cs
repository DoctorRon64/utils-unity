using UnityEngine;

namespace Utility {
    [CreateAssetMenu(menuName = FileStructure.SignalFolder + nameof(IntSignalAsset))]
    public class IntSignalAsset : SignalAsset<int>, ISignal<int> { }
}

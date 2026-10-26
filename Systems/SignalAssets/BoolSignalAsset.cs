using UnityEngine;

namespace Utility {
    [CreateAssetMenu(menuName = FileStructure.SignalFolder + nameof(BoolSignalAsset))]
    public class BoolSignalAsset : SignalAsset<bool>, ISignal<bool> { }
}

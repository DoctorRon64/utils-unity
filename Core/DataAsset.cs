using UnityEngine;

namespace Utility {
    public class DataAsset : ScriptableObject {
        [SerializeField] private string m_id;

        public string guid => m_id;
    }
}

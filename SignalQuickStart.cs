using UnityEngine;
using Utility;

namespace DoctorRon.UnityUtils.Samples.Signals {
    public sealed class SignalQuickStart : MonoBehaviour {
        [SerializeField] private MonoSignalAsset onClicked;

        private readonly Observer observer = new();

        private void OnEnable() {
            observer.Add(onClicked, HandleClicked);
        }

        private void OnDisable() {
            observer.Clear();
        }

        private void Update() {
            if (Input.GetMouseButtonDown(0)) {
                onClicked?.Invoke();
            }
        }

        private void HandleClicked() {
            Debug.Log("Signal received.");
        }
    }
}

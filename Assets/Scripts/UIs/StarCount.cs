using TMPro;
using UnityEngine;

namespace UIs {
    public class StarCount : MonoBehaviour {
        public TextMeshProUGUI starCountText;

        protected void Update() {
            starCountText.text = $"{Server.Server.Instance.StarCount}";
        }
    }
}
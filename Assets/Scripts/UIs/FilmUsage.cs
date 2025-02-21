using System;
using TMPro;
using UnityEngine;

namespace UIs {
    public class FilmUsage : MonoBehaviour {
        public TextMeshProUGUI filmUsageText;

        protected void Update() {
            filmUsageText.text = $"{Server.Server.Instance.FilmUsed} / {Server.Server.Instance.FilmCount}";
        }
    }
}
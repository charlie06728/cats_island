using System;
using TMPro;
using UnityEngine;

namespace UIs {
    public class FilmUsage : MonoBehaviour {
        public TextMeshProUGUI filmUsageText;

        protected void Update() {
            int filmRemaining = Server.Server.Instance.FilmCount - Server.Server.Instance.FilmUsed;
            filmUsageText.SetText(filmRemaining.ToString());
            if (filmRemaining < 3) {
                filmUsageText.color = Color.red;
            } else {
                filmUsageText.color = Color.black;
            }
        }
    }
}
using System;
using Player;
using TMPro;
using UnityEngine;

namespace UIs {
    public class CatsDiscovered : MonoBehaviour {
        public TextMeshProUGUI catsDiscoveredText;

        protected void Update() {
            catsDiscoveredText.SetText(
                Brochure.CollectedCats.Count.ToString() + "/" + Server.Server.Instance.CatDictionary.Count);
        }
    }
}
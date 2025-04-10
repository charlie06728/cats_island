using System;
using UnityEngine;

namespace UIs {
    public class GameEnds : MonoBehaviour {
        public void Update() {
            if (gameObject.activeInHierarchy && Input.anyKeyDown) {
                Server.Server.Instance.playerScript.Pocket.sfx_select.Post(Server.Server.Instance.playerScript.Pocket
                    .gameObject);
                gameObject.SetActive(false);
            }
        }
    }
}
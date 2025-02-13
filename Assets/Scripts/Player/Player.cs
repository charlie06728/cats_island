using System;
using UnityEngine;

namespace Player {
    public class Player : MonoBehaviour {
        public PlayerPocket Pocket;
        public GameObject Head;
        public GameObject Hand;
        public GameObject ItemHook;

        protected void Awake() {
            /* Make sure the components are set */
            Pocket = gameObject.GetComponent<PlayerPocket>();
            if (Pocket == null) Pocket = gameObject.AddComponent<PlayerPocket>();
            if (ItemHook == null)
                ItemHook = gameObject.transform.Find("Hand").gameObject.transform.Find("ItemHook").gameObject;
            if (Head == null) Head = gameObject.transform.Find("Head").gameObject;
            
            /* Throw error if cannot find item hook and head */
            if (ItemHook == null) throw new Exception("Cannot find item hook in player prefab!");
            if (Head == null) throw new Exception("Cannot find head in player prefab!");

            Pocket.Player = this;
        }
    }
}
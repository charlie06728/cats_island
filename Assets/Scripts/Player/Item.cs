using System;
using UnityEngine;

namespace Player {
    public class Item : MonoBehaviour {
        public Animator Animator;
        public GameObject hook;
        [NonSerialized] public PlayerPocket PlayerPocket;

        /* taking this item out of pocket, trigger animation */
        public virtual void TakeOut() {
            /* put all back first */
            PutBackAll();
            
            /* Set item visible */
            gameObject.SetActive(true);
            
            /* Make sure the parent is hand */
            transform.SetParent(PlayerPocket.Player.ItemHook.transform);
            
            /* Put it into proper position */
            FixItemOnHook();
        }
        
        /* When putting this item back to pocket */
        public virtual void PutBack() {
            /* make item invisible */
            gameObject.SetActive(false);
        }

        protected void PutBackAll() {
            // Player.Album.PutBack();
            PlayerPocket.CameraObject.PutBack();
            // Player.Treat.PutBack();
        }

        protected void FixItemOnHook() {
            /* Calculate the delta of hook movement */
            Vector3 delta = PlayerPocket.Player.ItemHook.transform.position - hook.transform.position;
            
            /* Set the position of item to be the edge of player hand */
            hook.transform.position = PlayerPocket.Player.ItemHook.transform.position;
            transform.position += delta;
            transform.rotation = PlayerPocket.Player.Head.transform.rotation;
        }

        protected void Update() {
            // FixItemOnHook();
        }

        protected void Awake() {
            /* Throw error if hook is not set */
            if (hook == null) {
                throw new Exception("Hook is not set for item!");
                hook = gameObject;
            }
        }
    }
}
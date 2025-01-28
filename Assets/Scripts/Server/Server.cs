using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Server {
    public class Server : MonoBehaviour {
        /* Game speed, set int editor */
        public float GameSpeed = 1f;
        public float MouseSensitivity = 1f;
        public float MoveSpeed = 1f;
        public float JumpForce = 1f;
        
        /* cat related fields */
        public float CatRandomMoveInterval = 1.5f;
        public float CatMoveSpeed = 10f;
        
        /* Input Action Map */
        public InputActionMap InputActionMap;
        
        /* Singleton pattern */
        public static Server Instance { get; private set; }
        
        protected void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
            
            /* Enable inputAction */
            InputActionMap.Enable();
        }

        protected void Update() {
            Clock.Update();
        }
    }
}
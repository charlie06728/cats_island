using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Server {
    public class Server : MonoBehaviour {
        /* Game speed, set int editor */
        public float GameSpeed = 1f;
        public float MouseSensitivity = 1f;
        public float MoveSpeed = 1f;
        
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
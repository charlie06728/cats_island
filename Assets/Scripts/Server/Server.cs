using System;
using System.Collections.Generic;
using UIs;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Server {
    public class Server : MonoBehaviour {
        /* Game speed, set int editor */
        public float GameSpeed = 1f;
        public float MouseSensitivity = 1f;
        public float ControllerSenstivity = 1f;
        public float MoveSpeed = 1f;
        public float JumpForce = 1f;
        
        /* cat related fields */
        public float CatRandomMoveInterval = 1.5f;
        public float CatMoveSpeed = 10f;
        
        /* Photo related fields */
        public int photoWidth = 1920;
        public int photoHeight = 1080;
        
        /* Input Action Map */
        public InputActionMap InputActionMap;
        
        /* Singleton pattern */
        public static Server Instance { get; private set; }
        
        /* UI */
        public Canvas canvas;
        public Scrollbar zoomScroll;
        public GameObject cameraMode;
        
        /* Cats */
        [NonSerialized] public List<Cat.Cat> Cats = new List<Cat.Cat>();
        [NonSerialized] public Dictionary<string, Cat.Cat> CatDictionary = new Dictionary<string, Cat.Cat>();
        
        /* Film */
        [NonSerialized] public int FilmCount = 18;
        [NonSerialized] public int FilmUsed = 0;
        public FilmUsage filmUsage;
        
        /* Star Count */
        [NonSerialized] public int StarCount = 0;
        
        protected void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
            
            /* Enable inputAction */
            InputActionMap.Enable();
            
            cameraMode.gameObject.SetActive(false);
            
            Cursor.visible = false;
        }

        protected void Update() {
            Clock.Update();
        }
    }
}
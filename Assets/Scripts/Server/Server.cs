using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
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
        
        public LayerMask GroundLayer;

        public float cameraCoolDown;

        [NonSerialized] public bool SuspendPlayerMove = false;
        
        /* cat related fields */
        public float CatRandomMoveInterval = 1.5f;
        public float CatMoveSpeed = 10f;
        
        /* Treat related */
        public TextMeshProUGUI treatCountText;
        
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
        public ItemBar itemBar;
        public Canvas pausemenu;
        
        /* Cats */
        [NonSerialized] public List<Cat.Cat> Cats = new List<Cat.Cat>();
        [NonSerialized] public Dictionary<string, Cat.Cat> CatDictionary = new Dictionary<string, Cat.Cat>();
        public GameObject newCatNotification;
        public GameObject albumNotification;
        
        /* Film */
        [NonSerialized] public int FilmCount = 18;
        [NonSerialized] public int FilmUsed = 0;
        public FilmUsage filmUsage;
        
        /* Star Count */
        [NonSerialized] public int StarCount = 0;
        
        /* Player obj */
        public GameObject player;
        public Player.Player playerScript;
        
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

            StartCoroutine(ColliderHandling());
        }

        protected void Update() {
            Clock.Update();
        }

        protected IEnumerator ColliderHandling() {
            /* wait for 0.5 seconds */
            yield return new WaitForSeconds(0.5f);
            
            string[] bothColliderAndRb = new string[] {"Bowl", "Toy", "Littlelight"};
            string[] colliderOnly = new string[] {"Rock"};
            /* Iterate through all game objects in the scene, add mesh collider to the object with name that contains letter "Bowl" */
            foreach (GameObject go in UnityEngine.Object.FindObjectsOfType<GameObject>()) {
                bool added = false;
                foreach (string n in bothColliderAndRb) {
                    if (go.name.Contains(n)) {
                        /* Set the object layer to terrain */
                        go.layer = LayerMask.NameToLayer("Toy");
                        
                        go.AddComponent<BoxCollider>();
                        BoxCollider mc = go.GetComponent<BoxCollider>();
                        mc.includeLayers = GroundLayer;
                        
                        /* Set include layers to every layer */
                        Rigidbody rb = go.AddComponent<Rigidbody>();
                        rb = go.GetComponent<Rigidbody>();
                        rb.isKinematic = false;
                        added = true;
                        break;
                    }
                }
                
                if (!added) {
                    foreach (string n in colliderOnly) {
                        if (go.name.Contains(n)) {
                            /* Set the object layer to terrain */
                            go.layer = LayerMask.NameToLayer("Toy");
                            
                            go.AddComponent<MeshCollider>();
                            break;
                        }
                    }
                }
            }
        }
    }
}
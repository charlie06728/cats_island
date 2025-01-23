using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Player {
    /* Define player movement, player should be able to move abound and jump */
    public class PlayerMovement : MonoBehaviour {
        /* Player camera */
        [NonSerialized] public UnityEngine.Camera PlayerCamera;
        public Rigidbody Rigidbody;
        public GameObject Head;
        
        
        /* camera rotation related */
        private Vector2 _look;
        private Vector2 _currentRotation;
        private Vector2 _rotationVelocity;
        
        /*  */
        private Vector3 MoveDirection = new Vector3();
        private Dictionary<string, bool> _keyDown = new Dictionary<string, bool>();

        public void Awake() {
            /* Make head in the same direction as camera */
            PlayerCamera = UnityEngine.Camera.main;
            PlayerCamera.transform.parent = Head.transform;
            
            /* Camera rotation behaviour */
            Server.Server.Instance.InputActionMap["MouseMove"].performed += context => {
                _look = context.ReadValue<Vector2>();
            };
            Server.Server.Instance.InputActionMap["MouseMove"].canceled += context => {
                _look = Vector2.zero;
            };
            
            /* Move */
            Server.Server.Instance.InputActionMap["W"].performed += context => { _keyDown["W"] = true; };
            Server.Server.Instance.InputActionMap["W"].canceled += context => { _keyDown["W"] = false; };
            Server.Server.Instance.InputActionMap["S"].performed += context => { _keyDown["S"] = true; };
            Server.Server.Instance.InputActionMap["S"].canceled += context => { _keyDown["S"] = false; };
            Server.Server.Instance.InputActionMap["A"].performed += context => { _keyDown["A"] = true; };
            Server.Server.Instance.InputActionMap["A"].canceled += context => { _keyDown["A"] = false; };
            Server.Server.Instance.InputActionMap["D"].performed += context => { _keyDown["D"] = true; };
            Server.Server.Instance.InputActionMap["D"].canceled += context => { _keyDown["D"] = false; };
        }

        private void Update() {
            /* Make sure the body is not tilted */
            transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
            
            /* Rotate camera */
            Vector2 delta = _look * Time.deltaTime * Server.Server.Instance.MouseSensitivity;
            Vector2 targetRotation = new Vector2(-delta.y, delta.x);
            
            /* Limit and angles of rotation */
            Vector3 rotated = new Vector3(Head.transform.localEulerAngles.x + targetRotation.x,
                transform.localEulerAngles.y + targetRotation.y, 0f);

            transform.localEulerAngles = new Vector3(0f, rotated.y, 0f);
            Head.transform.localEulerAngles = new Vector3(rotated.x, 0f, 0f);
            
            /* Move player */
            MoveDirection = new Vector3();
            foreach (string k in _keyDown.Keys) {
                if (!_keyDown[k]) continue;
                switch (k) {
                    case "W":
                        MoveDirection += transform.forward.normalized;
                        break;
                    case "S":
                        MoveDirection -= transform.forward.normalized;
                        break;
                    case "A":
                        MoveDirection -= transform.right.normalized;
                        break;
                    case "D":
                        MoveDirection += transform.right.normalized;
                        break;
                }
            }
            transform.position += MoveDirection.normalized * Time.deltaTime * Server.Server.Instance.MoveSpeed;
        }
    }
}
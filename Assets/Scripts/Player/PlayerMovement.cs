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
        private bool _isGrounded = true;
        private float _upVelocity;
        private float _upAcceleration;
        private float _playerHeight = 2.5f;
        
        /*  */
        private Vector3 MoveDirection = new Vector3();
        private Dictionary<string, bool> _keyDown = new Dictionary<string, bool>();

        public void Awake() {
            /* Make head in the same direction as camera */
            PlayerCamera = UnityEngine.Camera.main;
            PlayerCamera.transform.parent = Head.transform;
            
            /* Get rigidbody */
            Rigidbody = GetComponent<Rigidbody>();
            if (Rigidbody == null) Rigidbody = gameObject.AddComponent<Rigidbody>();
            
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
            Server.Server.Instance.InputActionMap["GamePadLeft"].performed += context => { _keyDown["GamePadLeft"] = true; };
            Server.Server.Instance.InputActionMap["GamePadLeft"].canceled += context => { _keyDown["GamePadLeft"] = false; };
            Server.Server.Instance.InputActionMap["Space"].performed += context => {
                /* Check if body is very close to the terrain */
                if (Physics.Raycast(transform.position, Vector3.down, 5f)) {
                    Rigidbody.AddForce(Vector3.up * Server.Server.Instance.JumpForce * Rigidbody.mass, ForceMode.Impulse);
                    // _upAcceleration = Physics.gravity.y;
                    // Rigidbody.useGravity = true;
                    Debug.Log("Jumping");
                }
            };
        }

        private void Update() {
            if (_keyDown.ContainsKey("GamePadLeft") && _keyDown["GamePadLeft"]) MoveByGamePad();
            
            Rigidbody.angularVelocity = new Vector3();
            bool keyDown = false;
            foreach (string k in _keyDown.Keys) {
                if (_keyDown[k]) {
                    keyDown = true;
                    Rigidbody.isKinematic = false;
                    break;
                }
            }
            
            /* Make sure the body is not tilted */
            transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, 0f);
            
            // /* Check if body is very close to the terrain */
            // if (IsOnGround()) {
            //     if (Rigidbody.linearVelocity.y < 0) Rigidbody.useGravity = false;
            //     _isGrounded = true;
            //     _upAcceleration = 0;
            //     _upVelocity = 0;
            //     Rigidbody.linearVelocity = new Vector3();
            //     
            //     /* Set player height to terrain sample height + player physical height */
            //     transform.position = new Vector3(transform.position.x,
            //         Terrain.TerrainManager.Instance.Terrain.SampleHeight(transform.position) + _playerHeight,
            //         transform.position.z);
            // } else {
            //     _isGrounded = false;
            // }
            
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
            
            // if (!_isGrounded) {
            //     _upVelocity += _upAcceleration * Time.deltaTime;
            //     transform.position += new Vector3(0, _upVelocity * Time.deltaTime, 0);
            //     _upAcceleration += Physics.gravity.y * Time.deltaTime * 2;
            //     
            //     transform.position += MoveDirection.normalized * Time.deltaTime * Server.Server.Instance.MoveSpeed;
            // } else {
            //     /* Precalculate the destination coordinate */
            //     Vector3 destination = transform.position +
            //                           MoveDirection.normalized * Time.deltaTime * Server.Server.Instance.MoveSpeed;
            //     
            //     /* Calculate the changes in terrain height */
            //     float heightDelta = Terrain.TerrainManager.Instance.Terrain.SampleHeight(destination) -
            //                         Terrain.TerrainManager.Instance.Terrain.SampleHeight(transform.position);
            //     /* Adjust the destination height */
            //     destination.y += heightDelta;
            //     
            //     /* ray cast again  */
            //     if (!IsOnGround())
            //         destination.y = Terrain.TerrainManager.Instance.Terrain.SampleHeight(destination) + _playerHeight;
            //     
            //     /* Move the player */
            //     transform.position = destination;
            // }
            

            // if (_isGrounded) {
            //     if (IsOnSlope()) {
            //         Rigidbody.velocity = new Vector3(Rigidbody.velocity.x,
            //             (Physics.gravity * 0.0f * (float)Time.deltaTime).y, Rigidbody.velocity.z);
            //     }
            // }
            
            // _velocity += MoveDirection.normalized * Time.deltaTime * Server.Server.Instance.MoveSpeed;
            
            // transform.position += _velocity;
            // Rigidbody.linearVelocity += MoveDirection.normalized * Server.Server.Instance.MoveSpeed;
            
            // transform.position += MoveDirection.normalized * Time.deltaTime * Server.Server.Instance.MoveSpeed;
            
            /* Reset the velocity except vertical */
            // _velocity = new Vector3(0, _velocity.y, 0);
        }

        protected void MoveByGamePad() {
            Vector2 gamePad = Server.Server.Instance.InputActionMap["GamePadLeft"].ReadValue<Vector2>();
            if (gamePad.x != 0) {
                Debug.Log("Gamepad x: " + gamePad.x);;
            }
            
            /* Move the player by gamePad */
            Vector3 move = new Vector3() + gamePad.x * transform.right + gamePad.y * transform.forward;
            transform.position += move * Time.deltaTime * Server.Server.Instance.MoveSpeed;
        }
        
        protected bool IsOnSlope()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 1.2f))
            {
                float angle = Vector3.Angle(hit.normal, Vector3.up);
                return angle > 0 && angle < 70; // Check if within slope range
            }
            return false;
        }

        protected bool IsOnGround() {
            /* Check if body is very close to the terrain */
            if (Physics.Raycast(transform.position, Vector3.down, _playerHeight)) {
                return true;
            }
            else {
                return false;
            }
        }

    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using DefaultNamespace.Sound;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Player {
    /* Define player movement, player should be able to move abound and jump */
    public class PlayerMovement : MonoBehaviour {
        /* foots step sound */
        public AudioSource footStepSound;
        
        /* Player camera */
        [NonSerialized] public UnityEngine.Camera PlayerCamera;
        public Rigidbody Rigidbody;
        public GameObject Head;
        public LayerMask groundLayer;

        /* Sprinting and crouching */
        public float sprintMultiplier = 1.5f; // How much faster should sprinting make you?
        public float crouchMultiplier = 0.5f; // How much slower should sprinting make you?
        public float crouchDist = 1f; // How far down should we crouch?
        public float crouchSpeed = 0.5f; // How fast should smooth crouching go?
        private Vector3 crouchTarget;
        bool isSprinting;
        bool isCrouching;
        
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

        public GameObject pauseMenu;


        /* Recieves messages from the PlayerInput component on the player when the player presses/releases shift/ctrl */
        // public void OnSprint(InputValue val){
        //     if(val.isPressed){
        //         // Debug.Log("now sprinting!");
        //         isSprinting = true;
        //     } else {
        //         // Debug.Log("no longer sprinting!");
        //         isSprinting = false;
        //     }
        // }

        // public void OnCrouch(InputValue val){
        //     if(val.isPressed){
        //         // Debug.Log("now crouching!");
        //         crouchTarget = new Vector3(0, crouchDist, 0);
        //         isCrouching = true;
        //     } else {
        //         // Debug.Log("no longer crouching!");
        //         crouchTarget = new Vector3(0, 0, 0);
        //         isCrouching = false;
        //     }
        // }

        public void Awake() {
            /* Make head in the same direction as camera */
            crouchTarget = new Vector3(0, 0, 0);

            PlayerCamera = UnityEngine.Camera.main;
            PlayerCamera.transform.parent = Head.transform;
            
            /* Get rigidbody */
            // Rigidbody = GetComponent<Rigidbody>();
            // if (Rigidbody == null) Rigidbody = gameObject.AddComponent<Rigidbody>();
            // Rigidbody.useGravity = true;
            
            /* Camera rotation behaviour */
            Server.Server.Instance.InputActionMap["MouseMove"].performed += context => {
                if (context.control.device is Gamepad) {
                    _look = context.ReadValue<Vector2>() * Server.Server.Instance.ControllerSenstivity;
                } else {
                    _look = context.ReadValue<Vector2>() * Server.Server.Instance.MouseSensitivity;
                }
                // _look = context.ReadValue<Vector2>();
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
            Server.Server.Instance.InputActionMap["Sprint"].performed += context => { isSprinting = true; };
            Server.Server.Instance.InputActionMap["Sprint"].canceled += context => { isSprinting = false; };
            Server.Server.Instance.InputActionMap["Crouch"].performed += context => {
                crouchTarget = new Vector3(0, -crouchDist, 0);
                isCrouching = true; 
            };
            Server.Server.Instance.InputActionMap["Crouch"].canceled += context => {
                crouchTarget = new Vector3(0, 0, 0);
                isCrouching = false; 
            };
            // Server.Server.Instance.InputActionMap["CrouchToggle"].performed += context => {
            //     if (isCrouching) {
            //         Head.transform.position += new Vector3(0, crouchDist, 0);
            //         isCrouching = false;
            //     } else {
            //         Head.transform.position -= new Vector3(0, crouchDist, 0);
            //         isCrouching = true;
            //     }
            // };
            Server.Server.Instance.InputActionMap["GamePadLeft"].performed += context => { _keyDown["GamePadLeft"] = true; };
            Server.Server.Instance.InputActionMap["GamePadLeft"].canceled += context => { _keyDown["GamePadLeft"] = false; };
            Server.Server.Instance.InputActionMap["Space"].performed += context => {
                /* Check if body is very close to the terrain */
                if (_isGrounded) {
                    Rigidbody.AddForce(Vector3.up * Server.Server.Instance.JumpForce * Rigidbody.mass, ForceMode.Impulse);
                    // _upAcceleration = Physics.gravity.y;
                    // Rigidbody.useGravity = true;
                    Debug.Log("Jumping");
                    // AkUnitySoundEngine.SetSwitch("sfx_Jump", "grass", gameObject);
                    // AkUnitySoundEngine.PostEvent("sfx_Jump", gameObject);
                }
            };
            Server.Server.Instance.InputActionMap["Pause"].performed += context => {
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
            };
        }
        
        private void OnCollisionStay(Collision collision)
        {
            if ((groundLayer.value & (1 << collision.gameObject.layer)) > 0) {
                _isGrounded = true;
            }
        }
        
        private void OnCollisionExit(Collision collision)
        {
            if ((groundLayer.value & (1 << collision.gameObject.layer)) > 0)
            {
                _isGrounded = false;
            }
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if ((groundLayer.value & (1 << collision.gameObject.layer)) > 0) // Check if touching terrain
            {
                _isGrounded = true;
            }
        }


        private void FixedUpdate() {
            Head.transform.localPosition = Vector3.Lerp(Head.transform.localPosition, crouchTarget, crouchSpeed * Time.deltaTime);
            Vector3 MoveValue = new Vector3();
            if (_keyDown.ContainsKey("GamePadLeft") && _keyDown["GamePadLeft"]) MoveValue += MoveByGamePad();
            
            // Rigidbody.angularVelocity = new Vector3();
            bool keyDown = false;
            foreach (string k in _keyDown.Keys) {
                if (_keyDown[k]) {
                    keyDown = true;
                    Rigidbody.isKinematic = false;
                    break;
                }
            }

            if (keyDown) {
                // if (!footStepSound.isPlaying) footStepSound.Play();
                FootStepManager.Instance.PlayFootstep();
            } else {
                // footStepSound.Stop();
                FootStepManager.Instance.StopFootstep();
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
            Vector2 delta = _look * Time.deltaTime;
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
            MoveValue += MoveDirection * Time.deltaTime * Server.Server.Instance.MoveSpeed;

            if (isSprinting) {
                MoveValue *= sprintMultiplier;
            } else if (isCrouching) { // Holding sprint overrides the effects of holding crouch
                MoveValue *= crouchMultiplier;
            }
            
            // if (!Server.Server.Instance.SuspendPlayerMove) transform.position += MoveValue;
            if (!Server.Server.Instance.SuspendPlayerMove) Rigidbody.linearVelocity = new Vector3(MoveValue.x, Rigidbody.linearVelocity.y, MoveValue.z);

            // Vector3 movementInput = MoveValue;
            // if (movementInput.magnitude > 0)
            // {
            //     // Target velocity without affecting gravity
            //     Vector3 targetVelocity = new Vector3(movementInput.x, Rigidbody.linearVelocity.y, movementInput.z);
            //
            //     // Apply velocity gradually (smooth acceleration)
            //     Rigidbody.linearVelocity = Vector3.Lerp(Rigidbody.linearVelocity, targetVelocity, Time.fixedDeltaTime);
            // }
            // else
            // {
            //     // Gradually reduce velocity when no input (smooth deceleration)
            //     Rigidbody.linearVelocity = Vector3.Lerp(Rigidbody.linearVelocity, new Vector3(0, Rigidbody.linearVelocity.y, 0), 0);
            // }
        }
        

        protected Vector3 MoveByGamePad() {
            Vector2 gamePad = Server.Server.Instance.InputActionMap["GamePadLeft"].ReadValue<Vector2>();
            if (gamePad.x != 0) {
                Debug.Log("Gamepad x: " + gamePad.x);;
            }

            if (gamePad.x != 0 || gamePad.y != 0) {
                if (!footStepSound.isPlaying) footStepSound.Play();
            } else {
                footStepSound.Stop();
            }
            
            /* Move the player by gamePad */
            Vector3 move = new Vector3() + gamePad.x * transform.right + gamePad.y * transform.forward;
            return move * Time.deltaTime * Server.Server.Instance.MoveSpeed;
            // transform.position += move * Time.deltaTime * Server.Server.Instance.MoveSpeed;
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
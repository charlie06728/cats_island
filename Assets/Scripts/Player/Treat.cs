using System;
using UnityEngine;
using Cat;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player {
    public class Treat : Item {
        public AK.Wwise.Event TreatLaunch;
        public GameObject treatPrefab;
        public float treat_distance; // Distance at which we should be able to give a treat
        [NonSerialized] public int Count = 10;

        // Camera mainCamera;

        private Action<InputAction.CallbackContext> _giveTreatAction;

        public override void TakeOut() {
            /* Set item visible */
            gameObject.SetActive(true);
            
            /* Make sure the parent is hand */
            transform.SetParent(PlayerPocket.Player.ItemHook.transform);
            
            /* Put it into proper position */
            FixItemOnHook();
            FixItemOnHook();
            
            /* define the input action behaviours */
            Server.Server.Instance.InputActionMap["LeftMouse"].performed += _giveTreatAction;
        }
        
        public override void PutBack() {
            base.PutBack();
            
            /* define the input action behaviours */
            Server.Server.Instance.InputActionMap["LeftMouse"].performed -= _giveTreatAction;
        }

        public bool GiveTreat() {
            if (Count <= 0) return false;
            TreatLaunch.Post(gameObject);
            Count--;
            RaycastHit hit;
            
            /* Instantiate the treat prefab and drop to the ground */
            GameObject treat = Instantiate(treatPrefab, transform.position, Quaternion.identity);
            /* Get the rigidbody of spawned */
            Rigidbody rb = treat.GetComponent<Rigidbody>();
            
            /* Add a force to the forward direction of player */
            rb.AddForce(PlayerPocket.Player.Head.transform.forward * 5, ForceMode.Impulse);
            
            // Send out a ray in the direction the camera is facing
            // Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, treat_distance));
            // if (Physics.Raycast(ray, out hit, treat_distance)) {   
            //     if (hit.transform.gameObject.GetComponent<CatBehaviour>() != null) {
            //         CatBehaviour behaviour = hit.transform.gameObject.GetComponent<CatBehaviour>(); 
            //         behaviour.SwitchState(CatState.Pose);
            //         Debug.Log("Given Treat!");
            //         return true;
            //     }
            // }
            // return false;

            return true;
        }

        protected override void Awake() {
            base.Awake();
            _giveTreatAction = ctx => GiveTreat();
            // mainCamera = mainCamera.main();
        }

        protected override void Update() {
            base.Update();
            if (gameObject.activeInHierarchy) {
                Server.Server.Instance.treatCountText.SetText(Count.ToString());
                
                /* Set it to red if < 3 */
                if (Count < 3) {
                    Server.Server.Instance.treatCountText.color = Color.red;
                } else {
                    Server.Server.Instance.treatCountText.color = Color.black;
                }
            }
        }
    }
}
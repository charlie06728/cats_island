using System;
using UnityEngine;
using Cat;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player {
    public class Treat : Item {
        public float treat_distance; // Distance at which we should be able to place a treat
        public float cat_trigger_distance; // Distance at which we trigger cats to enter treat mode
        public GameObject treatPrefab;
        CatBehaviour[] cats;
        // Camera mainCamera;

        private Action<InputAction.CallbackContext> _giveTreatAction;

        public override void TakeOut() {
            base.TakeOut();
            Server.Server.Instance.itemBar.SetCurrentItem(Items.Cookie);

            cats = FindObjectsByType<CatBehaviour>(FindObjectsSortMode.None);

            
            FixItemOnHook();
            
            /* define the input action behaviours */
            Server.Server.Instance.InputActionMap["LeftMouse"].performed += _giveTreatAction;
        }
        
        public override void PutBack() {
            base.PutBack();
            
            /* define the input action behaviours */
            Server.Server.Instance.InputActionMap["LeftMouse"].performed -= _giveTreatAction;
        }

        public void GiveTreat() {
            // RaycastHit hit;
            // // Send out a ray in the direction the camera is facing
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



            RaycastHit hit;
            
            /* Instantiate the treat prefab and drop to the ground */
            GameObject treat = Instantiate(treatPrefab, transform.position, Quaternion.identity);
            /* Get the rigidbody of spawned */
            Rigidbody rb = treat.GetComponent<Rigidbody>();
            
            /* Add a force to the forward direction of player */
            rb.AddForce(PlayerPocket.Player.Head.transform.forward * 5, ForceMode.Impulse);
            
            // Send out a ray in the direction the camera is facing
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, treat_distance));
            if (Physics.Raycast(ray, out hit, treat_distance)) {   
                Instantiate(treatPrefab, hit.point, new Quaternion(0,0,0,0));

                Vector3 currentPos = gameObject.transform.position;

                // All nearby cats move to nearest treat
                foreach (CatBehaviour c in cats)
                {   
                    Transform t = c.gameObject.transform;
                    float dist = Vector3.Distance(t.position, currentPos);
                    if (dist < cat_trigger_distance)
                    {
                        c.SwitchState(CatState.Treat);
                    }
                }
            }
        }

        protected override void Awake() {
            base.Awake();
            _giveTreatAction = ctx => GiveTreat();
            // mainCamera = mainCamera.main();
        }
    }
}
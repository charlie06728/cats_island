using System;
using UnityEngine;
using Cat;
using TMPro;
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
//             base.TakeOut();
//             Server.Server.Instance.itemBar.SetCurrentItem(Items.Cookie);

           
            /* Set item visible */
            cats = FindObjectsByType<CatBehaviour>(FindObjectsSortMode.None);
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

        public void GiveTreat() {
            RaycastHit hit;
            
            // Send out a ray in the direction the camera is facing
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, treat_distance));
            if (Physics.Raycast(ray, out hit, treat_distance)) {   
                // Create a treat on that surface
                Instantiate(treatPrefab, new Vector3(hit.point.x, hit.point.y + 0.5f, hit.point.z), new Quaternion(0,0,0,0));

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

        void Start() {
            cats = FindObjectsOfType<CatBehaviour>();
        }

        protected override void Awake() {
            base.Awake();
            _giveTreatAction = ctx => GiveTreat();
        }

        protected override void Update() {
            base.Update();
            if (gameObject.activeInHierarchy) {
                // Server.Server.Instance.treatCountText.SetText(Count.ToString());
                
                /* Set it to red if < 3 */
                // if (Count < 3) {
                //     Server.Server.Instance.treatCountText.color = Color.red;
                // } else {
                //     Server.Server.Instance.treatCountText.color = Color.black;
                // }
            }
        }
    }
}
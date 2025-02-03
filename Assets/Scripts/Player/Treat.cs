using System;
using UnityEngine;
using Cat;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player {
    public class Treat : Item {
        public float treat_distance; // Distance at which we should be able to give a treat

        // Camera mainCamera;

        private Action<InputAction.CallbackContext> _giveTreatAction;

        public override void TakeOut() {
            base.TakeOut();
            
            /* define the input action behaviours */
            Server.Server.Instance.InputActionMap["LeftMouse"].performed += _giveTreatAction;
        }
        
        public override void PutBack() {
            base.PutBack();
            
            /* define the input action behaviours */
            Server.Server.Instance.InputActionMap["LeftMouse"].performed -= _giveTreatAction;
        }

        public bool GiveTreat() {
            RaycastHit hit;
            // Send out a ray in the direction the camera is facing
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, treat_distance));
            if (Physics.Raycast(ray, out hit, treat_distance)) {   
                if (hit.transform.gameObject.GetComponent<CatBehaviour>() != null) {
                    CatBehaviour behaviour = hit.transform.gameObject.GetComponent<CatBehaviour>(); 
                    behaviour.SwitchState(CatState.Pose);
                    Debug.Log("Given Treat!");
                    return true;
                }
            }
            return false;
        }

        protected override void Awake() {
            base.Awake();
            _giveTreatAction = ctx => GiveTreat();
            // mainCamera = mainCamera.main();
        }
    }
}
using System;
using Cat;
using UnityEngine;

namespace Player {
    public class TreatInstance : MonoBehaviour {
        public Items treat;
        public Rigidbody rigidbody;
        public float evaluateInterval = 1.5f;
        public float treatWorkDistance = 15f;

        protected float prevUpdateTime = 0f;

        public bool targeted = false;

        protected void Update() {
            if (!targeted) SetTarget();
            // if (Time.time - prevUpdateTime > evaluateInterval) {
            //     foreach (Cat.Cat cat in Server.Server.Instance.Cats) {
            //         /* Check the distance of cat */
            //         float distance = Vector3.Distance(cat.transform.position, transform.position);
            //         if (distance < treatWorkDistance) {
            //             /* Check if the cat is already following the treat */
            //             if (cat.Behaviour.State != CatState.FollowTreat && cat.Behaviour.State != CatState.Eat) {
            //                 // if (!cat.Navigator.Agent.hasPath) {
            //                 /* Move the cat to the treat */
            //                 // cat.Navigator.MoveTo(transform.position);
            //                 cat.Behaviour.TargetTreat = this;
            //                 cat.Behaviour.SwitchState(CatState.FollowTreat);
            //                 break;
            //                 // }
            //             }
            //         }
            //     }
            //     
            //     prevUpdateTime = Time.time;
            // }
        }

        private void SetTarget() {
            bool catFound = false;
            foreach (Cat.Cat cat in Server.Server.Instance.Cats) {
                /* Skip if eat interval is smaller than 10s */
                if (Time.time - cat.Behaviour.prevEatTime < 10f) continue;
                
                /* Check the distance of cat */
                float distance = Vector3.Distance(cat.transform.position, transform.position);
                if (distance < treatWorkDistance) {
                    /* Check if the cat is already following the treat */
                    if (cat.Behaviour.State != CatState.FollowTreat && cat.Behaviour.State != CatState.Eat) {
                        // if (!cat.Navigator.Agent.hasPath) {
                        /* Move the cat to the treat */
                        // cat.Navigator.MoveTo(transform.position);
                        
                        /* check if the treat is cat's favourite treat */
                        if (treat != cat.favTreat) continue;
                        
                        cat.Behaviour.TargetTreat = this;
                        cat.Behaviour.SwitchState(CatState.FollowTreat);
                        catFound = true;
                        break;
                        // }
                    }
                }
            }

            if (catFound) targeted = true;
            // prevUpdateTime = Time.time;
        }
    }
}
using System;
using Cat;
using UnityEngine;

namespace Player {
    public class TreatInstance : MonoBehaviour {
        public Rigidbody rigidbody;
        public float evaluateInterval = 1.5f;
        public float treatWorkDistance = 15f;

        protected float prevUpdateTime = 0f;

        protected void Update() {
            if (Time.time - prevUpdateTime > evaluateInterval) {
                foreach (Cat.Cat cat in Server.Server.Instance.Cats) {
                    /* Check the distance of cat */
                    float distance = Vector3.Distance(cat.transform.position, transform.position);
                    if (distance < treatWorkDistance) {
                        /* Check if the cat is already following the treat */
                        if (cat.Behaviour.State != CatState.FollowTreat && cat.Behaviour.State != CatState.Eat) {
                            // if (!cat.Navigator.Agent.hasPath) {
                            /* Move the cat to the treat */
                            // cat.Navigator.MoveTo(transform.position);
                            cat.Behaviour.TargetTreat = this;
                            cat.Behaviour.SwitchState(CatState.FollowTreat);
                            break;
                            // }
                        }
                    }
                    
                }
                
                prevUpdateTime = Time.time;
            }
        }
    }
}
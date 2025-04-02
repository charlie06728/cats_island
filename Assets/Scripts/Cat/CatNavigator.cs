using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace Cat {
    public class CatNavigator : MonoBehaviour {
        public NavMeshAgent Agent;
        public Rigidbody Rigidbody;
        public BoxCollider Collider;
        public bool onBuilding = false;

        public Cat Cat;

        protected void Awake() {
            /* setup rigid body */
            Rigidbody = GetComponent<Rigidbody>();
            if (Rigidbody == null) Rigidbody = gameObject.AddComponent<Rigidbody>();
            Rigidbody.useGravity = true;
            
            /* Make sure the box collider exists */
            Collider = GetComponent<BoxCollider>();
            if (Collider == null) Collider = gameObject.AddComponent<BoxCollider>();
            
            /* Make sure the nav mesh agent exists */
            Agent = GetComponent<NavMeshAgent>();
            if (Agent == null) Agent = gameObject.AddComponent<NavMeshAgent>();
            
            /* Update fields based on server settings */
            Agent.speed = Server.Server.Instance.CatMoveSpeed;
        }

        /* Move to a specific world position */
        public void MoveTo(Vector3 position, bool run = false) {
            /* do a ray cast from sky to the gound, if the hitting object has water layer, ignore this request */
            RaycastHit hit;
            if (Physics.Raycast(position + Vector3.up * 100, Vector3.down, out hit, 900)) {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Water")) {
                    Debug.Log("Cat is trying to move to water, ignore this request");
                    return;
                }
            }
            
            /* Double the speed if run */
            Agent.speed = run ? Server.Server.Instance.CatMoveSpeed * 2.5f : Server.Server.Instance.CatMoveSpeed;
            
            /* y field of this position has to be the height of terrain */
            position.y = Terrain.TerrainManager.Instance.Terrain.SampleHeight(position);
            if (onBuilding) {
                /* Get wood layer mask */
                LayerMask woodLayer = LayerMask.GetMask("Wood");
                /* if cat is on building, reset the position's y coordinate to hitting point of wood's y */
                if (Physics.Raycast(position + Vector3.up * 100, Vector3.down, out hit, 900, woodLayer)) {
                    position.y = hit.point.y;
                }
            }
            
            /* Move agent to given location */
            Agent.SetDestination(position);
            // Debug.Log("Moving to " + position);
            
            /* Trigger cat animation */
            Cat.Behaviour.Animator.SetBool("IsWalking", true);
        }
        
        protected void Update() {
            if (Cat == null || Cat.Behaviour.Animator == null) return;
            
            /* check if the cat is moving, and set the animation correspondingly */
            if (Agent.velocity.magnitude < 0.1f) {
                Cat.Behaviour.Animator.SetBool("IsRunning", false);
                Cat.Behaviour.Animator.SetBool("IsWalking", false);
            } else {
                if (Agent.speed > Server.Server.Instance.CatMoveSpeed * 1.5f) {
                    Cat.Behaviour.Animator.SetBool("IsRunning", true);
                    Cat.Behaviour.Animator.SetBool("IsWalking", false);
                } else {
                    Cat.Behaviour.Animator.SetBool("IsRunning", false);
                    Cat.Behaviour.Animator.SetBool("IsWalking", true);
                }
            }
        }
    }
}
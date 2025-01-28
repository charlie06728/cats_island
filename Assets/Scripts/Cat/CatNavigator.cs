using System;
using UnityEngine;
using UnityEngine.AI;

namespace Cat {
    public class CatNavigator : MonoBehaviour {
        public NavMeshAgent Agent;
        public Rigidbody Rigidbody;
        public BoxCollider Collider;

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
        public void MoveTo(Vector3 position) {
            /* y field of this position has to be the height of terrain */
            position.y = Terrain.TerrainManager.Instance.Terrain.SampleHeight(position);
            
            /* Move agent to given location */
            Agent.SetDestination(position);
            Debug.Log("Moving to " + position);
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Cat {
    public class CatNavigator : MonoBehaviour {
        /* Randomly move around when set to true */
        [NonSerialized] public bool RandomMove = true;
        public NavMeshAgent Agent;
        
        /* Move to a specific world position */
        public void MoveTo(Vector3 position) {
            throw new NotImplementedException();
        }
    }
}
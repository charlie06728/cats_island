using System;
using UnityEngine;

namespace Cat {
    public class CatNavigator : MonoBehaviour {
        /* Randomly move around when set to true */
        [NonSerialized] public bool RandomMove = true;
        
        /* Move to a specific world position */
        public void MoveTo(Vector3 position) {
            throw new NotImplementedException();
        }
    }
}
using System;
using UnityEngine;

namespace Player {
    public class Item : MonoBehaviour {
        public Animator Animator;
        [NonSerialized] private PlayerPocket Player;

        /* taking this item out of pocket, trigger animation */
        public virtual void TakeOut() {
            throw new NotImplementedException();
        }
        
        /* When putting this item back to pocket */
        public virtual void PutBack() {
            throw new NotImplementedException();
        }
    }
}
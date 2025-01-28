using System;
using UnityEngine;

namespace Cat {
    public class Behaviour {
        public Cat Cat;
        public bool Enabled = false;
        public float PreviousEvaluateTime;

        public Behaviour(Cat cat) { Cat = cat;}

        public virtual void Enable() {
            Enabled = true;
            
            /* set the previous evaluate time to current time */
            PreviousEvaluateTime = Time.time;
        }
        
        public virtual void Disable() {
            Enabled = false;
        }
        
        public virtual void Update() {}
    }
}
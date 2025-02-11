using System;
using System.Collections.Generic;
using Cat.Behaviours;
using UnityEngine;

namespace Cat {

    public enum CatState {
        Idle,
        Treat,
        Pose
    }
    
    public class CatBehaviour : MonoBehaviour {
        public Animator Animator;
        public Cat Cat;
        [NonSerialized] public CatState State = CatState.Idle;
        
        /* Behaviours script */
        public Dictionary<CatState, Behaviour> StateToBehaviour = new Dictionary<CatState, Behaviour>();
        
        public void SwitchState(CatState state) {
            StateToBehaviour[State].Disable();
            State = state;
            StateToBehaviour[State].Enable();
        }
        
        protected void Awake() {
            Cat = GetComponent<Cat>();

            StateToBehaviour[CatState.Idle] = new CatIdleBehaviour(Cat);
            //TODO: change to other behaviours
            StateToBehaviour[CatState.Treat] = new CatTreatBehaviour(Cat);
            StateToBehaviour[CatState.Pose] = new CatPoseBehaviour(Cat);
            
            SwitchState(CatState.Idle);
        }


        protected void Update() {
            if (!StateToBehaviour[State].Enabled) return;
            StateToBehaviour[State].Update();
        }
    }
}
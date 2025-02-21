using System;
using Server;
using Terrain;
using UnityEngine;
using Time = UnityEngine.Time;

namespace Cat.Behaviours {
    public class CatPoseBehaviour : Behaviour {
        public CatPoseBehaviour(Cat cat) : base(cat) { }
        
        protected float prevUpdateTime = 0f;
        public float poseTime = 3.5f;

        public override void Update() {
            base.Update();
            
            if (Time.time - prevUpdateTime < poseTime) return;
            Cat.Behaviour.SwitchState(CatState.Idle);
        }

        public override void Enable() {
            base.Enable();
            Cat.Navigator.Agent.isStopped = true;
            Cat.Behaviour.Animator.SetBool("IsWondering", true);
            prevUpdateTime = Time.time;
        }

        public override void Disable() {
            Cat.Navigator.Agent.isStopped = false;
            Cat.Behaviour.Animator.SetBool("IsWondering", false);
            base.Disable();
        }
    }
}

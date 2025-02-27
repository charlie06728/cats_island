using System;
using Server;
using Terrain;
using Unity.VisualScripting;
using UnityEngine;
using Time = UnityEngine.Time;

namespace Cat.Behaviours {
    public class CatPoseBehaviour : Behaviour {
        public CatPoseBehaviour(Cat cat) : base(cat) { }
        
        protected float prevUpdateTime = 0f;
        public float sitTime = 5f;
        public float sittedTime = 7f;

        protected bool isSittingTriggered = false;
        protected bool sittingDown = true;

        public override void Update() {
            base.Update();
            
            // bool isSitting = Cat.Behaviour.Animator.GetBool("IsSitting");
            // bool isSitted = Cat.Behaviour.Animator.GetBool("IsSitted");
            float timeDelta = Time.time - prevUpdateTime;
            
            if (timeDelta < sitTime) return;
            Cat.Behaviour.Animator.SetBool("IsSitting", false);
            Cat.Behaviour.SwitchState(CatState.Idle);
            
            // /* Go to sited animation from sitting */
            // if (sittingDown && timeDelta > sitTime) {
            //     Cat.Behaviour.Animator.speed = 1f;
            //     Cat.Behaviour.Animator.SetBool("IsSitted", true);
            //     sittingDown = false;
            //     
            //     /* Update the prev time */
            //     prevUpdateTime = Time.time;
            // } else if (isSitted && timeDelta > sittedTime) {
            //     /* Go to sitting animation when seated ends */
            //     Cat.Behaviour.Animator.SetBool("IsSitted", false);
            //     Cat.Behaviour.Animator.speed = -1f;
            //     
            //     /* Update the pre update time */
            //     prevUpdateTime = Time.time;
            // } else if (!sittingDown && !isSitted && timeDelta > sitTime) {
            //     /* Go to sitting animation when seated ends */
            //     Cat.Behaviour.Animator.speed = 1f;
            //     Cat.Behaviour.Animator.SetBool("IsSitting", false);
            //     
            //     /* Switch to idle state */
            //     Cat.Behaviour.SwitchState(CatState.Idle);
            // }
            
            // /* if cat sitting is triggerred and is sitting ends, go to sitting pose */
            // if (isSittingTriggered && !isSitting) {
            //     Cat.Behaviour.Animator.SetBool("IsSitted", true);
            //     isSittingTriggered = false;
            //     prevUpdateTime = Time.time;
            //     return;
            // }
            //
            // if (Time.time - prevUpdateTime < poseTime) return;
            // Cat.Behaviour.SwitchState(CatState.Idle);
        }

        public override void Enable() {
            base.Enable();
            Cat.Navigator.Agent.isStopped = true;
            Cat.Behaviour.Animator.SetBool("IsSitting", true);
            prevUpdateTime = Time.time;
        }
        

        public override void Disable() {
            Cat.Navigator.Agent.isStopped = false;
            Cat.Behaviour.Animator.SetBool("IsSitting", false);
            base.Disable();
            
            Cat.Behaviour.Animator.Play("Default", 0, 0f);

        }
    }
}

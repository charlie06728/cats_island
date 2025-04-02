using UnityEngine;

namespace Cat.Behaviours {
    public class CatFollowTreatBehaviour : Behaviour {
        
        public CatFollowTreatBehaviour(Cat cat) : base(cat) { }
        
        protected float prevUpdateTime = 0f;

        public override void Enable() {
            base.Enable();
            Cat.Behaviour.StopOtherAnimations();
        }

        public override void Update() {
            base.Update();
            
            if (Cat.Behaviour.TargetTreat == null) {
                Cat.Behaviour.SwitchState(CatState.Idle);
                return;
            }

            if (Time.time - prevUpdateTime < 1f) return;
            prevUpdateTime = Time.time;
            
            /* Check the distance between target treat */
            float distance = Vector3.Distance(Cat.Behaviour.TargetTreat.transform.position, Cat.transform.position);
            if (distance < 1.5f) {
                /* Eat treat */
                // GameObject.Destroy(Cat.Behaviour.TargetTreat.gameObject);
                Cat.Behaviour.SwitchState(CatState.Eat);
            }
            
            /* Move the cat towards to target treat */
            Cat.Navigator.MoveTo(Cat.Behaviour.TargetTreat.transform.position);
        }
    }
}
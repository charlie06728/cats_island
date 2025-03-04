using Server;
using Time = UnityEngine.Time;

namespace Cat.Behaviours {
    public class CatPoseBehaviour : Behaviour {
        public CatPoseBehaviour(Cat cat) : base(cat) { }
        
        public float PoseTime = 5f;
        protected float prevUpdateTime = 0f;
        
        public override void Update() {
            base.Update();
            
            float timeDelta = Time.time - prevUpdateTime;
            if (timeDelta < PoseTime) return;
            Cat.Behaviour.SwitchState(CatState.Idle);
        }
        
        public override void Enable() {
            base.Enable();
            Cat.Behaviour.Animator.SetBool("IsPosing", true);
            prevUpdateTime = Time.time;
        }
        
        public override void Disable() {
            base.Disable();
            Cat.Behaviour.Animator.SetBool("IsPosing", false);
        }
    }
}
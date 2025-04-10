using Server;
using UnityEngine;
using Time = UnityEngine.Time;

namespace Cat.Behaviours {
    public class CatPoseBehaviour : Behaviour {
        public CatPoseBehaviour(Cat cat) : base(cat) { }
        
        public float PoseTime = 6f;
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
            
            string catBreed = Cat.catBreed.ToLower();
            GameObject gameObject = Server.Server.Instance.playerScript.Pocket.CameraObject.gameObject;
            switch (catBreed) {
                case "ragdoll":
                    AkUnitySoundEngine.PostEvent("mus_Ragdoll_Pose", gameObject);
                    break;
                case "grey cat":
                    AkUnitySoundEngine.PostEvent("mus_GreyCat_Pose", gameObject);
                    break;
                case "grey tabby":
                    AkUnitySoundEngine.PostEvent("mus_Tabby_Pose", gameObject);
                    break;
                case "tuxedo":
                    AkUnitySoundEngine.PostEvent("mus_Tuxedo_Pose", gameObject);
                    break;
                case "bengal":
                    AkUnitySoundEngine.PostEvent("mus_Bengal_Pose", gameObject);
                    break;
                default:
                    AkUnitySoundEngine.PostEvent("mus_Ragdoll_Pose", gameObject);
                    break;
                // }
            }
        }
        
        public override void Disable() {
            base.Disable();
            Cat.Behaviour.Animator.SetBool("IsPosing", false);
        }
    }
}
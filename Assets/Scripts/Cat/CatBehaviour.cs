using System;
using System.Collections.Generic;
using Cat.Behaviours;
using Player;
using UnityEngine;

namespace Cat {

    public enum CatState {
        Idle,
        Treat,
        Pose,
        Sit,
        FollowTreat,
        Eat,
    }
    
    public class CatBehaviour : MonoBehaviour {
        public Animator Animator;
        public Cat Cat;
        public GameObject catIdentifier;
        [NonSerialized] public CatState State = CatState.Idle;
        [NonSerialized] public TreatInstance TargetTreat;
        
        /* Audios */
        // public AudioSource meowAudio;
        // public AK.Wwise.Event meow;
        public string meowEventName;
        protected float meowCooldown = 15f;
        
        /* Behaviours script */
        public Dictionary<CatState, Behaviour> StateToBehaviour = new Dictionary<CatState, Behaviour>();
        
        public void SwitchState(CatState state) {
            StateToBehaviour[State].Disable();
            State = state;
            StateToBehaviour[State].Enable();
        }
        
        public void StopOtherAnimations() {
            Cat.Behaviour.Animator.SetBool("IsWondering", false);
            Cat.Behaviour.Animator.SetBool("IsWalking", false);
            Cat.Behaviour.Animator.SetBool("IsSitting", false);
            Cat.Navigator.Agent.ResetPath();
        }
        
        protected void Awake() {
            Cat = GetComponent<Cat>();

            StateToBehaviour[CatState.Idle] = new CatIdleBehaviour(Cat);
            //TODO: change to other behaviours
            StateToBehaviour[CatState.Treat] = new CatIdleBehaviour(Cat);
            StateToBehaviour[CatState.Sit] = new CatSitBehaviour(Cat);
            StateToBehaviour[CatState.FollowTreat] = new CatFollowTreatBehaviour(Cat);
            StateToBehaviour[CatState.Pose] = new CatPoseBehaviour(Cat);
            StateToBehaviour[CatState.Eat] = new CatEatBehaviour(Cat);
            
            SwitchState(CatState.Idle);
        }

        protected void Start() {
            AkUnitySoundEngine.RegisterGameObj(gameObject);
        }

        protected void Update() {
            if (!StateToBehaviour[State].Enabled) return;
            StateToBehaviour[State].Update();
            
            /* Meow cooldown */
            meowCooldown -= Time.deltaTime;
            if (meowCooldown <= 0) {
                /* Calculate the distance between player */
                float distance = Vector3.Distance(Server.Server.Instance.player.transform.position, transform.position);
                if (distance > 25) return;
                
                // meowAudio.Play();
                AkUnitySoundEngine.PostEvent(meowEventName, gameObject);
                /* Generate random number for cooldown between 2 and 8 */
                meowCooldown = UnityEngine.Random.Range(25, 60);
            }
        }
    }
}
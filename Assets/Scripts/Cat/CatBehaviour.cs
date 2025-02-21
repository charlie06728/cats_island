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
        FollowTreat,
    }
    
    public class CatBehaviour : MonoBehaviour {
        public Animator Animator;
        public Cat Cat;
        [NonSerialized] public CatState State = CatState.Idle;
        [NonSerialized] public TreatInstance TargetTreat;
        
        /* Audios */
        public AudioSource meowAudio;
        protected float meowCooldown = 3;
        
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
            StateToBehaviour[CatState.Treat] = new CatIdleBehaviour(Cat);
            StateToBehaviour[CatState.Pose] = new CatPoseBehaviour(Cat);
            StateToBehaviour[CatState.FollowTreat] = new CatFollowTreatBehaviour(Cat);
            
            SwitchState(CatState.Idle);
        }


        protected void Update() {
            if (!StateToBehaviour[State].Enabled) return;
            StateToBehaviour[State].Update();
            
            /* Meow cooldown */
            meowCooldown -= Time.deltaTime;
            if (meowCooldown <= 0) {
                meowAudio.Play();
                /* Generate random number for cooldown between 2 and 8 */
                meowCooldown = UnityEngine.Random.Range(5, 9);
            }
        }
    }
}
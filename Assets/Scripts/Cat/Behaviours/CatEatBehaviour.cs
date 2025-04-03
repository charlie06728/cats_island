using System.Collections;
using System.Collections.Generic;
using Server;
using UnityEngine;
using Time = UnityEngine.Time;

namespace Cat.Behaviours {
    public class CatEatBehaviour : Behaviour {
        public CatEatBehaviour(Cat cat) : base(cat) { }

        protected float StartEtaTime = 0f;
        protected float EatTime = 3.3f;
        public bool Eating = false;

        public override void Enable() {
            base.Enable();
            Eating = false;
        }

        public override void Update() {
            base.Update();

            if (Cat.Behaviour.TargetTreat == null) {
                Cat.Behaviour.SwitchState(CatState.Idle);
                return;
            }

            /* If cat is eating, calculate the time the cat has been eating */
            if (Eating) {
                float eatTime = Time.time - StartEtaTime;
                if (eatTime > EatTime) {
                    /* Eat animation ends */
                    Eating = false;
                    Cat.Behaviour.Animator.SetBool("IsEating", false);
                    /* Destroy the treat */
                    GameObject.Destroy(Cat.Behaviour.TargetTreat.gameObject);
                    
                    /* Scale cat bigger including childs object */
                    Cat.StartCoroutine(Bigger(Cat.gameObject));
                    
                    Cat.Behaviour.SwitchState(CatState.Pose);
                }
            } else {
                Cat.catEat.Post(Cat.gameObject);
                Eating = true;
                StartEtaTime = Time.time;
                /* Trigger the eating animation */
                Cat.Behaviour.Animator.SetBool("IsEating", true);
            }
        }

        protected IEnumerator Bigger(GameObject cat) {
            /* Make cat bigger smoothly */
            float time = 0f;
            float duration = 1f;
            Vector3 startScale = cat.transform.localScale;
            Vector3 endScale = startScale * 1.25f;
            while (time < duration) {
                time += Time.deltaTime;
                cat.transform.localScale = Vector3.Lerp(startScale, endScale, time / duration);
                yield return null;
            }
        }
    }
}
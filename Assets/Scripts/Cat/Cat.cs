using System;
using UnityEngine;

namespace Cat {
    public class Cat : MonoBehaviour{
        public CatBehaviour Behaviour;
        public CatNavigator Navigator;

        protected void Awake() {
            /* Make sure the components are set */
            if (Behaviour == null) Behaviour = gameObject.AddComponent<CatBehaviour>();
            if (Navigator == null) Navigator = gameObject.AddComponent<CatNavigator>();
            Navigator.Cat = this;
            Behaviour.Cat = this;
        }
    }
}
using System;
using UnityEngine;

namespace Cat {
    public class Cat : MonoBehaviour{
        public CatBehaviour Behaviour;
        public CatNavigator Navigator;

        public string catName;
        public string catBreed;

        protected virtual void Awake() {
            /* Make sure the components are set */
            if (Behaviour == null) Behaviour = gameObject.AddComponent<CatBehaviour>();
            if (Navigator == null) Navigator = gameObject.AddComponent<CatNavigator>();
            Navigator.Cat = this;
            Behaviour.Cat = this;
            
            /* Register to server */
            Server.Server.Instance.Cats.Add(this);
        }
    }
}
using System;
using UnityEngine;

namespace DefaultNamespace {
    public class SoundManager : MonoBehaviour {
        public AK.Wwise.Event amb;
        public AK.Wwise.Event snd;
        public AkEvent ambEvent;

        protected void Start() {
            amb.Post(gameObject);
        }
    }
}
using System;
using Server;
using Terrain;
using UnityEngine;
using Time = UnityEngine.Time;

namespace Cat.Behaviours {
    public class CatPoseBehaviour : Behaviour {
        public CatPoseBehaviour(Cat cat) : base(cat) { }

        public override void Enable() {
            base.Enable();
            Cat.Navigator.Agent.isStopped = true;
        }

        public override void Disable() {
            Cat.Navigator.Agent.isStopped = false;
            base.Disable();
        }
    }
}

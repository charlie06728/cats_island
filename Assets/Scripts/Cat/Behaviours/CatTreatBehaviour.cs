using System;
using Server;
using Terrain;
using UnityEngine;
using Time = UnityEngine.Time;

namespace Cat.Behaviours {
    public class CatTreatBehaviour : Behaviour {
        public CatTreatBehaviour(Cat cat) : base(cat) { }

        GameObject closestTreat = null;

        public override void Update() {
            base.Update();

            if (!closestTreat) {
                // If the treat has already been eaten, cancel navigation and return to idle
                Cat.Navigator.Agent.ResetPath();
                Cat.Behaviour.SwitchState(CatState.Idle);
            }
            
            if(Cat.Navigator.Agent.remainingDistance < 0.1 && closestTreat) {
                // If they reach the treat, eat it and pose
                GameObject.Destroy(closestTreat);
                Cat.Behaviour.SwitchState(CatState.Pose);
            }
            
        }

        
        public override void Enable() {
            base.Enable();
            // Cat.Navigator.Agent.isStopped = true;

            GameObject[] treats = GameObject.FindGameObjectsWithTag("placedTreat");

            Transform tMin = null;
            
            // Find closest treat
            float minDist = Mathf.Infinity;
            Vector3 currentPos = Cat.gameObject.transform.position;
            foreach (GameObject g in treats)
            {   
                Transform t = g.transform;
                float dist = Vector3.Distance(t.position, currentPos);
                if (dist < minDist)
                {
                    tMin = t;
                    closestTreat = g;
                    minDist = dist;
                }
            }
            
            // Navigate to it
            Cat.Navigator.MoveTo(tMin.position);
        }
    }
}
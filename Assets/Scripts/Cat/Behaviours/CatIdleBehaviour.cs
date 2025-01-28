using System;
using Server;
using Terrain;
using UnityEngine;
using Time = UnityEngine.Time;

namespace Cat.Behaviours {
    public class CatIdleBehaviour : Behaviour {
        public CatIdleBehaviour(Cat cat) : base(cat) { }

        public override void Update() {
            base.Update();
            
            /* Check if time delta reach the random move interval */
            if (Time.time - PreviousEvaluateTime >= Server.Server.Instance.CatRandomMoveInterval) {
                PreviousEvaluateTime = Time.time;
                
                Vector3 terrainSize = TerrainManager.Instance.Terrain.terrainData.size;
                /* Generate a random position in the terrain */
                Vector3 randomPosition = new Vector3(
                    UnityEngine.Random.Range(0, terrainSize.x),
                    0,
                    UnityEngine.Random.Range(0, terrainSize.z)
                );
                
                /* convert this local position to world position */
                randomPosition = TerrainManager.Instance.Terrain.transform.TransformPoint(randomPosition);
                
                /* Move to the random position */
                Cat.Navigator.MoveTo(randomPosition);
            }
        }
    }
}
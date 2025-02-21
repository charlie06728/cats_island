using System;
using System.Collections;
using System.Collections.Generic;
using Server;
using Terrain;
using UnityEngine;
using Time = UnityEngine.Time;

namespace Cat.Behaviours {
    public class CatIdleBehaviour : Behaviour {
        public CatIdleBehaviour(Cat cat) : base(cat) { }
        protected float WonderStopTime;
        protected float WonderTime = 2.5f;

        public override void Update() {
            base.Update();
            
            /* Check if time delta reach the random move interval */
            if (Time.time - PreviousEvaluateTime >= Server.Server.Instance.CatRandomMoveInterval) {
                PreviousEvaluateTime = Time.time;
                
                /* Generate a random boolean */
                bool randomBool = UnityEngine.Random.value < Cat.walkProportion;
                if (randomBool) {
                    Vector3 position = Cat.livingArea.transform.position;
                    /* Convert position to terrain local coordinates */
                    position = TerrainManager.Instance.Terrain.transform.InverseTransformPoint(position);
                    float radius = 45;
                
                    Vector3 terrainSize = TerrainManager.Instance.Terrain.terrainData.size;
                    /* Generate a random position with fixed radius in the terrain */
                    Vector3 randomPosition = new Vector3(
                        UnityEngine.Random.Range(position.x - radius, position.x + radius),
                        0,
                        UnityEngine.Random.Range(position.z - radius, position.z + radius)
                    );
                    // Vector3 randomPosition = new Vector3(
                    //     UnityEngine.Random.Range(0, terrainSize.x),
                    //     0,
                    //     UnityEngine.Random.Range(0, terrainSize.z)
                    // );
                
                    /* convert this local position to world position */
                    randomPosition = TerrainManager.Instance.Terrain.transform.TransformPoint(randomPosition);
                
                    /* Move to the random position */
                    Cat.Navigator.MoveTo(randomPosition);
                } else {
                    if (Cat.Behaviour.Animator.GetBool("IsWondering")) {
                        WonderStopTime += WonderTime;
                        return;
                    }
                    
                    Cat.Behaviour.Animator.SetBool("IsWondering", true);
                    Cat.StartCoroutine(WonderingStopCoroutine(WonderTime));
                }
            }
        }
        
        protected IEnumerator WonderingStopCoroutine(float wonderTime) {
            WonderStopTime = Time.time + wonderTime;
            while (Time.time < WonderStopTime && Cat.Behaviour.State == CatState.Idle) {
                yield return null;
            }
            
            Cat.Behaviour.Animator.SetBool("IsWondering", false);
        }
    }
}
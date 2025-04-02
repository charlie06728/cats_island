using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using Server;
using Terrain;
using UnityEngine;
using Time = UnityEngine.Time;

namespace Cat.Behaviours {
    public class CatIdleBehaviour : Behaviour {
        public CatIdleBehaviour(Cat cat) : base(cat) { }
        protected float WonderStopTime;
        protected float WonderTime = 2.5f;
        protected float playerAlartDistance = 15;
        protected float runAwayDistance = 7;

        protected bool runningAway = false;

        public override void Update() {
            base.Update();

            if (runningAway && Cat.Navigator.Agent.velocity.magnitude < 0.1f) {
                Cat.Behaviour.StopOtherAnimations();
                runningAway = false;
            } else if (runningAway) {
                return;
            }
            
            /* Check distance between the player */
            float distance = Vector3.Distance(Cat.transform.position, Server.Server.Instance.player.transform.position);
            if (distance <= runAwayDistance) {
                Cat.Behaviour.StopOtherAnimations();
                Debug.Log("Entering run away mode");

                Items favouriteTreat;
                switch (Cat.catBreed) {
                    case "Bengal":
                        favouriteTreat = Items.Fish;
                        break;
                    case "Grey Cat":
                        favouriteTreat = Items.Cookie;
                        break;
                    case "Grey Tabby":
                        favouriteTreat = Items.Fish;
                        break;
                    case "Ragdoll":
                        favouriteTreat = Items.Cookie;
                        break;
                    case "Tuxedo":
                        favouriteTreat = Items.Heart;
                        break;
                    default:
                        favouriteTreat = Items.Heart;
                        break;
                }
                
                /* Cat will not run away but sit instead when player holding favourite treat */
                if (Server.Server.Instance.playerScript.Pocket.CurrentItem == favouriteTreat) {
                    return;
                }
                
                /* calculate the vector opposite to the cat player direction */
                Vector3 runAwayDirection = Cat.transform.position - Server.Server.Instance.player.transform.position;
                /* Normalize it to playerAlartDistance */
                runAwayDirection = runAwayDirection.normalized * playerAlartDistance;
                runningAway = true;
                Vector3 destination = Cat.transform.position + runAwayDirection;
                Cat.Navigator.MoveTo(destination, true);
            } else if (distance <= playerAlartDistance) {
                Cat.Behaviour.StopOtherAnimations();
                Debug.Log("Entering player alart mode");
                /* Make cat turn face to the player smoothly */
                Vector3 direction = Server.Server.Instance.player.transform.position - Cat.transform.position;
                direction.y = 0;
                Quaternion toRotation = Quaternion.LookRotation(direction);
                Cat.transform.rotation = Quaternion.RotateTowards(Cat.transform.rotation, toRotation, 180 * Time.deltaTime);
            } else {
                /* Check if time delta reach the random move interval */
                if (Time.time - PreviousEvaluateTime >= Server.Server.Instance.CatRandomMoveInterval) {
                    PreviousEvaluateTime = Time.time;

                    /* Generate a random boolean */
                    float rand1 = UnityEngine.Random.value;
                    if (rand1 < Cat.walkProportion) {
                        Vector3 position = Cat.livingArea.transform.position;
                        /* Convert position to terrain local coordinates */
                        position = TerrainManager.Instance.Terrain.transform.InverseTransformPoint(position);
                        float radius = 20;

                        Vector3 terrainSize = TerrainManager.Instance.Terrain.terrainData.size;
                        /* Generate a random position with fixed radius in the terrain */
                        Vector3 randomPosition = new Vector3(
                            UnityEngine.Random.Range(position.x - radius, position.x + radius),
                            0,
                            UnityEngine.Random.Range(position.z - radius, position.z + radius)
                        );

                        /* convert this local position to world position */
                        randomPosition = TerrainManager.Instance.Terrain.transform.TransformPoint(randomPosition);

                        /* Move to the random position */
                        Cat.Navigator.MoveTo(randomPosition);

                    }
                    else if (rand1 > Cat.sitProportion + Cat.walkProportion) {
                        if (Cat.Behaviour.Animator.GetBool("IsWondering")) {
                            WonderStopTime += WonderTime;
                            return;
                        }

                        Cat.Behaviour.Animator.SetBool("IsWondering", true);
                        Cat.StartCoroutine(WonderingStopCoroutine(WonderTime));
                    }
                    else {
                        Cat.Behaviour.SwitchState(CatState.Sit);
                    }
                }
            }
        }

        public override void Enable() {
            base.Enable();
            PreviousEvaluateTime = Time.time;
        }

        public override void Disable() {
            base.Disable();
            Cat.Behaviour.Animator.SetBool("IsWondering", false);
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
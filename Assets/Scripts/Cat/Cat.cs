using System;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

namespace Cat {
    public class Cat : MonoBehaviour{
        public CatBehaviour Behaviour;
        public CatNavigator Navigator;
        public Image catImage;
        public string catPreferredSnack;
        public string catHabitat;
        public float walkProportion = 0.5f;
        public float sitProportion = 0.3f;

        public List<GameObject> castPoints;
        
        public AK.Wwise.Event catEat;
        
        /* The pointer around which cat moves */
        public GameObject livingArea;

        public string catName;
        public string catBreed;
        [HideInInspector] public Items favTreat = Items.Cookie;

        public void RegisterTreat(TreatInstance treat) {
            
        }

        protected virtual void Awake() {
            /* Make sure the components are set */
            if (Behaviour == null) {
                Behaviour = gameObject.AddComponent<CatBehaviour>();
                Behaviour.Animator = gameObject.GetComponent<Animator>();
            }
            if (Navigator == null) Navigator = gameObject.AddComponent<CatNavigator>();
            Navigator.Cat = this;
            Behaviour.Cat = this;
            
            catPreferredSnack = "Preferred Snack: " + catPreferredSnack;
            catHabitat = "Habitat: " + catHabitat;
            
            /* Register to server */
            Server.Server.Instance.Cats.Add(this);
            Server.Server.Instance.CatDictionary[catBreed] = this;
            
            /* If living area is null, create a empty object at current location, set parent to null and use it
               as living area */
            if (livingArea == null) {
                livingArea = new GameObject("Living Area");
                livingArea.transform.position = transform.position;
                livingArea.transform.SetParent(null);
            }
        }
    }
}
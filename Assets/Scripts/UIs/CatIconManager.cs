using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Player {
    
    public class CatIconManager : MonoBehaviour
    {   
        // public Brochure brochure;

        public CatIcon ragdoll;
        public CatIcon tabby;
        public CatIcon gray;
        public CatIcon tuxedo;
        public CatIcon bengal;
        List<string> catlist = new List<string>();
        //"grey tabby", "grey cat", "tuxedo", "ragdoll", "bengal"

        void Awake() {
            catlist.Add("Grey Tabby");
            catlist.Add("Grey Cat");
            catlist.Add("Tuxedo");
            catlist.Add("Ragdoll");
            catlist.Add("Bengal");
        }

        
        void Update() {
            foreach (string breed in catlist) {
                if (Brochure.CollectedCats.Contains(breed)) {
                    UpdateIcons(breed);
                }
            }
        }

        public void UpdateIcons(string catBreed)
        {
            switch (catBreed) {
                case "Ragdoll":
                    ragdoll.ToggleOn();
                    break;
                case "Grey Cat":
                    gray.ToggleOn();
                    break;
                case "Grey Tabby":
                    tabby.ToggleOn();
                    break;
                case "Tuxedo":
                    tuxedo.ToggleOn();
                    break;
                case "Bengal":
                    bengal.ToggleOn();
                    break;
            }
        }
    }
}

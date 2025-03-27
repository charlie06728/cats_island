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
        public FlashNotif sticker;
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
            // foreach (string breed in catlist) {
            //     if (Brochure.PurrfectCats.Contains(breed)) {
            //         EnablePurrfect(breed);
            //     } else if (Brochure.CollectedCats.Contains(breed)) {
            //         EnableIcon(breed);
            //     }
            // }
            for(int i = catlist.Count - 1; i >= 0; i--) {
                string breed = catlist[i];

                if (Brochure.PurrfectCats.Contains(breed)) {
                    EnablePurrfect(breed);
                    catlist.RemoveAt(i); // If we've already purrfected this icon, we'll never need to update it again
                } else if (Brochure.CollectedCats.Contains(breed)) {
                    EnableIcon(breed);
                }
            }
        }

        public void EnablePurrfect(string catBreed)
        {   
            sticker.StartFlash();
            switch (catBreed) {
                case "Ragdoll":
                    ragdoll.TogglePurrfect();
                    break;
                case "Grey Cat":
                    gray.TogglePurrfect();
                    break;
                case "Grey Tabby":
                    tabby.TogglePurrfect();
                    break;
                case "Tuxedo":
                    tuxedo.TogglePurrfect();
                    break;
                case "Bengal":
                    bengal.TogglePurrfect();
                    break;
            }
        }

        public void EnableIcon(string catBreed)
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

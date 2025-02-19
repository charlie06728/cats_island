using System;
using TMPro;
using UnityEngine.UI;

namespace Player {
    public class Brochure : Item {
        public RawImage rawImage1;
        public RawImage rawImage2;

        public Image[] image1Stars;
        public Image[] image2Stars;

        public TextMeshProUGUI catName1;
        public TextMeshProUGUI catName2;
        
        public TextMeshProUGUI catBreed1;
        public TextMeshProUGUI catBreed2;
        
        [NonSerialized] public Photo Photo1;
        [NonSerialized] public Photo Photo2;
        
        public override void TakeOut() {
            if (gameObject.activeInHierarchy) {
                PutBack();
                return;
            }
            
            /* put all back first */
            PutBackAll();

            /* Set item visible */
            gameObject.SetActive(true);
            
            HideAll();
        }

        protected void HideAll() {
            rawImage1.gameObject.SetActive(false);
            rawImage2.gameObject.SetActive(false);
            catName1.gameObject.SetActive(false);
            catName2.gameObject.SetActive(false);
            catBreed1.gameObject.SetActive(false);
            catBreed2.gameObject.SetActive(false);
            foreach (var image in image1Stars) {
                image.gameObject.SetActive(false);
            }
            foreach (var image in image2Stars) {
                image.gameObject.SetActive(false);
            }
        }
        
        protected void ShowAll() {
            rawImage1.gameObject.SetActive(true);
            rawImage2.gameObject.SetActive(true);
            catName1.gameObject.SetActive(true);
            catName2.gameObject.SetActive(true);
            catBreed1.gameObject.SetActive(true);
            catBreed2.gameObject.SetActive(true);
            foreach (var image in image1Stars) {
                image.gameObject.SetActive(true);
            }
            foreach (var image in image2Stars) {
                image.gameObject.SetActive(true);
            }
        }
    }
}
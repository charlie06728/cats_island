using System;
using System.Collections.Generic;
using System.Linq;
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
            DisplayPhotos();
        }
        
        public override void PutBack() {
            HideAll();
            
            /* make item invisible */
            gameObject.SetActive(false);
        }

        protected void DisplayPhotos() {
            List<Photo> photos = PlayerPocket.Album.Photos;

            int photoCount = 0;
            Photo prevP = null;
            Photo currP = null;
            foreach (Photo photo in photos) {
                if (photo.Cats.Count == 0) continue;
                if (currP == null || photo.Stars >= currP.Stars) {
                    if (currP != null) prevP = currP;
                    currP = photo;
                }
            }
            
            DisplayPhoto(currP, 0);
            DisplayPhoto(prevP, 1);
        }

        protected void DisplayPhoto(Photo photo, int displayIndex) {
            if (photo == null) return;
            
            if (displayIndex == 0) {
                rawImage1.gameObject.SetActive(true);
                catName1.gameObject.SetActive(true);
                catBreed1.gameObject.SetActive(true);
                
                rawImage1.texture = photo.PhotoTexture;
                catName1.text = photo.Cats.First().catName;
                catBreed1.text = photo.Cats.First().catBreed;
                for (int i = 0; i < photo.Stars; i++) {
                    image1Stars[i].gameObject.SetActive(true);
                }
                /* Hide the rest of stars */
                for (int i = photo.Stars; i < image1Stars.Length; i++) {
                    image1Stars[i].gameObject.SetActive(false);
                }
            } else {
                rawImage2.gameObject.SetActive(true);
                catName2.gameObject.SetActive(true);
                catBreed2.gameObject.SetActive(true);
                
                rawImage2.texture = photo.PhotoTexture;
                catName2.text = photo.Cats.First().catName;
                catBreed2.text = photo.Cats.First().catBreed;
                for (int i = 0; i < photo.Stars; i++) {
                    image2Stars[i].gameObject.SetActive(true);
                }
                /* Hide the rest of stars */
                for (int i = photo.Stars; i < image2Stars.Length; i++) {
                    image2Stars[i].gameObject.SetActive(false);
                }
            }
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
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Player {
    public class AlbumSlot : MonoBehaviour {
        public RawImage rawImage;
        public Image[] stars;
        public TextMeshProUGUI catName;
        public TextMeshProUGUI catBreed;

        public void Hide() {
            rawImage.gameObject.SetActive(false);
            catName.gameObject.SetActive(false);
            catBreed.gameObject.SetActive(false);
            foreach (var star in stars) {
                star.enabled = false;
            }
        }
        
        public void Show() {
            rawImage.gameObject.SetActive(true);
            catName.gameObject.SetActive(true);
            catBreed.gameObject.SetActive(true);
            foreach (var star in stars) {
                star.gameObject.SetActive(true);
            }
        }
        
        
        public void SetPhoto(Photo photo) {
            Show();
            rawImage.texture = photo.PhotoTexture;
            catName.text = photo.Cats.Count > 0 ? photo.Cats.First().catName : "Unknown";
            catBreed.text = photo.Cats.Count > 0 ? photo.Cats.First().catBreed : "Unknown";
            for (int i = 0; i < stars.Length; i++) {
                stars[i].enabled = i < photo.Stars;
            }
            /* disable the rest of stars */
            for (int i = photo.Stars; i < stars.Length; i++) {
                stars[i].enabled = false;
            }
        }
    }
}
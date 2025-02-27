using UnityEngine;

namespace Player {
    public enum Items {
        Camera,
        Album,
        Brochure,
        Cookie,
        Fish,
        Heart
    }
    
    public class ItemBar : MonoBehaviour {
        public Player player;
        public GameObject cameraSelect;
        public GameObject albumSelect;
        public GameObject brochureSelect;
        public GameObject cookieSelect;
        public GameObject fishSelect;
        public GameObject heartSelect;

        public void SetCurrentItem(Items item) {
            switch (item) {
                case Items.Camera:
                    DeSelectAll();
                    if (cameraSelect.activeInHierarchy) {
                        cameraSelect.SetActive(false);
                    } else {
                        cameraSelect.SetActive(true);
                    }
                    break;
                case Items.Album:
                    DeSelectAll();
                    if (albumSelect.activeInHierarchy) {
                        albumSelect.SetActive(false);
                    } else {
                        albumSelect.SetActive(true);
                    }
                    break;
                case Items.Brochure:
                    DeSelectAll();
                    if (brochureSelect.activeInHierarchy) {
                        brochureSelect.SetActive(false);
                    } else {
                        brochureSelect.SetActive(true);
                    }
                    break;
                case Items.Cookie:
                    DeSelectAll();
                    if (cookieSelect.activeInHierarchy) {
                        cookieSelect.SetActive(false);
                    } else {
                        cookieSelect.SetActive(true);
                    }
                    break;
                case Items.Fish:
                    DeSelectAll();
                    if (fishSelect.activeInHierarchy) {
                        fishSelect.SetActive(false);
                    } else {
                        fishSelect.SetActive(true);
                    }
                    break;
                case Items.Heart:
                    DeSelectAll();
                    if (heartSelect.activeInHierarchy) {
                        heartSelect.SetActive(false);
                    } else {
                        heartSelect.SetActive(true);
                    }
                    break;
            }
        }
        
        public void DeSelectAll() {
            cameraSelect.SetActive(false);
            albumSelect.SetActive(false);
            brochureSelect.SetActive(false);
            cookieSelect.SetActive(false);
            fishSelect.SetActive(false);
            heartSelect.SetActive(false);
        }
    }
}
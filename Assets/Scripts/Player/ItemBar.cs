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
                    // if (cookieSelect.activeInHierarchy) {
                    //     SetCurrentItem(Items.Fish);
                    // } else {
                    //     DeSelectAll();
                    //     cookieSelect.transform.parent.gameObject.SetActive(true);
                    //     cookieSelect.SetActive(true);
                    // }
                    
                    DeSelectAll();
                    cookieSelect.transform.parent.gameObject.SetActive(true);
                    cookieSelect.SetActive(true);
                    
                    /* Take out cookie */
                    player.Pocket.Cookie.TakeOut();
                    
                    // if (cookieSelect.activeSelf) {
                    //     cookieSelect.SetActive(false);
                    // } else {
                    //     cookieSelect.transform.parent.gameObject.SetActive(true);
                    //     cookieSelect.SetActive(true);
                    // }
                    break;
                case Items.Fish:
                    // if (fishSelect.activeInHierarchy) {
                    //     SetCurrentItem(Items.Heart);
                    // } else {
                    //     DeSelectAll();
                    //     fishSelect.transform.parent.gameObject.SetActive(true);
                    //     fishSelect.SetActive(true);
                    // }
                    
                    DeSelectAll();
                    fishSelect.transform.parent.gameObject.SetActive(true);
                    fishSelect.SetActive(true);
                    
                    /* Take out fish */
                    player.Pocket.Fish.TakeOut();
                    
                    // if (fishSelect.activeSelf) {
                    //     fishSelect.SetActive(false);
                    // } else {
                    //     fishSelect.transform.parent.gameObject.SetActive(true);
                    //     fishSelect.SetActive(true);
                    // }
                    break;
                case Items.Heart:
                    // if (heartSelect.activeInHierarchy) {
                    //     SetCurrentItem(Items.Cookie);
                    // } else {
                    //     DeSelectAll();
                    //     heartSelect.transform.parent.gameObject.SetActive(true);
                    //     heartSelect.SetActive(true);
                    // }
                    
                    DeSelectAll();
                    heartSelect.transform.parent.gameObject.SetActive(true);
                    heartSelect.SetActive(true);
                    
                    /* Take out heart */
                    player.Pocket.Heart.TakeOut();
                    
                    // if (heartSelect.activeSelf) {
                    //     heartSelect.SetActive(false);
                    // } else {
                    //     heartSelect.transform.parent.gameObject.SetActive(true);
                    //     heartSelect.SetActive(true);
                    // }
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
            
            /* Make all treats invisible */
            cookieSelect.transform.parent.gameObject.SetActive(false);
            fishSelect.transform.parent.gameObject.SetActive(false);
            heartSelect.transform.parent.gameObject.SetActive(false);
            
            /* Make current treat visible */
            switch (player.Pocket.CurrentTreat) {
                case 0:
                    cookieSelect.transform.parent.gameObject.SetActive(true);
                    break;
                case 1:
                    heartSelect.transform.parent.gameObject.SetActive(true);
                    break;
                case 2:
                    fishSelect.transform.parent.gameObject.SetActive(true);
                    break;
            }
        }
    }
}
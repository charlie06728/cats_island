using System;
using System.Collections.Generic;
using UIs;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player {
    public class PlayerPocket : MonoBehaviour {
        [NonSerialized] public Player Player;
        
        /* Album prefab and component after it being initialized */
        public GameObject albumPrefab;
        public GameObject albumVirtualPosition;
        [NonSerialized] public Album Album;
        
        /* Camera prefab and component after init */
        public GameObject cameraPrefab;
        public GameObject cameraVirtualPosition;
        // public Animator cameraAnimator;
        // public CameraAnimation cameraAnimation;
        public GameObject cameraAnimationPrefab;
        [NonSerialized] public CameraObject CameraObject;
        
        /* treat prefab and component after it being initialized */
        public GameObject treatPrefab;
        public GameObject cookiePrefab;
        public GameObject heartPrefab;
        public GameObject fishPrefab;
        [NonSerialized] public Treat Treat;
        [NonSerialized] public Cookie Cookie;
        [NonSerialized] public Heart Heart;
        [NonSerialized] public Fish Fish;
        [NonSerialized] public int CurrentTreat = 0;
        [NonSerialized] public Items CurrentItem = 0;
        [NonSerialized] public List<Items> Treats = new List<Items> {Items.Cookie, Items.Heart, Items.Fish};
        [NonSerialized] public List<Item> ItemList = new List<Item>();

        public GameObject treatControls;
        public GameObject cameraControls;
        public GameObject albumControls;

        public AK.Wwise.Event sfx_select;
        public AK.Wwise.Event sfx_equip;
        
        /* Brochure prefab and component after it being initialized */
        public Brochure brochure;

        protected void Awake() {
            /* initialize the camera */
            GameObject cameraObj = Instantiate(cameraPrefab, transform);
            CameraObject = cameraObj.GetComponent<CameraObject>();
            if (CameraObject == null) throw new Exception("Camera prefab does not have a CameraObject component!");
            CameraObject.PlayerPocket = this;
            CameraObject.gameObject.SetActive(false);
            ItemList.Add(CameraObject);
            
            /* Initialize the album */
            GameObject albumObj = Instantiate(albumPrefab, transform);
            Album = albumObj.GetComponent<Album>();
            if (Album == null) throw new Exception("Album prefab does not have an Album component!");
            Album.PlayerPocket = this;
            Album.gameObject.SetActive(false);
            ItemList.Add(Album);

            /* Initialize the treat */
            // GameObject treatObj = Instantiate(treatPrefab, transform);
            // Treat = treatObj.GetComponent<Treat>();
            // if (Treat == null) throw new Exception("Treat prefab does not have an Treat component!");
            // Treat.PlayerPocket = this;
            // Treat.gameObject.SetActive(false);
            
            /* Initialize the treats */
            GameObject cookieObj = Instantiate(cookiePrefab, transform);
            Cookie = cookieObj.GetComponent<Cookie>();
            if (Cookie == null) throw new Exception("Cookie prefab does not have an Cookie component!");
            Cookie.PlayerPocket = this;
            Cookie.gameObject.SetActive(false);
            Treat = Cookie;
            ItemList.Add(brochure);
            ItemList.Add(Cookie);
            
            GameObject heartObj = Instantiate(heartPrefab, transform);
            Heart = heartObj.GetComponent<Heart>();
            if (Heart == null) throw new Exception("Heart prefab does not have an Heart component!");
            Heart.PlayerPocket = this;
            Heart.gameObject.SetActive(false);
            
            GameObject fishObj = Instantiate(fishPrefab, transform);
            Fish = fishObj.GetComponent<Fish>();
            if (Fish == null) throw new Exception("Fish prefab does not have an Fish component!");
            Fish.PlayerPocket = this;
            Fish.gameObject.SetActive(false);
            
            /* Initialize the brochure */
            if (brochure == null) throw new Exception("Brochure prefab does not have an Brochure component!");
            brochure.PlayerPocket = this;
            brochure.gameObject.SetActive(false);
            
            /* Define the item switch behaviour */
            Server.Server.Instance.InputActionMap["1"].performed += context => {
                CameraObject.TakeOut(); 
                cameraControls.SetActive(true);
                treatControls.SetActive(false);
                albumControls.SetActive(false);
            };
            Server.Server.Instance.InputActionMap["2"].performed += context => {
                Album.TakeOut(); 
                cameraControls.SetActive(false);
                treatControls.SetActive(false);
                albumControls.SetActive(true);
            };
            Server.Server.Instance.InputActionMap["3"].performed += context => {
                /* Put back all other items */
                CameraObject.PutBackAll();
                cameraControls.SetActive(false);
                treatControls.SetActive(true);
                albumControls.SetActive(false);
                
                CurrentTreat++;
                if (CurrentTreat >= Treats.Count) CurrentTreat = 0;
                SwitchTreat(Treats[CurrentTreat]);
                Server.Server.Instance.itemBar.SetCurrentItem(Treats[CurrentTreat]);
            };
            Server.Server.Instance.InputActionMap["4"].performed += context => {
                cameraControls.SetActive(false);
                treatControls.SetActive(false);
                albumControls.SetActive(true);
                brochure.TakeOut(); 
            };
            Server.Server.Instance.InputActionMap["5"].performed += context => {
                cameraControls.SetActive(false);
                treatControls.SetActive(false);
                albumControls.SetActive(false);
                Player.Pocket.CameraObject.PutBackAll(); 
                Server.Server.Instance.itemBar.DeSelectAll();
            };
            // Server.Server.Instance.InputActionMap["PrevTreat"].performed += context => {
            //     if (CurrentItem != 3) return;
            //     CurrentTreat--;
            //     if (CurrentTreat < 0) CurrentTreat = Treats.Count - 1;
            //     SwitchTreat(Treats[CurrentTreat]);
            //     Server.Server.Instance.itemBar.SetCurrentItem(Treats[CurrentTreat]);
            // };
            // Server.Server.Instance.InputActionMap["NextTreat"].performed += context => {
            //     if (CurrentItem != 3) return;
            //     CurrentTreat++;
            //     if (CurrentTreat >= Treats.Count) CurrentTreat = 0;
            //     SwitchTreat(Treats[CurrentTreat]);
            //     Server.Server.Instance.itemBar.SetCurrentItem(Treats[CurrentTreat]);
            // };
            // Server.Server.Instance.InputActionMap["PrevItem"].performed += context => {
            //     CurrentItem--;
            //     if (CurrentItem < 0) CurrentItem = ItemList.Count - 1;
            //     ItemList[CurrentItem].TakeOut();
            //     
            //     /* Hale switch to treat */
            //     if (CurrentItem == 3) {
            //         SwitchTreat(Treats[CurrentTreat]);
            //         Server.Server.Instance.itemBar.SetCurrentItem(Treats[CurrentTreat]);
            //     }
            // };
            // Server.Server.Instance.InputActionMap["NextItem"].performed += context => {
            //     CurrentItem++;
            //     if (CurrentItem >= ItemList.Count) CurrentItem = 0;
            //     ItemList[CurrentItem].TakeOut();
            //     
            //     /* Hale switch to treat */
            //     if (CurrentItem == 3) {
            //         SwitchTreat(Treats[CurrentTreat]);
            //         Server.Server.Instance.itemBar.SetCurrentItem(Treats[CurrentTreat]);
            //     }
            // };
        }

        protected void SwitchTreat(Items treat) {
            switch (treat) {
                case Items.Cookie:
                    Treat = Cookie;
                    CurrentTreat = 0;
                    break;
                case Items.Heart:
                    Treat = Heart;
                    CurrentTreat = 1;
                    break;
                case Items.Fish:
                    Treat = Fish;
                    CurrentTreat = 2;
                    break;
                default:
                    Debug.LogError("Invalid treat type!");
                    Treat = Cookie;
                    CurrentTreat = 1;
                    break;
            }
        }
    }
}
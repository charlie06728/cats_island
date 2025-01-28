using System;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player {
    public class PlayerPocket : MonoBehaviour {
        [NonSerialized] public Player Player;
        
        /* Album prefab and component after it being initialized */
        public GameObject albumPrefab;
        [NonSerialized] public Album Album;
        
        /* Camera prefab and component after init */
        public GameObject cameraPrefab;
        [NonSerialized] public CameraObject CameraObject;
        
        /* treat prefab and component after it being initialized */
        public CameraObject treatPrefab;
        [NonSerialized] public Treat Treat;

        protected void Awake() {
            /* Initialize the album */
            GameObject albumObj = Instantiate(albumPrefab, transform);
            Album = albumObj.GetComponent<Album>();
            if (Album == null) throw new Exception("Album prefab does not have an Album component!");
            Album.PlayerPocket = this;
            Album.gameObject.SetActive(false);
            
            /* initialize the camera */
            GameObject cameraObj = Instantiate(cameraPrefab, transform);
            CameraObject = cameraObj.GetComponent<CameraObject>();
            if (CameraObject == null) throw new Exception("Camera prefab does not have a CameraObject component!");
            CameraObject.PlayerPocket = this;
            CameraObject.gameObject.SetActive(false);
            
            /* Define the item switch behaviour */
            Server.Server.Instance.InputActionMap["1"].performed += context => { CameraObject.TakeOut(); };
            Server.Server.Instance.InputActionMap["2"].performed += context => { Album.TakeOut(); };
            Server.Server.Instance.InputActionMap["3"].performed += context => { Treat.TakeOut(); };
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player {
    /* The camera objet */
    public class CameraObject : Item {
        public Camera photoCamera;
        public GameObject photoPrefab;
        [NonSerialized] public Photo CurrentPhoto;
        private RenderTexture _renderTexture;

        public void TakePhoto() {
            // Render the photo camera
            photoCamera.Render();

            // Copy the RenderTexture to the Texture2D
            RenderTexture.active = _renderTexture;
            CurrentPhoto.PhotoTexture.ReadPixels(
                new Rect(0, 0, Server.Server.Instance.photoWidth, Server.Server.Instance.photoHeight), 0, 0);
            CurrentPhoto.PhotoTexture.Apply();
            RenderTexture.active = null;

            // Apply the captured texture to the plane-like object
            if (CurrentPhoto.PhotoRenderer != null) {
                CurrentPhoto.PhotoRenderer.material.mainTexture = CurrentPhoto.PhotoTexture;
            }

            Debug.Log("Photo captured and displayed!");
        }
        
        protected void LoadFilm() {
            /* Instantiate the photo prefab */
            GameObject photoObject = Instantiate(photoPrefab, transform.position, transform.rotation);
            CurrentPhoto = photoObject.GetComponent<Photo>();
            if (CurrentPhoto == null) CurrentPhoto = photoObject.AddComponent<Photo>();
        }

        protected void Awake() {
            photoCamera = GetComponent<Camera>();
            if (photoCamera == null) photoCamera = gameObject.AddComponent<Camera>();
            /* photo camera same rotation as camera object */
            photoCamera.transform.rotation = transform.rotation;
            
            /* Set up the camera */
            _renderTexture =
                new RenderTexture(Server.Server.Instance.photoHeight, Server.Server.Instance.photoWidth, 24);
            photoCamera.targetTexture = _renderTexture;
            
            /* Load the film */
            LoadFilm();
        }
    }
}
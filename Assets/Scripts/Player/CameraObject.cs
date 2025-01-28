using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player {
    /* The camera objet */
    public class CameraObject : Item {
        public Camera photoCamera;
        public GameObject photoPrefab;
        [NonSerialized] public Photo CurrentPhoto;
        private RenderTexture _renderTexture;
        
        private Action<InputAction.CallbackContext> _takePhotoAction;

        public override void TakeOut() {
            base.TakeOut();
            
            /* define the input action behaviours */
            Server.Server.Instance.InputActionMap["LeftMouse"].performed += _takePhotoAction;
        }
        
        public override void PutBack() {
            base.PutBack();
            
            /* define the input action behaviours */
            Server.Server.Instance.InputActionMap["LeftMouse"].performed -= _takePhotoAction;
        }

        public void TakePhoto() {
            /* Enable the current photo */
            CurrentPhoto.gameObject.SetActive(true);
            
            // Render the photo camera
            photoCamera.Render();

            // Copy the RenderTexture to the Texture2D
            RenderTexture.active = _renderTexture;
            CurrentPhoto.PhotoTexture.ReadPixels(
                new Rect(0, 0, Server.Server.Instance.photoHeight, Server.Server.Instance.photoWidth), 0, 0);
            CurrentPhoto.PhotoTexture.Apply();
            RenderTexture.active = null;

            // Apply the captured texture to the plane-like object
            if (CurrentPhoto.PhotoRenderer != null) {
                CurrentPhoto.PhotoRenderer.material.mainTexture = CurrentPhoto.PhotoTexture;
            }
            
            PlayerPocket.Album.Photos.Add(CurrentPhoto);
            CurrentPhoto.gameObject.SetActive(false);
            
            LoadFilm();

            Debug.Log("Photo captured and displayed!");
        }
        
        protected void LoadFilm() {
            /* Instantiate the photo prefab */
            GameObject photoObject = Instantiate(photoPrefab, transform.position, transform.rotation);
            CurrentPhoto = photoObject.GetComponent<Photo>();
            if (CurrentPhoto == null) CurrentPhoto = photoObject.AddComponent<Photo>();
            
            /* disable the current photo so cannot be seen */
            CurrentPhoto.gameObject.SetActive(false);            
        }

        protected override void Awake() {
            base.Awake();
            _takePhotoAction = ctx => TakePhoto();
            
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
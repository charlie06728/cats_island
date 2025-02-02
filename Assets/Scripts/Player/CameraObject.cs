using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Player {
    /* The camera objet */
    public class CameraObject : Item {
        public Camera photoCamera;
        public GameObject photoPrefab;
        [NonSerialized] public Photo CurrentPhoto;
        public float zoom_max = 30f; // Max amount of zoom
        public float zoom_speed = 0.1f; // Speed at which scrolling zooms in/out
        GameObject canvas;
        MeshRenderer hand_renderer;
        MeshRenderer camera_renderer;
        MeshRenderer screen_renderer;
        Scrollbar zoomScroll;
        bool is_zoomed = false;
        private RenderTexture _renderTexture;
        private Action<InputAction.CallbackContext> _takePhotoAction;
        private Action<InputAction.CallbackContext> _toggleZoomAction;

        public override void TakeOut() {
            base.TakeOut();
            
            /* define the input action behaviours */
            Server.Server.Instance.InputActionMap["LeftMouse"].performed += _takePhotoAction;
            Server.Server.Instance.InputActionMap["RightMouse"].performed += _toggleZoomAction;
        }
        
        public override void PutBack() {
            base.PutBack();

            canvas.SetActive(false);

            if (is_zoomed) {
                // Reset the field of view, in case the camera is put away while zoomed
                ToggleZoom();
                Camera.main.fieldOfView = 60;
            }
            
            /* define the input action behaviours */
            Server.Server.Instance.InputActionMap["LeftMouse"].performed -= _takePhotoAction;
            Server.Server.Instance.InputActionMap["RightMouse"].performed -= _toggleZoomAction;
        }

        public void Update() {
            base.Update();
            // int isheld = Input.GetMouseButton(1) ? 1 : 0;
            // Camera.main.fieldOfView = 60 - zoom_amount * isheld;
            // is_zoomed = Input.GetMouseButton(1);
            // photoCamera.fieldOfView = Camera.main.fieldOfView;
            if (is_zoomed) {
                zoomScroll.value += Input.GetAxis("Mouse ScrollWheel") * zoom_speed; // I
                // Clamp scroll value
                if (zoomScroll.value > 1) {
                    zoomScroll.value = 1;
                } else if (zoomScroll.value < 0) {
                    zoomScroll.value = 0;
                }
                Camera.main.fieldOfView = 60 - zoom_max * zoomScroll.value;
                photoCamera.fieldOfView = 60 - zoom_max * zoomScroll.value;
            } else {
                Camera.main.fieldOfView = 60;
            }
        }

        void ToggleZoom() {
            // Activate the scroll bar and enable zooming
            Debug.Log("Toggle Zoom");
            is_zoomed = !is_zoomed;
            canvas.SetActive(is_zoomed);
            // hand.SetActive(!is_zoomed);
            hand_renderer.enabled = !is_zoomed;
            camera_renderer.enabled = !is_zoomed;
            // screen_renderer.enabled = !is_zoomed;
        }

        public void TakePhoto() {
            /* Enable the current photo */
            CurrentPhoto.gameObject.SetActive(true);
            
            // Render the photo camera
            // photoCamera.CopyFrom(Camera.main);
            photoCamera.fieldOfView = Camera.main.fieldOfView;
            photoCamera.Render();

            // Changed to take pictures via the main camera
            // Camera.main.Render();

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
            _toggleZoomAction = ctx => ToggleZoom();
            
            photoCamera = GetComponent<Camera>();
            if (photoCamera == null) photoCamera = gameObject.AddComponent<Camera>();
            /* photo camera same rotation as camera object */
            photoCamera.transform.rotation = transform.rotation;
            
            /* Set up the camera */
            _renderTexture =
                new RenderTexture(Server.Server.Instance.photoHeight, Server.Server.Instance.photoWidth, 24);
            photoCamera.targetTexture = _renderTexture;

            // Get the UI scrollbar for the camera zoom
            canvas = GameObject.Find("Canvas");
            zoomScroll = GameObject.FindWithTag("CameraScroll").GetComponent<Scrollbar>();
            hand_renderer = GameObject.Find("Hand").GetComponent<MeshRenderer>();
            camera_renderer = gameObject.GetComponent<MeshRenderer>();
            screen_renderer = this.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>();
            canvas.SetActive(false);
            
            /* Load the film */
            LoadFilm();
        }
    }
}
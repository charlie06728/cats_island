using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Player {
    /* The camera objet */
    public class CameraObject : Item {
        public Camera photoCamera;
        public Vector3 cameraScale;
        public GameObject photoPrefab;
        public AudioSource cameraSoundTakePicture;
        public LayerMask catLayerMask;
        [NonSerialized] public Photo CurrentPhoto;
        public float zoom_max = 30f; // Max amount of zoom
        public float zoom_speed = 0.5f; // Speed at which scrolling zooms in/out
        MeshRenderer hand_renderer;
        MeshRenderer camera_renderer;
        MeshRenderer screen_renderer;
        public Scrollbar zoomScroll;
        bool is_zoomed = false;
        private RenderTexture _renderTexture;
        private Action<InputAction.CallbackContext> _takePhotoAction;
        private Action<InputAction.CallbackContext> _toggleZoomAction;
        private Action<InputAction.CallbackContext> _enterCameraModeAction;
        private Action<InputAction.CallbackContext> _exitCameraModeAction;
        
        private bool _isCameraMode = false;

        private Vector3 _cameraLocalPositionToItemHook;
        private Quaternion _cameraLocalRotationToItemHook;
        
        /* child mesh renderers */
        private MeshRenderer[] meshRenderers;

        public override void TakeOut() {
            base.TakeOut();
            
            /* define the input action behaviours */
            Server.Server.Instance.InputActionMap["LeftMouse"].performed += _takePhotoAction;
            Server.Server.Instance.InputActionMap["RightMouse"].performed += _enterCameraModeAction;
            Server.Server.Instance.InputActionMap["RightMouse"].canceled += _exitCameraModeAction;
            
            /* Record local position */
            _cameraLocalPositionToItemHook = transform.localPosition;
            _cameraLocalRotationToItemHook = transform.localRotation;
            
            SetMeshRendering(true);
        }
        
        public override void PutBack() {
            base.PutBack();
            

            if (_isCameraMode) {
                // // Reset the field of view, in case the camera is put away while zoomed
                // ToggleZoom();
                // Camera.main.fieldOfView = 60;
                ExitCameraMode();
            }
            
            /* define the input action behaviours */
            Server.Server.Instance.InputActionMap["LeftMouse"].performed -= _takePhotoAction;
            Server.Server.Instance.InputActionMap["RightMouse"].performed -= _enterCameraModeAction;
            Server.Server.Instance.InputActionMap["RightMouse"].canceled -= _exitCameraModeAction;
        }

        public void Update() {
            base.Update();
            // int isheld = Input.GetMouseButton(1) ? 1 : 0;
            // Camera.main.fieldOfView = 60 - zoom_amount * isheld;
            // is_zoomed = Input.GetMouseButton(1);
            // photoCamera.fieldOfView = Camera.main.fieldOfView;
            if (_isCameraMode) {
                zoomScroll.value += Input.GetAxis("Mouse ScrollWheel") * zoom_speed; // I
                // Clamp scroll value
                if (zoomScroll.value > 1) {
                    zoomScroll.value = 1;
                } else if (zoomScroll.value < 0) {
                    zoomScroll.value = 0;
                }
                Camera.main.fieldOfView = 60 - zoom_max * zoomScroll.value;
                photoCamera.fieldOfView = 60 - zoom_max * zoomScroll.value;
            }
        }

        void ToggleZoom() {
            // Activate the scroll bar and enable zooming
            Debug.Log("Toggle Zoom");
            is_zoomed = !is_zoomed;
            // hand.SetActive(!is_zoomed);
            hand_renderer.enabled = !is_zoomed;
            camera_renderer.enabled = !is_zoomed;
            screen_renderer.enabled = !is_zoomed;
        }

        public void TakePhoto() {
            /* Record the camera position */
            Vector3 cameraLocalPosition = photoCamera.transform.localPosition;
            Quaternion cameraLocalRotation = photoCamera.transform.localRotation;
            
            /* Set photo camera to have the same rect as main camera if zoomed in */
            if (_isCameraMode) {
                photoCamera.transform.position = Camera.main.transform.position;
                photoCamera.transform.rotation = Camera.main.transform.rotation;
            }
            
            /* Play sound */
            cameraSoundTakePicture.Play();
            
            /* Enable the current photo */
            CurrentPhoto.gameObject.SetActive(true);
            
            // Render the photo camera
            // photoCamera.CopyFrom(Camera.main);
            photoCamera.fieldOfView = Camera.main.fieldOfView;
            photoCamera.Render();

            // Copy the RenderTexture to the Texture2D
            // RenderTexture.active = _renderTexture;
            // CurrentPhoto.PhotoTexture.ReadPixels(
            //     new Rect(0, 0, Server.Server.Instance.photoWidth, Server.Server.Instance.photoHeight), 0, 0);
            // CurrentPhoto.PhotoTexture.Apply();
            // RenderTexture.active = null;
            StartCoroutine(CapturePhoto());

            // Apply the captured texture to the plane-like object
            // if (CurrentPhoto.PhotoRenderer != null) {
            //     CurrentPhoto.PhotoRenderer.material.mainTexture = CurrentPhoto.PhotoTexture;
            // }
            
            // PlayerPocket.Album.Photos.Add(CurrentPhoto);
            // CurrentPhoto.gameObject.SetActive(false);
            //
            // LoadFilm();
            
            /* Set back the photo camera location */
            if (_isCameraMode) {
                photoCamera.transform.localPosition = cameraLocalPosition;
                photoCamera.transform.localRotation = cameraLocalRotation;
            }

            Debug.Log("Photo captured and displayed!");
        }
        
        IEnumerator CapturePhoto()
        {
            /* Find the cats with view */
            List<Cat.Cat> catsInView = new List<Cat.Cat>();
            
            // Calculate camera frustum
            Camera camera = Camera.main;
            Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(camera);
            
            /* Get all cat game objects */
            List<GameObject> cats = new List<GameObject>();
            foreach (var cat in Server.Server.Instance.Cats) { cats.Add(cat.gameObject); }

            /* Iterate through the cats and see if within the frustum */
            foreach (GameObject obj in cats) {
                if (((1 << obj.layer) & catLayerMask) == 0) continue;
                
                // Get all renderers in this object and its children
                Renderer[] childRenderers = obj.GetComponentsInChildren<Renderer>();

                foreach (Renderer renderer in childRenderers) {
                    if (GeometryUtility.TestPlanesAABB(frustumPlanes, renderer.bounds))
                    {
                        Debug.Log($"{renderer.gameObject.name} is visible.");
                        catsInView.Add(renderer.gameObject.GetComponent<Cat.Cat>());
                        break;
                    }
                }
            }
            
            /* Set the cats in view */
            CurrentPhoto.Cats = catsInView;
            
            yield return new WaitForEndOfFrame(); // Ensures rendering is completed

            RenderTexture.active = _renderTexture;
            CurrentPhoto.PhotoTexture.ReadPixels(
                new Rect(0, 0, Server.Server.Instance.photoWidth, Server.Server.Instance.photoHeight), 0, 0);
            CurrentPhoto.PhotoTexture.Apply();
            RenderTexture.active = null;
            
            if (CurrentPhoto.PhotoRenderer != null) {
                CurrentPhoto.PhotoRenderer.material.mainTexture = CurrentPhoto.PhotoTexture;
            }
            
            PlayerPocket.Album.Photos.Add(CurrentPhoto);
            CurrentPhoto.gameObject.SetActive(false);
            
            LoadFilm();
        }
        
        protected void LoadFilm() {
            /* Instantiate the photo prefab */
            GameObject photoObject = Instantiate(photoPrefab, transform.position, transform.rotation);
            CurrentPhoto = photoObject.GetComponent<Photo>();
            if (CurrentPhoto == null) CurrentPhoto = photoObject.AddComponent<Photo>();
            
            /* disable the current photo so cannot be seen */
            CurrentPhoto.gameObject.SetActive(false);            
        }

        protected void EnterCameraMode() {
            _isCameraMode = true;
            StartCoroutine(EnterCameraModeCoroutine(0.25f));
        }

        protected IEnumerator EnterCameraModeCoroutine(float moveTime = 0.25f) {
            /* Set parent to eye view */
            transform.SetParent(PlayerPocket.Player.EyeView.transform);

            transform.localScale = cameraScale;

            Vector3 destinationLocalPosition = PlayerPocket.cameraVirtualPosition.transform.localPosition;
            Vector3 positionDelta = destinationLocalPosition - transform.localPosition;
            Quaternion destinationLocalRotation = PlayerPocket.cameraVirtualPosition.transform.localRotation;
            
            float elapsedTime = 0;
            while (elapsedTime <= moveTime) {
                if (!_isCameraMode) break;
                transform.localPosition += positionDelta * Time.deltaTime / moveTime;
                transform.localRotation = Quaternion.Slerp(transform.localRotation, destinationLocalRotation, elapsedTime / moveTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            
            if (!_isCameraMode) yield break;
            
            /* Hand invisible */
            PlayerPocket.Player.Hand.SetActive(false);
            
            /* Disable camera object rendering */
            SetMeshRendering(false);
            hand_renderer.enabled = false;
            zoomScroll.gameObject.SetActive(true);
            Server.Server.Instance.cameraMode.SetActive(true);
        }

        protected void ExitCameraMode() {
            _isCameraMode = false;
            hand_renderer.enabled = true;
            
            /* Hand visible */
            PlayerPocket.Player.Hand.SetActive(true);
            
            /* enable mesh rendering */
            SetMeshRendering(true);
            StartCoroutine(ExitCameraCoroutine(0.25f));
        }

        protected IEnumerator ExitCameraCoroutine(float moveTime = 0.25f) {

            /* Disable the camera mode ui */
            DisableCameraModeUI();
            
            /* Clear Camera object parent and set back the position & rotation to item hook */
            transform.SetParent(PlayerPocket.Player.ItemHook.transform);
            // Vector3 positionDelta = PlayerPocket.Player.ItemHook.transform.position - hook.transform.position;
            // photoCamera.transform.position += positionDelta;
            // photoCamera.transform.rotation = PlayerPocket.Player.ItemHook.transform.rotation;
            
            /* enable mesh rendering */
            SetMeshRendering(true);
            hand_renderer.enabled = true;
            zoomScroll.gameObject.SetActive(false);
            Server.Server.Instance.cameraMode.SetActive(false);
            
            /* Reset the scroll and fov */
            zoomScroll.value = 0;
            Camera.main.fieldOfView = 60;
            photoCamera.fieldOfView = 60;
            
            transform.SetParent(PlayerPocket.Player.ItemHook.transform);
            
            transform.localScale = cameraScale;
            
            /* Need to smoothly move the camera to player hand hook */
            Vector3 positionDelta = _cameraLocalPositionToItemHook - transform.localPosition;
            
            /* Calculate the rotation delta */ 
            Quaternion startRotation = transform.localRotation;
            Quaternion rotationDelta = _cameraLocalRotationToItemHook * Quaternion.Inverse(startRotation);
            Quaternion destinationLocalRotation = rotationDelta * transform.localRotation;
            
            /* In move time, move the camera to destination */
            float elapsedTime = 0;
            while (elapsedTime <= moveTime) {
                if (_isCameraMode) yield break;
                transform.localPosition += positionDelta * Time.deltaTime / moveTime;
                transform.localRotation = Quaternion.Slerp(startRotation, destinationLocalRotation, elapsedTime / moveTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        protected void EnableCameraModeUI() {
            
        }
        
        protected void DisableCameraModeUI() {
            
        }

        protected override void Awake() {
            base.Awake();
            _takePhotoAction = ctx => TakePhoto();
            _toggleZoomAction = ctx => ToggleZoom();
            _enterCameraModeAction = ctx => EnterCameraMode();
            _exitCameraModeAction = ctx => ExitCameraMode();

            if (photoCamera == null) {
                Debug.Log("Photo camera is null!");
                throw new Exception("Photo camera is not set for camera object!");
            }
            // /* photo camera same rotation as camera object */
            // photoCamera.transform.rotation = transform.rotation;
            
            // /* Set up the camera */
            // _renderTexture =
            //     new RenderTexture(Server.Server.Instance.photoWidth, Server.Server.Instance.photoHeight, 24);
            // photoCamera.targetTexture = _renderTexture;
            _renderTexture = photoCamera.targetTexture;

            // Get the UI scrollbar for the camera zoom
            if (zoomScroll == null) zoomScroll = GameObject.FindWithTag("CameraScroll").GetComponent<Scrollbar>();
            if (hand_renderer == null) hand_renderer = GameObject.Find("Hand").GetComponent<MeshRenderer>();
            if (camera_renderer == null) camera_renderer = gameObject.GetComponent<MeshRenderer>();
            if (screen_renderer == null) screen_renderer = this.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>();
            zoomScroll.gameObject.SetActive(false);
            
            /* Load the film */
            LoadFilm();
            
            // Get all MeshRenderer components under this GameObject (including children)
            meshRenderers = GetComponentsInChildren<MeshRenderer>();
        }
        
        void SetMeshRendering(bool state)
        {
            foreach (MeshRenderer renderer in meshRenderers)
            {
                renderer.enabled = state;
            }
        }

        IEnumerator ReRenderAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            SetMeshRendering(true);
        }
    }
}
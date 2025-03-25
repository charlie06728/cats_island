using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using NUnit.Framework;
using UIs;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Plane = UnityEngine.Plane;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Player {
    /* The camera objet */
    public class CameraObject : Item {
        public AK.Wwise.Event cameraOn;
        public AK.Wwise.Event cameraOff;
        public Camera photoCamera;
        public Vector3 cameraScale;
        public GameObject photoPrefab;
        public AudioSource cameraSoundTakePicture;
        public LayerMask catLayerMask;
        [NonSerialized] public Photo CurrentPhoto;
        public float zoom_max = 30f; // Max amount of zoom
        public float zoom_speed = 0.5f; // Speed at which scrolling zooms in/out
        public float zoom_speed_controller = 1000f;
        
        public LayerMask cameraLayerMask;
        
        // MeshRenderer hand_renderer;
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

        private float _prevTakeTime = 0f;
        private Animator _snapAnimator;
        private CameraAnimation _cameraAnimation;
        
        /* child mesh renderers */
        private MeshRenderer[] meshRenderers;
        

        public override void TakeOut() {
            base.TakeOut();
            
            FixItemOnHook();
            
            /* define the input action behaviours */
            Server.Server.Instance.InputActionMap["LeftMouse"].performed += _takePhotoAction;
            Server.Server.Instance.InputActionMap["RightMouse"].performed += _enterCameraModeAction;
            Server.Server.Instance.InputActionMap["RightMouse"].canceled += _exitCameraModeAction;
            
            /* Record local position */
            _cameraLocalPositionToItemHook = transform.localPosition;
            _cameraLocalRotationToItemHook = transform.localRotation;
            
            SetMeshRendering(true);
            
            Server.Server.Instance.itemBar.SetCurrentItem(Items.Camera);
        }
        
        public override void PutBack() {
            

            if (_isCameraMode) {
                // // Reset the field of view, in case the camera is put away while zoomed
                // ToggleZoom();
                // Camera.main.fieldOfView = 60;
                ExitCameraMode();
            }
            
            base.PutBack();
            
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
                
                /* Read input ZoomIn and ZoomOut */
                if (Server.Server.Instance.InputActionMap["ZoomIn"].IsPressed()) {
                    zoomScroll.value -= zoom_speed_controller * Time.deltaTime;
                }
                if (Server.Server.Instance.InputActionMap["ZoomOut"].IsPressed()) {
                    zoomScroll.value += zoom_speed_controller * Time.deltaTime;
                }
                
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
            // hand_renderer.enabled = !is_zoomed;
            camera_renderer.enabled = !is_zoomed;
            screen_renderer.enabled = !is_zoomed;
        }

        public void TakePhoto() {
            if (Server.Server.Instance.FilmUsed >= Server.Server.Instance.FilmCount 
                || Time.time - _prevTakeTime < Server.Server.Instance.cameraCoolDown) return;
            Server.Server.Instance.FilmUsed++;
            _prevTakeTime = Time.time;
            
            if (_isCameraMode) {
                // PlayerPocket.cameraAnimator.speed = 1;
                // PlayerPocket.cameraAnimator.Play("CameraAnimation", 0, 0);
                _cameraAnimation.ShowImage();
                // _snapAnimator.SetTrigger("TakePhoto");
                _snapAnimator.Play("CameraAnimation", 0, 0f);
            }
            
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
            AkUnitySoundEngine.PostEvent("sfx_CameraPicture", gameObject);
            
            /* Enable the current photo */
            // CurrentPhoto.gameObject.SetActive(true);
            
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
            /* Set album notification */
            Server.Server.Instance.albumNotification.SetActive(true);
            
            CurrentPhoto.Stars = 0;
            
            /* Find the cats with view */
            List<Cat.Cat> catsInView = new List<Cat.Cat>();
            
            // foreach (Cat.Cat cat in Server.Server.Instance.Cats) {
            //
            //     bool isCatInView = false;
            //     int casthit = 0;
            //     foreach (GameObject obj in cat.castPoints) {
            //         /* Raycast from main camera to cat, can be blocked */
            //         RaycastHit hit;
            //         // catLayerMask = LayerMask.GetMask("Cat");
            //         Vector3 direction = obj.transform.position - Camera.main.transform.position;
            //         float distance = direction.magnitude; // Limit ray to cat's distance
            //         if (Physics.Raycast(Camera.main.transform.position, direction, out hit, distance)) {
            //             if ((1 << hit.transform.gameObject.layer & this.catLayerMask) != 0) {
            //                 isCatInView = true;
            //                 casthit++;
            //                 // catsInView.Add(cat);
            //             }
            //         }
            //     }
            //     
            //     if (isCatInView) {
            //         catsInView.Add(cat);
            //         Debug.Log($"Cat star: {casthit}");
            //         CurrentPhoto.Stars += casthit;
            //         if (CurrentPhoto.Stars > 5) CurrentPhoto.Stars = 5;
            //     }
            // }
            
            // Calculate camera frustum
            Camera camera = Camera.main;
            Plane[] frustumPlanes = GeometryUtility.CalculateFrustumPlanes(camera);
            
            // /* Get all cat game objects */
            // List<GameObject> cats = new List<GameObject>();
            // foreach (var cat in Server.Server.Instance.Cats) { cats.Add(cat.gameObject); }
            
            Cat.Cat finalCat = null;
            float finalDistance = 25;
            
            /* Iterate through the cats and see if within the frustum */
            foreach (Cat.Cat cat in Server.Server.Instance.Cats) {
                GameObject obj = cat.gameObject;
                
                /* Cast a ray between main camera and cat to see if being blocked by terrains */
                RaycastHit cameraHit;
                Vector3 rayDirection = obj.transform.position - camera.transform.position;
                if (Physics.Raycast(camera.transform.position, rayDirection, out cameraHit, rayDirection.magnitude, layerMask:cameraLayerMask)) {
                    continue;
                }

                CurrentPhoto.Stars = 0;
                
                if (((1 << obj.layer) & catLayerMask) == 0) continue;
                
                // Get all renderers in this object and its children
                Renderer[] childRenderers = obj.GetComponentsInChildren<Renderer>();
            
                int casthit = 0;
                foreach (Renderer renderer in childRenderers) {
                    if (GeometryUtility.TestPlanesAABB(frustumPlanes, renderer.bounds)) {
                        casthit++;
                        // foreach (GameObject castPoint in cat.castPoints) {
                        //     RaycastHit hit;
                        //     Vector3 direction = castPoint.transform.position - camera.transform.position;
                        //     float distance = direction.magnitude; // Limit ray to cat's distance
                            // if (Physics.Raycast(camera.transform.position, direction, out hit, distance)) {
                            //     /* If the hit object has cat layer, means hit */
                            //     if ((1 << hit.transform.gameObject.layer & catLayerMask) != 0) {
                            //         casthit++;
                            //     }
                            // }
                        // }
                        
                        // Debug.Log($"{renderer.gameObject.name} is visible.");
                        // catsInView.Add(obj.gameObject.GetComponent<Cat.Cat>());
                        // break;
                    }
                    
                    if (casthit >= childRenderers.Length / 2 && casthit > 0) {
                        // catsInView.Add(cat);
                        // finalCat = cat;
                        // CurrentPhoto.Stars += casthit;
                        Debug.Log($"Cat star: {casthit}");
                            
                        /* Calculate the distance between main camera and cat */
                        float d = Vector3.Distance(camera.transform.position, obj.transform.position);
                        if (d < finalDistance) {
                            finalDistance = d;
                            finalCat = cat;
                        } else continue;
                    }
                }
            }
            
            if (finalCat != null) catsInView.Add(finalCat);
            
            /* Check if the cat is captured before */
            if (finalCat != null && !Brochure.CollectedCats.Contains(finalCat.catBreed)) {
                Brochure.CollectedCats.Add(finalCat.catBreed);
                Server.Server.Instance.newCatNotification.SetActive(true);

                if (Brochure.CollectedCats.Count == Server.Server.Instance.CatDictionary.Count) {
                    AkUnitySoundEngine.PostEvent("mus_AllCatsFound", gameObject);
                } else {
                    string catBreed = finalCat.catBreed.ToLower();
                    switch (catBreed) {
                        case "ragdoll":
                            AkUnitySoundEngine.PostEvent("mus_Ragdoll_Pose", gameObject);
                            break;
                        case "grey cat":
                            AkUnitySoundEngine.PostEvent("mus_GreyCat_Pose", gameObject);
                            break;
                        case "grey tabby":
                            AkUnitySoundEngine.PostEvent("mus_Tabby_Pose", gameObject);
                            break;
                        case "tuxedo":
                            AkUnitySoundEngine.PostEvent("mus_Tuxedo_Pose", gameObject);
                            break;
                        case "bengal":
                            AkUnitySoundEngine.PostEvent("mus_Bengal_Pose", gameObject);
                            break;
                        default:
                            AkUnitySoundEngine.PostEvent("mus_Ragdoll_Pose", gameObject);
                            break;
                    }
                }
            }
            
            /* Set the cats in view */
            CurrentPhoto.Cats.AddRange(catsInView);
            CurrentPhoto.Stars = 0;
            CurrentPhoto.Stars += CalculateStar(CurrentPhoto);
            if (CurrentPhoto.Stars > 1) CurrentPhoto.Stars = 1;
            Server.Server.Instance.StarCount += CurrentPhoto.Stars;
            
            yield return new WaitForEndOfFrame(); // Ensures rendering is completed

            RenderTexture.active = _renderTexture;
            CurrentPhoto.PhotoTexture.ReadPixels(
                new Rect(0, 0, Server.Server.Instance.photoWidth, Server.Server.Instance.photoHeight), 0, 0);
            CurrentPhoto.PhotoTexture.Apply();
            RenderTexture.active = null;
            
            // 🔹 Apply gamma correction (Convert Linear to sRGB)
            Color[] pixels = CurrentPhoto.PhotoTexture.GetPixels();
            for (int i = 0; i < pixels.Length; i++) {
                pixels[i] = pixels[i].gamma; // Converts from Linear space to sRGB
            }
            CurrentPhoto.PhotoTexture.SetPixels(pixels);
            CurrentPhoto.PhotoTexture.Apply();
            
            // if (CurrentPhoto.PhotoRenderer != null) {
            //     CurrentPhoto.PhotoRenderer.material.mainTexture = CurrentPhoto.PhotoTexture;
            // }
            
            PlayerPocket.Album.Photos.Add(CurrentPhoto);
            // CurrentPhoto.gameObject.SetActive(false);
            
            LoadFilm();
        }
        
        protected void LoadFilm() {
            /* Instantiate the photo prefab */
            // GameObject photoObject = Instantiate(photoPrefab, transform.position, transform.rotation);
            // CurrentPhoto = photoObject.GetComponent<Photo>();
            // if (CurrentPhoto == null) CurrentPhoto = photoObject.AddComponent<Photo>();
            //
            // /* disable the current photo so cannot be seen */
            // CurrentPhoto.gameObject.SetActive(false);
            CurrentPhoto = new Photo();
        }

        protected void EnterCameraMode() {
            _isCameraMode = true;
            Server.Server.Instance.itemBar.gameObject.SetActive(false);

            /* Instantiate and set active all components */
            _snapAnimator = GameObject.Instantiate(PlayerPocket.cameraAnimationPrefab, Server.Server.Instance.canvas.transform).GetComponent<Animator>();
            _cameraAnimation = _snapAnimator.gameObject.GetComponent<CameraAnimation>();
            _cameraAnimation.HideImage();
            _snapAnimator.gameObject.SetActive(true);
            _snapAnimator.Play("Idle", 0, 0);
            
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
            // hand_renderer.enabled = false;
            zoomScroll.gameObject.SetActive(true);
            Server.Server.Instance.cameraMode.SetActive(true);
            
            /* Post caemra on sfx */
            cameraOn.Post(gameObject);
        }

        protected void ExitCameraMode() {
            _isCameraMode = false;
            // hand_renderer.enabled = true;
            
            _cameraAnimation.HideImage();
            _snapAnimator.SetTrigger("Exit");
            Destroy(_snapAnimator.gameObject);
            _snapAnimator = null;
            
            /* Hand visible */
            PlayerPocket.Player.Hand.SetActive(true);
            
            /* item bar visible */
            Server.Server.Instance.itemBar.gameObject.SetActive(true);
            
            /* enable mesh rendering */
            SetMeshRendering(true);
            StartCoroutine(ExitCameraCoroutine(0.25f));
        }

        protected int CalculateStar(Photo photo) {
            int star = 0;
            // star += photo.Cats.Count;

            foreach (Cat.Cat cat in photo.Cats) {
                if (cat.Behaviour.State == Cat.CatState.Pose) {
                    star++;
                }
            }

            return star;
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
            // hand_renderer.enabled = true;
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
            
            /* Post camera off sfx */
            cameraOff.Post(gameObject);
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
            // if (hand_renderer == null) hand_renderer = GameObject.Find("Hand").GetComponent<MeshRenderer>();
            if (camera_renderer == null) camera_renderer = gameObject.GetComponent<MeshRenderer>();
            if (screen_renderer == null) screen_renderer = this.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>();
            zoomScroll.gameObject.SetActive(false);
            
            /* Load the film */
            LoadFilm();
            
            // Get all MeshRenderer components under this GameObject (including children)
            meshRenderers = GetComponentsInChildren<MeshRenderer>();

            AkUnitySoundEngine.RegisterGameObj(gameObject);
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
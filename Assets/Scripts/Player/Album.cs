using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Player {
    public class Album : Item {
        public List<AlbumSlot> albumSlots;
        public List<Photo> Photos = new List<Photo>();
        public List<RawImage> rawImages;
        public Vector3 albumScale = new Vector3(0.5f, 0.5f, 0.5f);
        
        /* Album view related */
        private Action<InputAction.CallbackContext> _albumViewAction;
        private Action<InputAction.CallbackContext> _exitAlbumViewAction;
        private Action<InputAction.CallbackContext> _nextPage;
        private Action<InputAction.CallbackContext> _prevPage;
        private Action<InputAction.CallbackContext> _up;
        private Action<InputAction.CallbackContext> _down;
        private Action<InputAction.CallbackContext> _left;
        private Action<InputAction.CallbackContext> _right;
        private Action<InputAction.CallbackContext> _delete;
        
        private Action<InputAction.CallbackContext> _gamePad;
        
        /* record alum local positions */
        private Vector3 _albumLocalPositionToItemHook;
        private Quaternion _albumLocalRotationToItemHook;

        public GameObject nextPrompt;
        public GameObject prevPrompt;

        [NonSerialized] public int CurrentSelect;

        [NonSerialized] public int CurrentStartIndex = 0;
        
        public override void TakeOut() {
            base.TakeOut();
            Server.Server.Instance.itemBar.SetCurrentItem(Items.Album);
            if (!gameObject.activeInHierarchy) return;
            
            /* Reset the notifications */
            Server.Server.Instance.albumNotification.SetActive(false);
            
            RenderImages();
            
            // int currentSlot = 0;
            // foreach (var photo in Photos) {
            //     if (currentSlot >= rawImages.Count) break;
            //     rawImages[currentSlot].texture = photo.PhotoTexture;
            //     currentSlot++;
            // }
            
            /* Define the album view behaviour */
            // Server.Server.Instance.InputActionMap["LeftMouse"].performed += _albumViewAction;
            // Server.Server.Instance.InputActionMap["RightMouse"].performed += _exitAlbumViewAction;
            Server.Server.Instance.InputActionMap["Next"].canceled += _nextPage;
            Server.Server.Instance.InputActionMap["Prev"].canceled += _prevPage;
            Server.Server.Instance.InputActionMap["W"].performed += _up;
            Server.Server.Instance.InputActionMap["S"].performed += _down;
            Server.Server.Instance.InputActionMap["A"].performed += _left;
            Server.Server.Instance.InputActionMap["D"].performed += _right;
            Server.Server.Instance.InputActionMap["Delete"].performed += _delete;
            Server.Server.Instance.InputActionMap["GamePadLeft"].performed += _gamePad;
            
            /* Suspend the player movement */
            Server.Server.Instance.SuspendPlayerMove = true;
            
            /* Record local position */
            _albumLocalPositionToItemHook = transform.localPosition;
            _albumLocalRotationToItemHook = transform.localRotation;
            
            transform.localScale = albumScale;
            
            FixItemOnHook();
            
            /* Enter album view immediately */
            EnterAlbumView();
        }

        protected void RenderImages() {
            /* Audio */
            AkUnitySoundEngine.PostEvent("sfx_pageflip", gameObject);
            
            HideAll();
            
            int endIndex = CurrentStartIndex + albumSlots.Count;
            if (endIndex > Photos.Count) {
                endIndex = Photos.Count;
            }
            
            for (int i = CurrentStartIndex; i < endIndex; i++) {
                int slotIndex = i % albumSlots.Count;
                if (slotIndex == CurrentSelect) {
                    albumSlots[slotIndex].select.SetActive(true);
                } else {
                    albumSlots[slotIndex].select.SetActive(false);
                }
                albumSlots[slotIndex].Show();
                albumSlots[slotIndex].SetPhoto(Photos[i]);
            }
            
            /* Check if the prompts needs to be displayed or hidden */
            if (CurrentStartIndex - albumSlots.Count < 0) {
                prevPrompt.SetActive(false);
            } else {
                prevPrompt.SetActive(true);
            }
            
            if (CurrentStartIndex + albumSlots.Count >= Photos.Count) {
                nextPrompt.SetActive(false);
            } else {
                nextPrompt.SetActive(true);
            }
        }

        protected override void Update() {
            for (int i = 0; i < albumSlots.Count; i++) {
                if (i == CurrentSelect) {
                    if (albumSlots[i].rawImage.gameObject.activeInHierarchy) albumSlots[i].select.SetActive(true);
                } else {
                    albumSlots[i].select.SetActive(false);
                }
            }
        }
        
        public override void PutBack() {
            base.PutBack();
            
            foreach (var slot in albumSlots) {
                slot.Hide();
            }
            
            transform.SetParent(PlayerPocket.Player.ItemHook.transform);
            
            /* Allow player move */
            Server.Server.Instance.SuspendPlayerMove = false;
            
            // /* Hide all the photos */
            // foreach (var photo in Photos) {
            //     photo.gameObject.SetActive(false);
            // }
            
            // Server.Server.Instance.InputActionMap["LeftMouse"].performed -= _albumViewAction;
            // Server.Server.Instance.InputActionMap["RightMouse"].performed -= _exitAlbumViewAction;
            Server.Server.Instance.InputActionMap["Next"].canceled -= _nextPage;
            Server.Server.Instance.InputActionMap["Prev"].canceled -= _prevPage;
            Server.Server.Instance.InputActionMap["W"].performed -= _up;
            Server.Server.Instance.InputActionMap["S"].performed -= _down;
            Server.Server.Instance.InputActionMap["A"].performed -= _left;
            Server.Server.Instance.InputActionMap["D"].performed -= _right;
            Server.Server.Instance.InputActionMap["Delete"].performed -= _delete;
            Server.Server.Instance.InputActionMap["GamePadLeft"].performed -= _gamePad;
        }

        protected void EnterAlbumView() {
            gameObject.SetActive(true);
            StartCoroutine(EnterAlbumViewCoroutine(0.2f));
        }

        protected IEnumerator EnterAlbumViewCoroutine(float moveTime = 0.2f) {
            /* Set parent to eye view */
            transform.SetParent(PlayerPocket.Player.EyeView.transform);

            transform.localScale = albumScale;

            Vector3 destinationLocalPosition = PlayerPocket.albumVirtualPosition.transform.localPosition;
            Vector3 positionDelta = destinationLocalPosition - transform.localPosition;
            Quaternion destinationLocalRotation = PlayerPocket.albumVirtualPosition.transform.localRotation;
            
            float elapsedTime = 0;
            while (elapsedTime <= moveTime) {
                transform.localPosition += positionDelta * Time.deltaTime / moveTime;
                transform.localRotation = Quaternion.Slerp(transform.localRotation, destinationLocalRotation, elapsedTime / moveTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        protected void ExitAlbumView() {
            StartCoroutine(ExitAlbumViewCoroutine(0.2f));
        }

        protected IEnumerator ExitAlbumViewCoroutine(float moveTime = 0.2f) {
            transform.SetParent(PlayerPocket.Player.ItemHook.transform);
            
            transform.localScale = albumScale;
            
            /* Need to smoothly move the album to player hand hook */
            Vector3 positionDelta = _albumLocalPositionToItemHook - transform.localPosition;
            
            /* Calculate the rotation delta */ 
            Quaternion startRotation = transform.localRotation;
            Quaternion rotationDelta = _albumLocalRotationToItemHook * Quaternion.Inverse(startRotation);
            Quaternion destinationLocalRotation = rotationDelta * transform.localRotation;
            
            /* In move time, move the album to destination */
            float elapsedTime = 0;
            while (elapsedTime <= moveTime) {
                transform.localPosition += positionDelta * Time.deltaTime / moveTime;
                transform.localRotation = Quaternion.Slerp(startRotation, destinationLocalRotation, elapsedTime / moveTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // transform.localPosition = _albumLocalPositionToItemHook;
            // transform.localRotation = _albumLocalRotationToItemHook;
        }
        
        protected void HideAll() {
            foreach (var slot in albumSlots) {
                slot.Hide();
            }
        }

        protected void PreviousPage() {
            int jumpInterval = albumSlots.Count;
            CurrentStartIndex -= jumpInterval;
            if (CurrentStartIndex < 0) {
                CurrentStartIndex = 0;
            }
            
            RenderImages();
        }
        
        protected void NextPage() {
            int jumpInterval = albumSlots.Count;
            if (CurrentStartIndex + jumpInterval >= Photos.Count) return;
            CurrentStartIndex += jumpInterval;
            RenderImages();
        }

        protected override void Awake() {
            base.Awake();
            
            _albumViewAction = context => { EnterAlbumView(); };
            _exitAlbumViewAction = context => { ExitAlbumView(); };
            _prevPage = context => { PreviousPage(); };
            _nextPage = context => { NextPage(); };
            _up = context => {
                if (CurrentSelect == 1 || CurrentSelect == 3) CurrentSelect -= 1;
            };
            _down = context => {
                if (CurrentSelect == 0 || CurrentSelect == 2) CurrentSelect += 1;
            };
            _left = context => {
                if (CurrentSelect == 2 || CurrentSelect == 3) CurrentSelect -= 2;
            };
            _right = context => {
                if (CurrentSelect == 0 || CurrentSelect == 1) CurrentSelect += 2;
            };
            _delete = context => {
                if (Photos.Count == 0) return;
                /* calculate the current selected photo index */
                int photoIndex = CurrentStartIndex + CurrentSelect;
                if (photoIndex >= Photos.Count) return;
                Photos.RemoveAt(photoIndex);
                Debug.Log("Delete photo at index " + photoIndex);
                RenderImages();
            };
            _gamePad = context => {
                Vector2 gamePad = Server.Server.Instance.InputActionMap["GamePadLeft"].ReadValue<Vector2>();
                if (gamePad.x < -0.9) {
                    if (CurrentSelect == 2 || CurrentSelect == 3) CurrentSelect -= 2;
                } else if (gamePad.x > 0.9) {
                    if (CurrentSelect == 0 || CurrentSelect == 1) CurrentSelect += 2;
                } else if (gamePad.y < -0.9) {
                    if (CurrentSelect == 1 || CurrentSelect == 3) CurrentSelect -= 1;
                } else if (gamePad.y > 0.9) {
                    if (CurrentSelect == 0 || CurrentSelect == 2) CurrentSelect += 1;
                }
            };
        }

        public void ResetAlbum() {
            // Reset progress
            Photos = new List<Photo>();
        }
    }
}
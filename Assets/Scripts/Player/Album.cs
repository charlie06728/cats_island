using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Player {
    public class Album : Item {
        public List<Photo> Photos = new List<Photo>();
        public List<RawImage> rawImages;
        public Vector3 albumScale = new Vector3(0.5f, 0.5f, 0.5f);
        
        /* Album view related */
        private Action<InputAction.CallbackContext> _albumViewAction;
        private Action<InputAction.CallbackContext> _exitAlbumViewAction;
        
        /* record alum local positions */
        private Vector3 _albumLocalPositionToItemHook;
        private Quaternion _albumLocalRotationToItemHook;
        
        public override void TakeOut() {
            base.TakeOut();
            
            int currentSlot = 0;
            foreach (var photo in Photos) {
                if (currentSlot >= rawImages.Count) break;
                rawImages[currentSlot].texture = photo.PhotoTexture;
                currentSlot++;
            }
            
            /* Define the album view behaviour */
            Server.Server.Instance.InputActionMap["LeftMouse"].performed += _albumViewAction;
            Server.Server.Instance.InputActionMap["RightMouse"].performed += _exitAlbumViewAction;
            
            /* Record local position */
            _albumLocalPositionToItemHook = transform.localPosition;
            _albumLocalRotationToItemHook = transform.localRotation;
            
            transform.localScale = albumScale;
        }
        
        public override void PutBack() {
            base.PutBack();
            
            transform.SetParent(PlayerPocket.Player.ItemHook.transform);
            
            // /* Hide all the photos */
            // foreach (var photo in Photos) {
            //     photo.gameObject.SetActive(false);
            // }
            Server.Server.Instance.InputActionMap["LeftMouse"].performed -= _albumViewAction;
            Server.Server.Instance.InputActionMap["RightMouse"].performed -= _exitAlbumViewAction;
        }

        protected void EnterAlbumView() {
            StartCoroutine(EnterAlbumViewCoroutine(0.5f));
        }

        protected IEnumerator EnterAlbumViewCoroutine(float moveTime = 0.5f) {
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
            StartCoroutine(ExitAlbumViewCoroutine(0.5f));
        }

        protected IEnumerator ExitAlbumViewCoroutine(float moveTime = 0.5f) {
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

        protected override void Awake() {
            base.Awake();
            
            _albumViewAction = context => { EnterAlbumView(); };
            _exitAlbumViewAction = context => { ExitAlbumView(); };
        }
    }
}
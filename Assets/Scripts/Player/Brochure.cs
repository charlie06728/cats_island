using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Player {
    public class Brochure : Item {
        public RawImage rawImage1;
        public RawImage rawImage2;

        public TextMeshProUGUI catName1;
        public TextMeshProUGUI catName2;
        
        public TextMeshProUGUI catBreed1;
        public TextMeshProUGUI catBreed2;
        
        public TextMeshProUGUI preferSnack1;
        public TextMeshProUGUI preferSnack2;
        
        public TextMeshProUGUI habitat1;
        public TextMeshProUGUI habitat2;
        
        public GameObject prevPrompt;
        public GameObject nextPrompt;

        [NonSerialized] public int CurrentStartIndex = 0;
        private Action<InputAction.CallbackContext> _nextPage;
        private Action<InputAction.CallbackContext> _prevPage;

        private List<Cat.Cat> _allCats;
        
        // [NonSerialized] public Photo Photo1;
        // [NonSerialized] public Photo Photo2;
        
        public override void TakeOut() {
            if (gameObject.activeInHierarchy) {
                PutBack();
                return;
            }
            
            /* put all back first */
            PutBackAll();

            /* Set item visible */
            gameObject.SetActive(true);
            
            HideAll();
            DisplayPhotos();
            
            /* Define the switch page behaviour */
            Server.Server.Instance.InputActionMap["Next"].performed += _nextPage;
            Server.Server.Instance.InputActionMap["Prev"].performed += _prevPage;
        }
        
        public override void PutBack() {
            HideAll();
            
            /* make item invisible */
            gameObject.SetActive(false);
        }

        protected override void Awake() {
            base.Awake();
            
            _allCats = Server.Server.Instance.CatDictionary.Values.ToList();

            _nextPage = ctx => {
                if (CurrentStartIndex + 2 < _allCats.Count) {
                    CurrentStartIndex += 2;
                    DisplayPhotos();
                }
            };
            _prevPage = ctx => {
                if (CurrentStartIndex - 2 >= 0) {
                    CurrentStartIndex -= 2;
                    DisplayPhotos();
                }
            };
        }

        protected void DisplayPhotos() {
            HideAll();
            List<Cat.Cat> cats = _allCats;
            
            for (int i = 0; i < 2; i++) {
                if (CurrentStartIndex + i >= cats.Count) break;
                DisplayCat(cats[CurrentStartIndex + i], i);
            }
            
            /* Hide prompts if possible */
            if (CurrentStartIndex - 2 < 0) {
                prevPrompt.SetActive(false);
            } else {
                prevPrompt.SetActive(true);
            }
            
            if (CurrentStartIndex + 2 >= cats.Count) {
                nextPrompt.SetActive(false);
            } else {
                nextPrompt.SetActive(true);
            }
            
            // List<Photo> photos = PlayerPocket.Album.Photos;
            //
            // int photoCount = 0;
            // Photo prevP = null;
            // Photo currP = null;
            // foreach (Photo photo in photos) {
            //     if (photo.Cats.Count == 0) continue;
            //     if (currP == null || photo.Stars >= currP.Stars) {
            //         if (currP != null) prevP = currP;
            //         currP = photo;
            //     }
            // }
            //
            // DisplayPhoto(currP, 0);
            // DisplayPhoto(prevP, 1);
        }

        protected void DisplayCat(Cat.Cat cat, int displayIndex) {
            if (displayIndex == 0) {
                rawImage1.gameObject.SetActive(true);
                catName1.gameObject.SetActive(true);
                catBreed1.gameObject.SetActive(true);
                preferSnack1.gameObject.SetActive(true);
                habitat1.gameObject.SetActive(true);

                rawImage1.texture = cat.catImage.mainTexture;
                catName1.text = cat.catName;
                catBreed1.text = cat.catBreed;
                
                preferSnack1.text = cat.catPreferredSnack;
                habitat1.text = cat.catHabitat;
            } else {
                rawImage2.gameObject.SetActive(true);
                catName2.gameObject.SetActive(true);
                catBreed2.gameObject.SetActive(true);
                preferSnack2.gameObject.SetActive(true);
                habitat2.gameObject.SetActive(true);

                rawImage2.texture = cat.catImage.mainTexture;
                catName2.text = cat.catName;
                catBreed2.text = cat.catBreed;
                
                preferSnack2.text = cat.catPreferredSnack;
                habitat2.text = cat.catHabitat;
            }
        }

        protected void HideAll() {
            rawImage1.gameObject.SetActive(false);
            rawImage2.gameObject.SetActive(false);
            catName1.gameObject.SetActive(false);
            catName2.gameObject.SetActive(false);
            catBreed1.gameObject.SetActive(false);
            catBreed2.gameObject.SetActive(false);
            preferSnack1.gameObject.SetActive(false);
            preferSnack2.gameObject.SetActive(false);
            habitat1.gameObject.SetActive(false);
            habitat2.gameObject.SetActive(false);
        }
        
        protected void ShowAll() {
            rawImage1.gameObject.SetActive(true);
            rawImage2.gameObject.SetActive(true);
            catName1.gameObject.SetActive(true);
            catName2.gameObject.SetActive(true);
            catBreed1.gameObject.SetActive(true);
            catBreed2.gameObject.SetActive(true);
            preferSnack1.gameObject.SetActive(true);
            preferSnack2.gameObject.SetActive(true);
            habitat1.gameObject.SetActive(true);
            habitat2.gameObject.SetActive(true);
        }
    }
}
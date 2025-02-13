using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class Album : Item {
        public List<Photo> Photos = new List<Photo>();
        
        /* four slots for photos */
        public GameObject Slot1;
        public GameObject Slot2;
        public GameObject Slot3;
        public GameObject Slot4;
        protected GameObject[] Slots;
        
        public override void TakeOut() {
            base.TakeOut();
            
            int currentSlot = 0;
            foreach (var photo in Photos) {
                if (currentSlot >= Slots.Length) break;
                Slots[currentSlot].SetActive(true);
                photo.gameObject.SetActive(true);
                photo.transform.SetParent(Slots[currentSlot].transform);
                photo.transform.rotation = Slots[currentSlot].transform.rotation;
                
                /* Make photo fits within the plane by adjusting the scale */
                photo.transform.localScale = new Vector3(1f, 1f, 1f);
                photo.transform.localPosition = new Vector3(0f, 0f, 0f);
                photo.transform.localRotation = Quaternion.Euler(0, 0, 0);
                
                /* Set parent and rotate 180 degrees */
                photo.transform.SetParent(Slots[currentSlot].transform.parent);
                // photo.transform.Rotate(0f, 180f, 0f);
                photo.transform.localRotation = Quaternion.Euler(0, 180f, 0);
                
                /* Disable the slot */
                Slots[currentSlot].SetActive(false);
                
                currentSlot++;
            }
        }
        
        public override void PutBack() {
            base.PutBack();
            
            /* Hide all the photos */
            foreach (var photo in Photos) {
                photo.gameObject.SetActive(false);
            }
            
            /* Enable all the slots */
            foreach (var slot in Slots) {
                slot.SetActive(true);
                slot.transform.rotation = transform.rotation;
            }
        }

        protected override void Awake() {
            base.Awake();
            Slots = new[] {Slot1, Slot2, Slot3, Slot4};
            
        }
    }

    // public class PhotoSlot : MonoBehaviour {
    //     
    //     
    // }
}
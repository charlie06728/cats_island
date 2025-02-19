using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class Photo : MonoBehaviour {
        public Renderer PhotoRenderer;
        public int PhotoWidth; // Width of the captured photo
        public int PhotoHeight; // Height of the captured photo
        
        /* Brochure related */
        [NonSerialized] public int Stars;
        [NonSerialized] public List<Cat.Cat> Cats = new List<Cat.Cat>();
        
        /* Store the scene capture as Texture2D */
        [NonSerialized] public Texture2D PhotoTexture;

        protected void Awake() {
            /* Initialize the photo texture */
            PhotoTexture = new Texture2D(Server.Server.Instance.photoWidth,
                Server.Server.Instance.photoHeight, TextureFormat.RGB24, false);
        }
    }
}
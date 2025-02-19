using System;
using System.Collections.Generic;
using UnityEngine;

namespace Player {
    public class Photo {
        // public Renderer PhotoRenderer;
        public int PhotoWidth; // Width of the captured photo
        public int PhotoHeight; // Height of the captured photo
        
        /* Brochure related */
        public int Stars;
        public HashSet<Cat.Cat> Cats = new HashSet<Cat.Cat>();
        
        /* Store the scene capture as Texture2D */
        public Texture2D PhotoTexture;

        public Photo() {
            /* Initialize the photo texture */
            PhotoTexture = new Texture2D(Server.Server.Instance.photoWidth,
                Server.Server.Instance.photoHeight, TextureFormat.RGB24, false);
            
            PhotoWidth = Server.Server.Instance.photoWidth;
            PhotoHeight = Server.Server.Instance.photoHeight;
        }

        // protected void Awake() {
        //     /* Initialize the photo texture */
        //     PhotoTexture = new Texture2D(Server.Server.Instance.photoWidth,
        //         Server.Server.Instance.photoHeight, TextureFormat.RGB24, false);
        // }
    }
}
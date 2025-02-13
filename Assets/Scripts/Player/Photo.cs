using System;
using UnityEngine;

namespace Player {
    public class Photo : MonoBehaviour {
        public Renderer PhotoRenderer;
        public int PhotoWidth; // Width of the captured photo
        public int PhotoHeight; // Height of the captured photo
        
        /* Store the scene capture as Texture2D */
        [NonSerialized] public Texture2D PhotoTexture;

        protected void Awake() {
            /* Initialize the photo texture */
            PhotoTexture = new Texture2D(Server.Server.Instance.photoWidth,
                Server.Server.Instance.photoHeight, TextureFormat.RGB24, false);
        }
    }
}
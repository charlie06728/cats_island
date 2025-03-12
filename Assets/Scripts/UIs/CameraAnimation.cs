using UnityEngine;
using UnityEngine.UI;

namespace UIs {
    public class CameraAnimation : MonoBehaviour {
        private Image imageComponent;

        void Start()
        {
            imageComponent = GetComponent<Image>();
            SetTransparency(0); // Hide at start
        }

        // Called at animation start
        public void ShowImage()
        {
            SetTransparency(1); // Make visible
        }

        // Called at animation end
        public void HideImage()
        {
            SetTransparency(0); // Make transparent
        }

        private void SetTransparency(float alpha)
        {
            if (imageComponent != null) // For UI Image
            {
                Color color = imageComponent.color;
                color.a = alpha;
                imageComponent.color = color;
            }
        }
    }
}
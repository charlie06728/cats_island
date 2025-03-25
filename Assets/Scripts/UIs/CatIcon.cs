using UnityEngine;
using UnityEngine.UI;

public class CatIcon : MonoBehaviour
{
    public Sprite catsprite;
    private Image image;
    
    void Awake() {
        image = gameObject.GetComponent<Image>();
    }

    public void ToggleOn() {
        image.sprite = catsprite;
    }
}

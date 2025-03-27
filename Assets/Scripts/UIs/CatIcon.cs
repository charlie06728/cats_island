using UnityEngine;
using UnityEngine.UI;

public class CatIcon : MonoBehaviour
{
    public Sprite catSprite;
    public Sprite purrfectSprite;
    private Image image;
    
    void Awake() {
        image = gameObject.GetComponent<Image>();
    }

    public void ToggleOn() {
        image.sprite = catSprite;
    }

    public void TogglePurrfect() {
        image.sprite = purrfectSprite;
    }
}

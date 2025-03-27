using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlashNotif : MonoBehaviour
{
    private Image image;
    public float flashLength; // How long to we want the image to appear for?
    
    void Awake() {
        image = gameObject.GetComponent<Image>();
        // StartFlash();
    }

    IEnumerator Flash()
    {
        Color c = image.color;
        for (float alpha = 0f; alpha <= 1; alpha += 0.1f)
        {
            c.a = alpha;
            image.color = c;
            yield return null;
        }
        yield return new WaitForSeconds(flashLength);
        for (float alpha = 1f; alpha >= -0.1f; alpha -= 0.1f)
        {
            c.a = alpha;
            image.color = c;
            yield return null;
        }
    }

    public void StartFlash() {
        StartCoroutine(Flash());
    }
}

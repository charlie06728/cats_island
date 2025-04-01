using UnityEngine;

public class DayStart : MonoBehaviour
{
    void Awake()
    {
        Time.timeScale = 0;
    }

    void Update()
    {
        if (Input.anyKeyDown) {
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }
    }
}

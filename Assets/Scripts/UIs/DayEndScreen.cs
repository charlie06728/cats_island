using UnityEngine;
using TMPro;

public class DayEndScreen : MonoBehaviour
{
    public TMP_Text cats;
    public TMP_Text remaining;
    public Transform playerSpawn;
    public GameObject player;

    void OnEnable() {
        // set the text to the number of cats 
    }

    void Update()
    {
        if (Input.anyKeyDown) {
            Time.timeScale = 1;
            player.transform.position = playerSpawn.position;
            gameObject.SetActive(false);
        }
    }
}

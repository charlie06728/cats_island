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
            Server.Server.Instance.playerScript.Pocket.sfx_select.Post(Server.Server.Instance.playerScript.Pocket
                .gameObject);
            
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeCounter : MonoBehaviour
{
    public static TimeCounter instance;
    public TimeManager tm;
    private TMP_Text text;

    string mins;
    string hours;
    string ampm;

    void Start() {
        /* Singleton pattern */
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
        
        text = gameObject.GetComponent<TMP_Text>();
    }
    void Update()
    {   
        if (tm.hours > 12) {
            hours = (tm.hours % 12).ToString();
        } else {
            hours = tm.hours.ToString();
        }

        if (tm.hours >= 12) {
            ampm = " pm";
        } else {
            ampm = " am";
        }

        if (tm.minutes < 10) {
            mins = "0" + tm.minutes.ToString();
        } else {
            mins = tm.minutes.ToString();
        }

        text.text = hours + ":" + mins + ampm;
    }
}

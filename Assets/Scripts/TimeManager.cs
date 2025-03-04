using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    public int day_length; // How many real-world seconds is an in-game day?

    // public Color day_tint;
    // public Color night_tint;

    private float curr_time;
    // public int CurrTime {get {return curr_time;} set {curr_time = value; OnTimeChange(value);}}

    // private int minutes;
    // public int Minutes {get {return minutes;} set {minutes = value; OnMinuteChange(value);}}
    // private int hours;
    // public int Hours {get {return hours;} set {hours = value; OnHoursChange(value);}}
    // private int days;
    // public int Days {get {return days;} set {days = value; OnDaysChange(value);}}

    // public Transform light_t;
    


    // void Start() {
    //     light_t = this.gameObject.transform.GetChild(0);
    // }

    void Update()
    {
        curr_time += Time.deltaTime;
        gameObject.transform.rotation = Quaternion.Euler(180 * (curr_time / day_length), 0f, 0f);
        if (curr_time >= day_length) {
            curr_time = 0;
        }
    }

    // void OnTimeChange(int value) {
    //     light.transform.ro
    // }

    // private void OnMinuteChange(int value) {
    //     if (value >= 60) {
    //         Hours += 1;
    //         minutes = 0;
    //     }
    // }

    // private void OnHoursChange(int value) {
    //     if (value > 24) {
    //         Days += 1;
    //         hours = 0;
    //     }
    // } 

    private void OnDaysChange(int value) {
        // OnDaysChanged not implemented yet, this can be for displaying results screen when the day ends
        curr_time = 0;
    }
}

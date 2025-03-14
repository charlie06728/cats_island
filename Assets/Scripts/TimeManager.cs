using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    public float day_length; // How many real-world seconds is an in-game day?
    private float hour_length; // How long is an hour?

    private float curr_time;
    
    public int hours;
    public int minutes;
    public int days;
    

    void Start() {
        hour_length = day_length / 15f;
    }

    void Update()
    {
        curr_time += Time.deltaTime;
        gameObject.transform.rotation = Quaternion.Euler(180 * (curr_time / day_length), 0f, 0f);
        if (curr_time >= day_length) {
            DayChange();
        }
        // Sun rises at 6 am and sets at 9 pm
        // 15 total hours
        hours = 6 + Mathf.FloorToInt(curr_time / hour_length);
        minutes = Mathf.FloorToInt(60 * (curr_time % hour_length) / hour_length);
    }


    private void DayChange() {
        // OnDaysChanged not implemented yet, this can be for displaying results screen when the day ends
        days++;
        curr_time = 0;
    }
}

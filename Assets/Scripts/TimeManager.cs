using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class TimeManager : MonoBehaviour
{
    public float day_length; // How many real-world seconds is an in-game day?
    private float hour_length; // How long is an hour?

    private float curr_time;
    
    public int hours;
    public int minutes;
    
    public Image[] clock_images;
    

    void Start() {
        hour_length = day_length / 15f;
    }

    void Update()
    {
        curr_time += Time.deltaTime;
        gameObject.transform.rotation = Quaternion.Euler(180 * (curr_time / day_length), 0f, 0f);
        if (curr_time >= day_length) {
            curr_time = 0;
        }
        // Sun rises at 6 am and sets at 9 pm
        // 15 total hours
        hours = 6 + Mathf.FloorToInt(curr_time / hour_length);
        minutes = Mathf.FloorToInt(60 * (curr_time % hour_length) / hour_length);
        
        /* Decide which image to display out of 9 images */
        float progress = (hours - 6f) / 15f;
        int imageIndex = (int)(progress * 100f / (100f / 8f) + 0.5f);
        for (int i = 0; i < 9; i++) {
            clock_images[i].gameObject.SetActive(i == imageIndex);
        }
    }


    private void OnDaysChange(int value) {
        // OnDaysChanged not implemented yet, this can be for displaying results screen when the day ends
        curr_time = 0;
    }
}

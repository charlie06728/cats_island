using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    public float day_length; // How many real-world seconds is an in-game day?
    private float hour_length; // How long is an hour?

    private float curr_time;
    
    public int hours;
    public int minutes;
    public int days;
    
    public Image[] clock_images;

    public GameObject dayEndScreen;
    
    public float NormalizeTimeSound {
        get {
            return curr_time / day_length * 12f;
        }
    }

    private void Awake() {
        /* Singleton pattern */
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        hour_length = day_length / 15f;
        dayEndScreen.SetActive(false);
    }


    void Update()
    {
        curr_time += Time.deltaTime;
        gameObject.transform.rotation = Quaternion.Euler(180 * (curr_time / day_length), 0f, 0f);
        if (curr_time >= day_length) {
            curr_time = 0;
            days += 1;
            day_change();
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
        
        /* Normalized the time to a float between 0 and 12 */
        float normalized = curr_time / day_length * 12f;
    }


    private void day_change() {
        // OnDaysChanged not implemented yet, this can be for displaying results screen when the day ends
        curr_time = 0;
        Time.timeScale = 0;
        dayEndScreen.SetActive(true);
    }

    public void ResetTime() {
        // Reset the time to the beginning
        curr_time = 0;
        minutes = 0;
        hours = 0;
        days = 0;
    }

}



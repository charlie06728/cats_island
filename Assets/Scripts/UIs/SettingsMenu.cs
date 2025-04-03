using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Player {
    public class SettingsMenu : MonoBehaviour
    {
        public Slider volumeSlider;
        public TimeManager tm;
        public Transform playerTransform;
        public Transform playerSpawn;
        public CatIconManager im;
        public Brochure brochure;
        public PlayerPocket pocket;
        public AK.Wwise.RTPC masterVolume;

        private Action<InputAction.CallbackContext> _close;

        public void OnVolumeChanged (){
            // AkSoundEngine.SetRTPCValue("MasterVolume", volumeSlider.value);
            masterVolume.SetValue(gameObject, volumeSlider.value);
        }

        public void Show() {
            if (gameObject.activeInHierarchy) {
                Resume();
            } else {
                gameObject.SetActive(true);
                Time.timeScale = 0;
            
                /* Display the cursor */
                Cursor.visible = true;
            }
        }

        public void Update() {
            if (gameObject.activeInHierarchy) {
                /* Display the cursor */
                Cursor.visible = true;
            } else {
                /* Hide the cursor */
                Cursor.visible = false;
            }
        }

        public void Start() {
            /* Register game object to WWISE */
            AkUnitySoundEngine.RegisterGameObj(gameObject);
        }

        public void Resume() {
            Cursor.visible = false;
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }

        public void Restart() {
            tm.ResetTime();
            // playerTransform.position = playerSpawn.position;
            // brochure.ResetProgress();
            // im.ResetIcons();
            // pocket.Album.ResetAlbum();
            // pocket.Album.ResetAlbum();
            // PlayerPocket.Album.Photos = new List<Photo>();
        }
    }
}

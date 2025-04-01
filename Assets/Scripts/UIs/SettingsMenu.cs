using UnityEngine;
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

        

        public void OnVolumeChanged (){
            AkSoundEngine.SetRTPCValue("MasterVolume", volumeSlider.value);
        }

        public void Resume() {
            Time.timeScale = 1;
            gameObject.SetActive(false);
        }

        public void Restart() {
            tm.ResetTime();
            playerTransform.position = playerSpawn.position;
            brochure.ResetProgress();
            im.ResetIcons();
            pocket.Album.ResetAlbum();
            pocket.Album.ResetAlbum();
            // PlayerPocket.Album.Photos = new List<Photo>();
        }
    }
}

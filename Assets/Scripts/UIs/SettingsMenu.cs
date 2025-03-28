using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Slider volumeSlider;
    

	public void OnVolumeChanged (){
        AkSoundEngine.SetRTPCValue("MasterVolume", volumeSlider.value);
    }

    public void Resume() {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }
}

using System;
using UnityEngine;

namespace DefaultNamespace {
    public class SoundManager : MonoBehaviour {
        public AK.Wwise.Event amb;
        public AK.Wwise.Event snd;
        public AkEvent ambEvent;

        protected uint sndID;

        protected void Start() {
            amb.Post(gameObject);
            sndID = snd.Post(gameObject);
        }
        
        protected void Update() {
            /* Set RTPC */
            AKRESULT setRes = AkSoundEngine.SetRTPCValue("Time_Of_Day", TimeManager.Instance.NormalizeTimeSound);
            // int valueType = 1;
            // AKRESULT result = AkSoundEngine.GetRTPCValue("Time_Of_Day", null, sndID, out float timeOfDay, ref valueType);
            // Debug.Log("Time of day for WWISE : " + timeOfDay);
        }
    }
}
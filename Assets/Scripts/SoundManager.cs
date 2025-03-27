using System;
using UnityEngine;

namespace DefaultNamespace {
    public class SoundManager : MonoBehaviour {
        public AK.Wwise.Event amb;
        public AK.Wwise.Event snd;
        public AK.Wwise.RTPC timeOfDay;
        public AkEvent ambEvent;
        
        public AK.Wwise.Event sndEvent1;
        public AK.Wwise.Event sndEvent2;
        public AK.Wwise.Event sndEvent3;
        

        protected uint sndID;

        protected void Start() {
            amb.Post(gameObject);
            snd.Post(gameObject);
            sndEvent1.Post(gameObject);
            sndEvent2.Post(gameObject);
            sndEvent3.Post(gameObject);
        }
        
        protected void Update() {
            /* Set RTPC */
            
            // timeOfDay.SetValue(gameObject, TimeManager.Instance.NormalizeTimeSound);
            // AKRESULT setRes = AkSoundEngine.SetRTPCValue("Time_Of_Day", TimeManager.Instance.NormalizeTimeSound);
        }
    }
}
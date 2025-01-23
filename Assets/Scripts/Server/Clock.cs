namespace Server {

    /* Stores current time info */
    public struct Time {
        public int Second;
        public int Minute;
        public int Hour;
        public int Day;
    }
    
    /* Keep track of current time */
    public static class Clock {
        public static Time Time = new Time();

        public static void Update() {
            float timePassed = UnityEngine.Time.time * Server.Instance.GameSpeed;
            
            /* Calculate second, minute, hour, and day */
            Time.Second = (int)timePassed % 60;
            Time.Minute = (int)timePassed / 60 % 60;
            Time.Hour = (int)timePassed / 3600 % 24;
            Time.Day = (int)timePassed / 86400;
        }
    }
}
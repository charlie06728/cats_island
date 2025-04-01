using UnityEngine;

public class FilmNotifManager : MonoBehaviour
{
    public FlashNotif almostFull;
    public FlashNotif full;
    public int almostThreshold = 3; // How much film remaining counts as "almost full"?
    private bool isAlmostFull; // Have we flashed the "almost full" notif already?
    private bool isFull; // Have we flashed the "album full" notif already?

    
    void Awake() {
        isAlmostFull = false;
        isFull = false;
    }

    void Update()
    {
        int filmRemaining = Server.Server.Instance.FilmCount - Server.Server.Instance.FilmUsed;
        if (filmRemaining <= almostThreshold && !isAlmostFull) {
            isAlmostFull = true;
            almostFull.StartFlash();
        }
        if (filmRemaining == 0 && !isFull) {
            isFull = true;
            full.StartFlash();
        }

        if (filmRemaining > 0 && isFull) {
            isFull = false;
        }
        if (filmRemaining > almostThreshold && isAlmostFull) {
            isAlmostFull = false;
        }
    }
}

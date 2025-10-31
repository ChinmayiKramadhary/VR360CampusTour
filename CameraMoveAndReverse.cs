using UnityEngine;
using UnityEngine.Video;

public class CameraMoveAndReverse : MonoBehaviour
{
    public VideoPlayer videoPlayer;
    public float moveSpeed = 2f;
    public float rewindSpeed = 1f; // higher = faster rewind

    void Update()
    {
        // --- Move Right (R) ---
        if (Input.GetKey(KeyCode.L))
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime, Space.Self);

        // --- Move Left (L) ---
        if (Input.GetKey(KeyCode.R))
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.Self);

        // --- Rewind (B) ---
        if (Input.GetKey(KeyCode.B) && videoPlayer != null && videoPlayer.isPrepared)
        {
            videoPlayer.Pause(); // stop normal playback
            double newTime = videoPlayer.time - rewindSpeed * Time.deltaTime;
            if (newTime < 0) newTime = 0;
            videoPlayer.time = newTime;  // manually seek backward
        }
        else if (Input.GetKeyUp(KeyCode.B) && videoPlayer != null)
        {
            videoPlayer.Play(); // resume forward when you release B
        }
    }
}
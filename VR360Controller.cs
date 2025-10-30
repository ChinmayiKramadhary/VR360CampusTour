using UnityEngine;
using UnityEngine.Video;

public class VR360ControlledPlayer : MonoBehaviour
{
    [Header("References")]
    public VideoPlayer videoPlayer;         // Drag your VideoPlayer here
    public Transform cameraTransform;       // Drag Main Camera (or XR Origin Camera)
    public Transform playerRig;             // Drag XR Origin or parent of camera

    [Header("Settings")]
    public float rotationSpeed = 45f;       // Degrees per second for rotation
    public float moveSpeed = 0.5f;          // Movement speed for joystick/keyboard
    public bool isVRMode = false;           // Auto-detect VR if available

    private void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.playOnAwake = false;
            videoPlayer.Pause();
        }

        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

#if UNITY_ANDROID && !UNITY_EDITOR
        isVRMode = true; // if on headset
#endif
    }

    private void Update()
    {
        if (videoPlayer == null || cameraTransform == null) return;

        bool anyInput = false;

        // ---------- PC Keyboard Input ----------
        if (!isVRMode)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                anyInput = true;
                if (!videoPlayer.isPlaying) videoPlayer.Play();
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                anyInput = true;
                if (videoPlayer.isPlaying) videoPlayer.Pause();
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                anyInput = true;
                cameraTransform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime, Space.World);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                anyInput = true;
                cameraTransform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
            }
        }

        // ---------- VR Controller Input ----------
#if UNITY_ANDROID && !UNITY_EDITOR
        float horizontal = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x;
        float vertical = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y;

        // Move forward/backward with joystick
        if (Mathf.Abs(vertical) > 0.1f)
        {
            anyInput = true;

            // Forward: play, Backward: pause
            if (vertical > 0f && !videoPlayer.isPlaying)
                videoPlayer.Play();
            else if (vertical < 0f && videoPlayer.isPlaying)
                videoPlayer.Pause();
        }

        // Rotate left/right with joystick
        if (Mathf.Abs(horizontal) > 0.1f && playerRig != null)
        {
            anyInput = true;
            playerRig.Rotate(Vector3.up, horizontal * rotationSpeed * Time.deltaTime, Space.World);
        }
#endif

        // ---------- Pause if no input ----------
        if (!anyInput && videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
        }
    }
}
using UnityEngine;
using UnityEngine.Video;

public class VR360ControlledPlayer : MonoBehaviour
{
    [Header("References")]
    public VideoPlayer videoPlayer;      // Drag your VideoPlayer here
    public Transform cameraTransform;    // Drag Main Camera (inside XR Rig)
    public Transform playerRig;          // Drag XR Origin or parent object (used in VR)

    [Header("Settings")]
    public float rotationSpeed = 60f;    // Degrees per second for rotation
    public bool isVRMode = false;        // Automatically detects on headset

    private void Start()
    {
        if (videoPlayer != null)
        {
            videoPlayer.playOnAwake = false;
            videoPlayer.Pause();
        }

        // Auto-assign main camera if not set
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

#if UNITY_ANDROID && !UNITY_EDITOR
        isVRMode = true; // Auto-detect VR when running on Meta Quest
#endif
    }

    private void Update()
    {
        if (videoPlayer == null || cameraTransform == null) return;

        // ========= Keyboard Controls (Mac / PC) =========
        if (!isVRMode)
        {
            // Forward → Play
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                if (!videoPlayer.isPlaying) videoPlayer.Play();
            }
            // Backward → Pause
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                if (videoPlayer.isPlaying) videoPlayer.Pause();
            }

            // Rotate Left
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                cameraTransform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime, Space.World);
            }
            // Rotate Right
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                cameraTransform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
            }
        }

        // ========= VR Joystick Controls (Meta Quest) =========
#if UNITY_ANDROID && !UNITY_EDITOR
        Vector2 joystick = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        // Forward (Play) / Backward (Pause)
        if (Mathf.Abs(joystick.y) > 0.2f)
        {
            if (joystick.y > 0f && !videoPlayer.isPlaying)
                videoPlayer.Play();
            else if (joystick.y < 0f && videoPlayer.isPlaying)
                videoPlayer.Pause();
        }

        // Rotate Left/Right via joystick
        if (Mathf.Abs(joystick.x) > 0.2f)
        {
            if (playerRig != null)
                playerRig.Rotate(Vector3.up, joystick.x * rotationSpeed * Time.deltaTime, Space.World);
            else
                cameraTransform.Rotate(Vector3.up, joystick.x * rotationSpeed * Time.deltaTime, Space.World);
        }
#endif
    }
}

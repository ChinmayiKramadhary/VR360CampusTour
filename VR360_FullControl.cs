using UnityEngine;
using UnityEngine.Video;

public class VR360_FullControl : MonoBehaviour
{
    public VideoPlayer videoPlayer;      // drag GameObject with VideoPlayer here
    public Transform rotateParent;       // ManualLookAnchor (parent to rotate horizontally)
    public Transform cameraTransform;    // Main Camera

    public float rotationSpeed = 0.18f;
    public float pitchSpeed = 0.12f;
    public float minPitch = -85f;
    public float maxPitch = 85f;

    public float fovStep = 6f;
    public float minFOV = 45f;
    public float maxFOV = 95f;

    bool isDragging = false;
    Vector3 lastMousePos;
    float currentPitch = 0f;

    void Start()
    {
        if (cameraTransform == null && Camera.main != null) cameraTransform = Camera.main.transform;
        if (rotateParent == null) rotateParent = transform;
        if (cameraTransform != null)
        {
            float p = cameraTransform.localEulerAngles.x;
            if (p > 180f) p -= 360f;
            currentPitch = p;
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, minFOV, maxFOV);
        }
    }

    void Update()
    {
        // Pause/Play
        if (Input.GetKeyDown(KeyCode.Space) && videoPlayer != null)
        {
            if (videoPlayer.isPlaying) videoPlayer.Pause();
            else videoPlayer.Play();
            Debug.Log("[VR360] Video toggled. Playing? " + videoPlayer.isPlaying);
        }

        // Mouse drag rotation (works any time)
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0)) isDragging = false;

        if (isDragging && rotateParent != null && cameraTransform != null)
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            float yaw = delta.x * rotationSpeed;
            float pitchDelta = -delta.y * pitchSpeed;

            rotateParent.Rotate(0f, yaw, 0f, Space.World);

            currentPitch = Mathf.Clamp(currentPitch + pitchDelta, minPitch, maxPitch);
            Vector3 e = cameraTransform.localEulerAngles;
            e.x = currentPitch;
            cameraTransform.localEulerAngles = e;

            lastMousePos = Input.mousePosition;
        }

        // Zoom via mouse wheel (FOV)
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.0001f && Camera.main != null)
        {
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView - scroll * fovStep * 20f * Time.deltaTime, minFOV, maxFOV);
        }

        if (Input.GetKeyDown(KeyCode.Equals) || Input.GetKeyDown(KeyCode.KeypadPlus))
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView - fovStep, minFOV, maxFOV);
        if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
            Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView + fovStep, minFOV, maxFOV);
    }
}
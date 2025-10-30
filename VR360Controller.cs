using UnityEngine;
using UnityEngine.Video;

public class VR360Controller : MonoBehaviour
{
    [Header("References")]
    public VideoPlayer videoPlayer;     // drag your VideoPlayer here
    public Camera vrCamera;             // drag Main Camera here

    [Header("Settings")]
    public float rotationSpeed = 0.2f;
    public float moveSpeed = 2f;
    public float zoomSpeed = 10f;
    public float minFOV = 30f;
    public float maxFOV = 100f;

    private bool isPaused = false;
    private bool isDragging = false;
    private Vector3 lastMousePos;
    private float yaw = 0f;
    private float pitch = 0f;

    void Start()
    {
        if (vrCamera == null)
            vrCamera = GetComponent<Camera>();

        Vector3 euler = transform.localEulerAngles;
        yaw = euler.y;
        pitch = euler.x;
    }

    void Update()
    {
        // --- Pause / Play ---
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Pause();
                isPaused = true;
                Debug.Log("Paused");
            }
            else
            {
                videoPlayer.Play();
                isPaused = false;
                Debug.Log("Playing");
            }
        }

        // --- Mouse Look ---
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMousePos = Input.mousePosition;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            yaw += delta.x * rotationSpeed;
            pitch -= delta.y * rotationSpeed;
            pitch = Mathf.Clamp(pitch, -85f, 85f);

            vrCamera.transform.localRotation = Quaternion.Euler(pitch, yaw, 0f);
            lastMousePos = Input.mousePosition;
        }

        // --- Zoom ---
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f && vrCamera != null)
        {
            vrCamera.fieldOfView -= scroll * zoomSpeed;
            vrCamera.fieldOfView = Mathf.Clamp(vrCamera.fieldOfView, minFOV, maxFOV);
        }

        // --- Movement follows camera direction ---
        float moveInput = Input.GetAxis("Vertical"); // W/S or Up/Down
        if (Mathf.Abs(moveInput) > 0.01f)
        {
            // Move exactly in direction the camera is facing
            Vector3 moveDir = vrCamera.transform.forward;
            moveDir.y = 0; // keep level
            moveDir.Normalize();

            vrCamera.transform.parent.Translate(moveDir * moveInput * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}

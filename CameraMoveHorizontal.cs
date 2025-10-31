using UnityEngine;

public class CameraMoveHorizontal : MonoBehaviour
{
    public float moveSpeed = 2f; // Adjust movement speed

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); // A/D or Left/Right arrows

        // Move camera left or right in world space (no rotation)
        Vector3 moveDirection = Vector3.right * horizontal * moveSpeed * Time.deltaTime;
        transform.Translate(moveDirection, Space.World);

        // Up/Down keys do nothing — ignore vertical input
    }
}
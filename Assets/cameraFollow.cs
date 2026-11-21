using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The player's transform
    public float smoothSpeed = 0.125f; // How fast the camera should follow
    public Vector3 offset; // The offset from the player (e.g., to keep the player at the bottom of the screen)

    // LateUpdate is called after all Update functions have been called.
    // This is the best place for camera logic to avoid jitter.
    void LateUpdate()
    {
        // Make sure we have a target to follow
        if (target == null)
        {
            return;
        }

        // Calculate the desired position for the camera
        Vector3 desiredPosition = target.position + offset;
        // The camera's Z position should remain the same (usually -10 for 2D)
        desiredPosition.z = transform.position.z;

        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
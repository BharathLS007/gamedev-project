using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target Settings")]
    public Transform target;       // The target (car) the camera follows
    public Vector3 offset = new Vector3(0, 5, -10);  // Offset from the target
    public float smoothSpeed = 0.125f;               // Smooth speed for camera movement

    private void LateUpdate()
    {
        // Calculate the desired position
        Vector3 desiredPosition = target.position + target.TransformVector(offset);

        // Smoothly move the camera towards the desired position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // Make the camera look at the car
        transform.LookAt(target);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [Header("Car Settings")]
    public float maxMotorTorque = 1500f;      // Maximum torque applied to the wheels
    public float maxSteeringAngle = 30f;      // Maximum steering angle for the wheels
    public float maxSpeed = 200f;             // Maximum speed in km/h
    public float brakeForce = 3000f;          // Braking force applied when braking
    public float downforce = 100f;            // Downforce to apply at high speeds
    public float steeringSensitivity = 0.1f;  // Steering sensitivity for gradual turning
    public float highSpeedSteerAngle = 10f;   // Reduced steering angle at high speeds

    [Header("Wheel Colliders")]
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    [Header("Wheel Transforms (for visuals)")]
    public Transform frontLeftTransform;
    public Transform frontRightTransform;
    public Transform rearLeftTransform;
    public Transform rearRightTransform;

    private Rigidbody rb;
    private float currentSteeringAngle = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Input for acceleration, braking, and steering
        float motorInput = Input.GetKey(KeyCode.W) ? 1 : (Input.GetKey(KeyCode.S) ? -1 : 0);
        float steeringInput = Input.GetKey(KeyCode.A) ? -1 : (Input.GetKey(KeyCode.D) ? 1 : 0);
        bool isBraking = Input.GetKey(KeyCode.Space);

        // Calculate speed in km/h
        float speed = rb.velocity.magnitude * 3.6f;

        // Motor torque with speed limit
        float motorTorque = (speed < maxSpeed || motorInput < 0) ? motorInput * maxMotorTorque : 0f;
        rearLeftWheel.motorTorque = motorTorque;
        rearRightWheel.motorTorque = motorTorque;

        // Apply brake torque to all wheels if braking
        if (isBraking)
        {
            ApplyBrakes(brakeForce);
        }
        else
        {
            ApplyBrakes(0);
        }

        // Smooth steering angle adjustment based on speed
        float targetSteeringAngle = steeringInput * (speed > 60 ? highSpeedSteerAngle : maxSteeringAngle);
        currentSteeringAngle = Mathf.Lerp(currentSteeringAngle, targetSteeringAngle, steeringSensitivity);
        frontLeftWheel.steerAngle = currentSteeringAngle;
        frontRightWheel.steerAngle = currentSteeringAngle;

        // Apply downforce
        rb.AddForce(-transform.up * downforce * rb.velocity.magnitude);

        // Update visual wheel positions
        UpdateWheelPosition(frontLeftWheel, frontLeftTransform);
        UpdateWheelPosition(frontRightWheel, frontRightTransform);
        UpdateWheelPosition(rearLeftWheel, rearLeftTransform);
        UpdateWheelPosition(rearRightWheel, rearRightTransform);
    }

    void ApplyBrakes(float brakeForce)
    {
        rearLeftWheel.brakeTorque = brakeForce;
        rearRightWheel.brakeTorque = brakeForce;
        frontLeftWheel.brakeTorque = brakeForce;
        frontRightWheel.brakeTorque = brakeForce;
    }

    void UpdateWheelPosition(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 position;
        Quaternion rotation;
        wheelCollider.GetWorldPose(out position, out rotation);
        wheelTransform.position = position;
        wheelTransform.rotation = rotation;
    }
}

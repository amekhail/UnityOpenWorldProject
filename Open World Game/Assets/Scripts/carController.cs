using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class carController : MonoBehaviour
{
    // Reference to all wheel colliders on vehicle
    [SerializeField] private WheelCollider frontLeftWheelCollider;
    [SerializeField] private WheelCollider frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider;
    [SerializeField] private WheelCollider rearRightWheelCollider;

    // Reference to all wheel transforms on vehicle
    [SerializeField] private Transform frontLeftWheelTransform;
    [SerializeField] private Transform frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform;
    [SerializeField] private Transform rearRightWheelTransform;

    [SerializeField] private float motorForce;
    [SerializeField] private float brakeForce;
    [SerializeField] private float maxSteeringAngle;

    // Unity Standard input
    private const string HORIZONTAL = "Horizontal";
    private const string VERTICAL = "Vertical";

    // user inputs
    private float horizontalInput;
    private float verticalInput;
    private float currentBrakeForce;
    private float currentSteeringAngle;
    private float currentSpeed;
    private bool isBraking;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float audioPitch;

    // called every frame because this is a physics object
    private void FixedUpdate()
    {
        getInput();
        handleMotor();
        setPitch();
        handleSteering();
        updateWheeles();
    }

    private void getInput()
    {
        horizontalInput = Input.GetAxis(HORIZONTAL);
        verticalInput = Input.GetAxis(VERTICAL);
        isBraking = Input.GetKey(KeyCode.Space);
    }

    private void setPitch()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        Vector3 vel = rb.velocity;

        if (vel.magnitude < 0)
        {
            audioPitch = 1f;
        }
        else
        {
            audioPitch = vel.magnitude / 2;
        }
        Debug.Log(audioPitch);
        audioSource.pitch = audioPitch;
    }

    private void ApplyBraking()
    {
        frontLeftWheelCollider.brakeTorque = currentBrakeForce;
        frontRightWheelCollider.brakeTorque = currentBrakeForce;
        rearLeftWheelCollider.brakeTorque = currentBrakeForce;
        rearRightWheelCollider.brakeTorque = currentBrakeForce;
    }

    private void handleMotor()
    {
        currentSpeed = verticalInput * motorForce;
        frontLeftWheelCollider.motorTorque = currentSpeed;
        frontRightWheelCollider.motorTorque = currentSpeed;
        currentBrakeForce = isBraking ? brakeForce : 0f;
        ApplyBraking();
    }

    private void handleSteering()
    {
        currentSteeringAngle = maxSteeringAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteeringAngle;
        frontRightWheelCollider.steerAngle = currentSteeringAngle;
    }

    private void updateWheeles()
    {
        updateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        updateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        updateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        updateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void updateSingleWheel(WheelCollider w, Transform t)
    {
        Vector3 pos;
        Quaternion rot;
        w.GetWorldPose(out pos, out rot);
        t.rotation = rot;
        t.position = pos;
    }

}

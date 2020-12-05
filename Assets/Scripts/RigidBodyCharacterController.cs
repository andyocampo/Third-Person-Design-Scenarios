using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RigidBodyCharacterController : MonoBehaviour
{
    [SerializeField]
    private float accelerationForce = 10;

    [SerializeField]
    private float maxSpeed = 3;
    
    [SerializeField]
    [Tooltip("0 = no turning, 1 = instant snap turning.")]
    [Range(0,1)]
    private float turnSpeed = 0.3f;

    [SerializeField]
    private PhysicMaterial stoppingPhysicsMaterial, movingPhysicsMaterial;

    private new Rigidbody rigidbody;
    private Vector2 input;
    private new Collider collider;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        Vector3 cameraRelativeInputDirection = GetCameraRelativeInputDirection();
        UpdatePhysicsMaterial();
        Move(cameraRelativeInputDirection);
        RotateToFaceInput(cameraRelativeInputDirection);
    }

    /// <summary>
    /// Turning the character to face the direction it wants to move in.
    /// </summary>
    /// <param name="movementDirection">The direction the character is trying to move in.</param>
    private void RotateToFaceInput(Vector3 movementDirection)
    {
        if (movementDirection.magnitude > 0)
        {
            var targetRotation = Quaternion.LookRotation(movementDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed);
        }
    }

    /// <summary>
    /// Moves the player in a direction based on its max speed and acceleration.
    /// </summary>
    /// <param name="moveDirection">The direction to move.</param>
    private void Move(Vector3 moveDirection)
    {
        if (rigidbody.velocity.magnitude < maxSpeed)
        {
            rigidbody.AddForce(moveDirection * accelerationForce, ForceMode.Acceleration);
        }
    }

    /// <summary>
    /// Updates the physics material to a low friction option if the player is moving,
    /// or high friction option if they're trying to stop.
    /// </summary>
    private void UpdatePhysicsMaterial()
    {
        collider.material = input.magnitude > 0 ? collider.material = movingPhysicsMaterial : collider.material = stoppingPhysicsMaterial;
    }

    /// <summary>
    /// Uses the input vector to create a camera relzative version so the player moves based
    /// on the camera's forward.
    /// </summary>
    /// <returns>Returns camera relative input direction</returns>
    private Vector3 GetCameraRelativeInputDirection()
    {
        var inputDirection = new Vector3(input.x, 0, input.y);

        Vector3 flattenedCameraForward = Camera.main.transform.forward;
        flattenedCameraForward.y = 0;
        var cameraRotation = Quaternion.LookRotation(flattenedCameraForward);

        Vector3 cameraRelativeInputDirection = cameraRotation * inputDirection;
        return cameraRelativeInputDirection;
    }

    /// <summary>
    /// This event handler is called from the PlayerInput component
    /// using the new Unity input system.
    /// </summary>
    /// <param name="context">Vector 2 representing move input</param>
    public void OnMove(InputAction.CallbackContext context)
    {
        input = context.ReadValue<Vector2>();
    }
}

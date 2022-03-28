using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotionManager : MonoBehaviour
{
    private InputManager inputManager;
    private PlayerManager playerManager;

    public Rigidbody playerRigidbody;

    [Header("Camera Transform")]
    public Transform playerCamera;

    [Header("Movement Speed")]
    public float rotationSpeed = 3.5f;
    public float quickTurnSpeed = 8f;

    [Header("Rotation variables")]
    private Quaternion targetRotation; // the place we want to rotate
    private Quaternion playerRotation; // the place we are rotating now, constantly changing

    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        inputManager = GetComponent<InputManager>();
        playerManager = GetComponent<PlayerManager>();
    }

    public void HandleAllLocomotion()
    {
        // handle rotation
        HandleRotation();

        // handle falling
    }

    private void HandleRotation()
    {
        if (playerManager.isAiming)
        {
            targetRotation = Quaternion.Euler(0, playerCamera.eulerAngles.y, 0);
            playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.rotation = playerRotation;
        }
        else
        {
            targetRotation = Quaternion.Euler(0, playerCamera.eulerAngles.y, 0);
            playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (inputManager.verticalMovementInput != 0 || inputManager.horizontalMovementInput != 0) // rotating player only when there is some input, not when player is idle
            {
                transform.rotation = playerRotation;
            }
            if (playerManager.isPerformingQuickTurn)
            {
                playerRotation = Quaternion.Slerp(transform.rotation, targetRotation, quickTurnSpeed * Time.deltaTime);
                transform.rotation = playerRotation;
            }
        }
    }
}

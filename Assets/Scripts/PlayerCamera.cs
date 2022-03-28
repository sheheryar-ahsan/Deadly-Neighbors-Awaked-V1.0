using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private InputManager inputManager;
    private PlayerManager playerManager;

    public Transform cameraPivot;
    public Camera cameraObject;
    [Header("Camera Follows Target")]
    public GameObject player; // follows the player while we are not aiming
    public Transform aimCameraPosition; // follow the player while we are aiming

    [Header("Camera Speed")]
    public float cameraSmoothTime = 0.2f;
    public float aimedCameraSmoothTime = 3f;

    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 targetPosition;
    private Vector3 cameraRotation;
    private Quaternion targetRotation;

    private float lookAmountVertical;
    private float lookAmountHorizontal;
    private float minimumPivotAngle = -15;
    private float maximumPivotAngle = 15;

    private void Awake()
    {
        inputManager = player.GetComponent<InputManager>();
        playerManager = player.GetComponent<PlayerManager>();
    }

    public void HandleAllCameraMovement()
    {
        if (Time.timeScale != 0)
        {
            // Follow the player
            FollowPlayer();

            // Rotate the camera
            RotateCamera();
        }
    }

    private void FollowPlayer()
    {
        if (playerManager.isAiming) // if we are aiming we want the camera follow the aim position
        {
            targetPosition = Vector3.SmoothDamp(transform.position, aimCameraPosition.transform.position, ref cameraFollowVelocity, cameraSmoothTime * Time.deltaTime);
            transform.position = targetPosition;
        }
        else 
        {
            // For moving from current position to target position, ref (passing parameter by reference not by value)
            targetPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraFollowVelocity, cameraSmoothTime * Time.deltaTime);
            transform.position = targetPosition;
        }
    }

    private void RotateCamera()
    {
        if (playerManager.isAiming) // if we are aiming we want camera object to rotate itself
        {
            minimumPivotAngle = -50;
            maximumPivotAngle = 50;
            
            cameraPivot.localRotation = Quaternion.Euler(0, 0, 0);

            lookAmountVertical = lookAmountVertical + (inputManager.horizontalCameraInput);
            lookAmountHorizontal = lookAmountHorizontal - (inputManager.verticalCameraInput);
            // for locking values between minimum and maximum not over
            lookAmountHorizontal = Mathf.Clamp(lookAmountHorizontal, minimumPivotAngle, maximumPivotAngle);

            cameraRotation = Vector3.zero;
            cameraRotation.y = lookAmountVertical;
            targetRotation = Quaternion.Euler(cameraRotation);
            targetRotation = Quaternion.Slerp(transform.rotation, targetRotation, cameraSmoothTime);
            transform.rotation = targetRotation;

            cameraRotation = Vector3.zero;
            cameraRotation.x = lookAmountHorizontal;
            targetRotation = Quaternion.Euler(cameraRotation);
            targetRotation = Quaternion.Slerp(cameraPivot.localRotation, targetRotation, cameraSmoothTime);
            cameraObject.transform.localRotation = targetRotation;
        }
        else 
        {
            minimumPivotAngle = -15;
            maximumPivotAngle = 15;

            cameraObject.transform.localRotation = Quaternion.Euler(0, 0, 0);

            lookAmountVertical = lookAmountVertical + (inputManager.horizontalCameraInput);
            lookAmountHorizontal = lookAmountHorizontal - (inputManager.verticalCameraInput);
            // for locking values between minimum and maximum not over
            lookAmountHorizontal = Mathf.Clamp(lookAmountHorizontal, minimumPivotAngle, maximumPivotAngle);

            cameraRotation = Vector3.zero;
            cameraRotation.y = lookAmountVertical;
            targetRotation = Quaternion.Euler(cameraRotation);
            targetRotation = Quaternion.Slerp(transform.rotation, targetRotation, cameraSmoothTime);
            transform.rotation = targetRotation;

            // if we perform quick turn we need to snap camera 180
            if (inputManager.quickTurnInput)
            {
                inputManager.quickTurnInput = false;
                lookAmountVertical = lookAmountVertical + 180f;
                cameraRotation.y = cameraRotation.y + 180f;
                transform.rotation = targetRotation;
                // in future add smooth transition
            }

            cameraRotation = Vector3.zero;
            cameraRotation.x = lookAmountHorizontal;
            targetRotation = Quaternion.Euler(cameraRotation);
            targetRotation = Quaternion.Slerp(cameraPivot.localRotation, targetRotation, cameraSmoothTime);
            cameraPivot.localRotation = targetRotation;
        }
    }
}

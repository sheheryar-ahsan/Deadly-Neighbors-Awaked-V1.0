using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class AnimatorManager : MonoBehaviour
{
    public Animator animator;

    [Header("Hand IK Constraints")] // these constraints enable our character to hold the current weapon properly
    public TwoBoneIKConstraint rightHandIK;
    public TwoBoneIKConstraint leftHandIK;

    [Header("Hand IK Constraints")] // these constraints turn the character towards aiming target
    public MultiAimConstraint spine01;
    public MultiAimConstraint spine02;
    public MultiAimConstraint head;

    private float snappedHorizontal;
    private float snappedVertical;

    private RigBuilder rigBuilder;
    private PlayerLocomotionManager playerLocomotionManager;
    private PlayerManager playerManager;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rigBuilder = GetComponent<RigBuilder>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerManager = GetComponent<PlayerManager>();
    }

    public void PlayAnimationWithOurRootMotion(string targetAnimation, bool isPerformingAction)
    {
        animator.SetBool("isPerformingAction", isPerformingAction);
        animator.SetBool("disableRootMotion", true);
        animator.applyRootMotion = false;
        animator.CrossFade(targetAnimation, 0.2f);
    }

    public void PlayAnimation(string targetAnimation, bool isPerformingAction)
    {
        animator.SetBool("isPerformingAction", isPerformingAction);
        animator.SetBool("disableRootMotion", true);
        animator.CrossFade(targetAnimation, 0.2f);
    }

    // controlling the animator values
    public void HandleAnimatorValue(float horizontalMovement, float verticalMovement, bool isRunning)
    {
        // incase on adding more animations, dont want to blend between two animation So, 
        // for animator to have exact value, using snap methods to avoid in-between values for horizontal movement 
        if (horizontalMovement > 0)
        {
            snappedHorizontal = 1;
        }
        else if (horizontalMovement > 1)
        {
            snappedHorizontal = 2;
        }
        else if (horizontalMovement < 0)
        {
            snappedHorizontal = -1;
        }
        else if (horizontalMovement < -1)
        {
            snappedHorizontal = -2;
        }
        else
        {
            snappedHorizontal = 0;
        }
        // for animator to have exact value, using snap methods to avoid in-between values for vertical movement
        if (verticalMovement > 0)
        {
            snappedVertical = 1;
        }
        else if (verticalMovement < 0)
        {
            snappedVertical = -1;
        }
        else
        {
            snappedVertical = 0;
        }

        if (isRunning && snappedVertical > 0) // we dont want to be able to run backward, or run whilst moving backwards
        {
            snappedVertical = 2;
        }
        if (isRunning && snappedVertical == 0 && snappedHorizontal > 0) // when only left shift and D key is pressed, for running right
        {
            snappedHorizontal = 2;
        }
        if (isRunning && snappedVertical == 0 && snappedHorizontal < 0) // when only left shift and A key is pressed, for running left
        {
            snappedHorizontal = -2;
        }

        animator.SetFloat("Horizontal", snappedHorizontal, 0.1f, Time.deltaTime);
        animator.SetFloat("Vertical", snappedVertical, 0.1f, Time.deltaTime);
    }

    public void AssignHandIK(RightHandIKTarget rightTarget, LeftHandIKTarget leftTarget)
    {
        rightHandIK.data.target = rightTarget.transform;
        leftHandIK.data.target = leftTarget.transform;
        rigBuilder.Build();
    }

    private void OnAnimatorMove() // when an animation is playing on animator and it has root motion, we going to mimic movement of animtion on the player rigidbody
    {
        if (playerManager.disableRootMotion)
        {
            return;
        }

        Vector3 animatorDeltaPosition = animator.deltaPosition;
        animatorDeltaPosition.y = 0;

        if (animatorDeltaPosition == Vector3.zero)
        {
            return;
        }

        Vector3 velocity = animatorDeltaPosition / Time.deltaTime;
        playerLocomotionManager.playerRigidbody.drag = 0;
        playerLocomotionManager.playerRigidbody.velocity = velocity;
        transform.rotation *= animator.deltaRotation;
    }

    // while aiming our character will turn towards center of the screen
    public void UpdateAimConstraints()
    {
        if (playerManager.isAiming)
        {
            spine01.weight = 0.3f;
            spine02.weight = 0.3f;
            head.weight = 0.7f;
        }
        else
        {
            spine01.weight = 0;
            spine02.weight = 0;
            head.weight = 0;
        }
    }
}

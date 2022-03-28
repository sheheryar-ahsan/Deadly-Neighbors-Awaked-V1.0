using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorBool : StateMachineBehaviour
{
    [Header("Disable Root Motion")]
    public string disableRootMotion = "disableRootMotion";
    public bool disableRootMotionStatus = false;

    [Header("Is performing Action Bool")]
    public string isPerformingAction = "isPerformingAction";
    public bool isPerformingActionStatus = false;

    [Header("Is performing Quick Turn")]
    public string isPerformingQuickTurn = "isPerformingQuickTurn";
    public bool isPerformingQuickTurnStatus = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(disableRootMotion, disableRootMotionStatus);
        animator.SetBool(isPerformingAction, isPerformingActionStatus);
        animator.SetBool(isPerformingQuickTurn, isPerformingQuickTurnStatus);
        
    }
}

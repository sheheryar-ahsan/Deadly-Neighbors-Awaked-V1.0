using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueTargetState : State
{
    private AttackState attackState;

    private void Awake()
    {
        attackState = GetComponent<AttackState>();
    }

    public override State Tick(ZombieManager zombieManager)
    {
        // if zombie is being hurt or in some action, pause the state
        if (zombieManager.isPerformingAction)
        {
            zombieManager.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
            return this;
        }

        //Debug.Log("running the pursue target state");
        MoveTowardsCurrentTarget(zombieManager);
        RotateTowardsCurrentTarget(zombieManager);

        if (zombieManager.distanceFromCurrentTarget <= zombieManager.maximumAttackDistance)
        {
            zombieManager.zombieNavmeshAgent.enabled = false;
            return attackState;
        }
        else
        {
            return this;
        }
    }

    private void MoveTowardsCurrentTarget(ZombieManager zombieManager)
    {
        zombieManager.animator.SetFloat("Vertical", 2f, 0.2f, Time.deltaTime);
    }

    private void RotateTowardsCurrentTarget(ZombieManager zombieManager)
    {
        zombieManager.zombieNavmeshAgent.enabled = true;
        zombieManager.zombieNavmeshAgent.SetDestination(zombieManager.currentTarget.transform.position);

        // overtime we are going to turn our zombie in the same direction our navmesh agent is turning
        zombieManager.transform.rotation = Quaternion.Slerp(zombieManager.transform.rotation, zombieManager.zombieNavmeshAgent.transform.rotation, zombieManager.rotationSpeed / Time.deltaTime);
    }
}

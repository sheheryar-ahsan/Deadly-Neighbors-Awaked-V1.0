using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    private PursueTargetState pursueTargetState;

    [Header("Zombie Attack")]
    public ZombieAttackAction[] zombieAttackActions;
    public bool isAttack = false;

    [Header("Potential Attack Performable Right Now")]
    public List<ZombieAttackAction> potentialAttack;

    [Header("Current Attack Being Performed")]
    public ZombieAttackAction currentAttack;

    [Header("State Flags")]
    public bool hasPerformedAttack;

    private void Awake()
    {
        pursueTargetState = GetComponent<PursueTargetState>();
    }

    public override State Tick(ZombieManager zombieManager)
    {
        zombieManager.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);

        if (zombieManager.isPerformingAction)
        {
            zombieManager.animator.SetFloat("Vertical", 0, 0.2f, Time.deltaTime);
            return this;
        }

        if (!hasPerformedAttack && zombieManager.attackCooldownTimer <=0)
        {
            if (currentAttack == null)
            {
                // get a new attack based on the distance and angle from the current target
                GetNewAttack(zombieManager);
            }
            else
            {
                // attack the current target
                AttackTarget(zombieManager);
            }
        }

        if (hasPerformedAttack)
        {
            // reset our state flags exit
            // go back to the pursue target state
            ResetStateFlags();
            return pursueTargetState;
        }
        else
        {
            return this;
        }
    }

    private void GetNewAttack(ZombieManager zombieManager)
    {
        for (int i = 0; i < zombieAttackActions.Length; i++)
        {
            ZombieAttackAction zombieAttack = zombieAttackActions[i];

            // check for attack distances needed to perform the potential attack
            if (zombieManager.distanceFromCurrentTarget <= zombieAttack.maximumAttackDistance && zombieManager.distanceFromCurrentTarget >= zombieAttack.minimumAttackDistance)
            {
                // check for attack angles 
                if (zombieManager.viewableAngleFromCurrentTarget <= zombieAttack.maximumAttackAngle && zombieManager.viewableAngleFromCurrentTarget >= zombieAttack.minimumAttackAngle)
                {
                    // if the attack passes the distance and angle check, add it to the list of attacks we may perform right now
                    potentialAttack.Add(zombieAttack);
                }
            }
        }

        int randomValue = Random.Range(0, potentialAttack.Count);

        if (potentialAttack.Count > 0)
        {
            currentAttack = potentialAttack[randomValue];
            potentialAttack.Clear();
        }
    }

    private void AttackTarget(ZombieManager zombieManager)
    {
        if (currentAttack != null)
        {
            hasPerformedAttack = true;
            zombieManager.attackCooldownTimer = currentAttack.attackCooldown;
            zombieManager.ZombieAnimatorManager.PlayTargetAttackAnimation(currentAttack.attackAnimation);
            isAttack = true;
        }
        else
        {
            Debug.LogWarning("Zombie is attempting tp perform an attack, but has no attack");
        }
    }

    private void ResetStateFlags()
    {
        hasPerformedAttack = false;
    }
}

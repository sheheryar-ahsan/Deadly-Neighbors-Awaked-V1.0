using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "A.I/Actions/Zombie Attack Action")]
public class ZombieAttackAction : ScriptableObject
{
    [Header("Attack Animation")]
    public string attackAnimation;

    [Header("Attack Cooldown")]
    public float attackCooldown = 5f; // the time before the zombie can perform another attack

    // the minimum & maximum angle and distance from target neeeded to perform attack
    [Header("Attack Angles & Distances")]
    public float minimumAttackAngle = -20f;
    public float maximumAttackAngle = 20f;
    public float minimumAttackDistance = 1f;
    public float maximumAttackDistance = 3.5f;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
    // the layer used to detect potential attack targets
    [Header("Detection Layer")]
    [SerializeField] private LayerMask detectionLayer;

    // this setting determines where our linecast start on the Y axis of the character (used for line of sight)
    [Header("Line Of Sight Detection")]
    [SerializeField] private float characterEyeLevel = 1.8f;
    [SerializeField] private LayerMask ignoreForLineOfSightDetection;

    // how far we can detect a target
    [Header("Detection Radius")]
    [SerializeField] private float detectionRadius = 5f;

    // how wide we can see a target within our Field Of View
    [Header("Detection Angle Radius")]
    [SerializeField] private float minimumDetectionRadiusAngle = -50f;
    [SerializeField] private float maximumDetectionRadiusAngle = 50f;

    private PursueTargetState pursueTargetState;
    // we make our character idle until they find a potentioanl target
    // if a target is found we proceed to the "PursueTarget" state
    // if no target is found we remain in idle position

    private void Awake()
    {
        pursueTargetState = GetComponent<PursueTargetState>();
    }

    public override State Tick(ZombieManager zombieManager)
    {
        if (zombieManager.currentTarget != null)
        {
            return pursueTargetState;
        }
        else
        {
            FindTargetViaLineOfSight(zombieManager);
            return this;
        }
    }

    private void FindTargetViaLineOfSight(ZombieManager zombieManager)
    {
        // search for all colliders on the layer of the player within a certain radius
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayer);

        //Debug.Log("we are checking for colliders");
        // for every collider we found, that is on the some layer of the player, we try and search it for the a player manager script
        for (int i = 0; i < colliders.Length; i++)
        {
            PlayerManager player = colliders[i].transform.GetComponent<PlayerManager>();

            // if the player manager is detected, we then check for line of sight;
            if (player != null)
            {
                //Debug.Log("we have found the player collider");
                // target must in front
                Vector3 targetDirection = transform.position - player.transform.position;
                float viewAbleAngle = Vector3.Angle(targetDirection, transform.forward);

                if (viewAbleAngle > minimumDetectionRadiusAngle && viewAbleAngle < maximumDetectionRadiusAngle)
                {
                    //Debug.Log("we have passed the field of view check");

                    RaycastHit hit;
                    Vector3 playerStartPoint = new Vector3(player.transform.position.x, characterEyeLevel, player.transform.position.z);
                    Vector3 zombiesStartPoint = new Vector3(transform.position.x, characterEyeLevel, transform.position.z);

                    Debug.DrawLine(playerStartPoint, zombiesStartPoint, Color.yellow);

                    // check one last time for object blocking view
                    if (Physics.Linecast(playerStartPoint, zombiesStartPoint, out hit, ignoreForLineOfSightDetection))
                    {
                        //Debug.Log("there is somthing in the way");
                        // cannt find the target, there is an object in the way
                    }
                    else
                    {
                        //Debug.Log("we have target swithing states");
                        zombieManager.currentTarget = player;
                    }
                }
            }
        }
    }
}

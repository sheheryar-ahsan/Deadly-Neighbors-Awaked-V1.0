using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieEffectsManager : MonoBehaviour
{
    public ParticleSystem bloodParticle;
    public ZombieManager zombie;

    private void Awake()
    {
        zombie = GetComponent<ZombieManager>();
    }

    public void DamageZombieHead(int damage, Transform spawPosition)
    {
        zombie.isPerformingAction = true;
        zombie.animator.CrossFade("Head Hit Reaction", 0.1f);
        zombie.zombieStatManager.DealHeadShotDamage(damage);
        ParticalSystem(spawPosition);
    }

    public void DamageZombieTorso(int damage, Transform spawPosition)
    {
        zombie.isPerformingAction = true;
        zombie.animator.CrossFade("Torso Hit Reaction", 0.1f);
        zombie.zombieStatManager.DealTorsoDamage(damage);
        ParticalSystem(spawPosition);
    }

    public void DamageZombieRightArm(int damage, Transform spawPosition)
    {
        zombie.isPerformingAction = true;
        zombie.animator.CrossFade("Torso Hit Reaction", 0.1f);
        zombie.zombieStatManager.DealArmDamage(false, damage);
        ParticalSystem(spawPosition);
    }

    public void DamageZombieLeftArm(int damage, Transform spawPosition)
    {
        zombie.isPerformingAction = true;
        zombie.animator.CrossFade("Torso Hit Reaction", 0.1f);
        zombie.zombieStatManager.DealArmDamage(true, damage);
        ParticalSystem(spawPosition);
    }

    public void DamageZombieLeftLeg(int damage, Transform spawPosition)
    {
        zombie.isPerformingAction = true;
        zombie.animator.CrossFade("Torso Hit Reaction", 0.1f);
        zombie.zombieStatManager.DealLegDamage(true, damage);
        ParticalSystem(spawPosition);
    }
    public void DamageZombieRightLeg(int damage, Transform spawPosition)
    {
        zombie.isPerformingAction = true;
        zombie.animator.CrossFade("Torso Hit Reaction", 0.1f);
        zombie.zombieStatManager.DealLegDamage(false, damage);
        ParticalSystem(spawPosition);
    }

    private void ParticalSystem(Transform spawPosition)
    {
        ParticleSystem bloodSplash = Instantiate(bloodParticle, spawPosition.position, bloodParticle.transform.rotation, zombie.transform);
        bloodSplash.Play();
        Destroy(bloodSplash.gameObject, 3f);
    }
}

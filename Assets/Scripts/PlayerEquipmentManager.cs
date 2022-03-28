using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipmentManager : MonoBehaviour
{
    private WeaponLoaderSlot weaponLoaderSlot;
    private PlayerManager playerManager;

    [Header("Current Equipment")]
    public WeaponItem weapon;
    public WeaponAnimatorManager weaponAnimator;
    private LeftHandIKTarget leftHandIK;
    private RightHandIKTarget rightHandIK;
    private object playerManageranimatorManager;

    //public SubWeaponItem Subweapon; // knife, grenade etc

    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        LoadWeaponLoaderSlots();
    }

    private void Start()
    {
        LoadCurrentweapon();
    }

    private void LoadWeaponLoaderSlots()
    {
        // back slot
        // hip slot
        weaponLoaderSlot = GetComponentInChildren<WeaponLoaderSlot>();
    }

    private void LoadCurrentweapon()
    {
        // load the weapon onto our players hand
        weaponLoaderSlot.LoadWeaponModel(weapon);
        // change our player movement/idle animations to the weapon movemnt/idle animations
        playerManager.animatorManager.animator.runtimeAnimatorController = weapon.weaponAnimator;
        weaponAnimator = weaponLoaderSlot.currentWeaponModel.GetComponentInChildren<WeaponAnimatorManager>();
        rightHandIK = weaponLoaderSlot.currentWeaponModel.GetComponentInChildren<RightHandIKTarget>();
        leftHandIK = weaponLoaderSlot.currentWeaponModel.GetComponentInChildren<LeftHandIKTarget>();
        playerManager.animatorManager.AssignHandIK(rightHandIK, leftHandIK);
        playerManager.playerUIManager.currentAmmoCountText.text = weapon.remainingAmmo.ToString();

        // check for ammo that supports this weapon in our inventory
        if (playerManager.playerInventoryManager.currentAmmoInventory != null)
        {
            if (playerManager.playerInventoryManager.currentAmmoInventory.ammoType == weapon.ammoType)
            {
                playerManager.playerUIManager.reservedAmmoCountText.text = playerManager.playerInventoryManager.currentAmmoInventory.ammoRemaining.ToString();
            }
        }
    }
}

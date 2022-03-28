using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickUp : MonoBehaviour
{
    private PlayerManager playerManager;
    private WeaponSoundManager weaponSoundManager;

    [Header("Ammo Amount")]
    public List<int> ammoAmount;

    private void Start()
    {
        StartCoroutine(DestroyPickUp());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            weaponSoundManager = other.gameObject.GetComponentInChildren<WeaponSoundManager>();
            playerManager = other.gameObject.GetComponent<PlayerManager>();
            weaponSoundManager.PlayAmmoPickupSound();
            HandleAmmoAmount();
            Destroy(this.gameObject);
        }
    }

    private void HandleAmmoAmount()
    {
        int randomAmount = Random.Range(0, ammoAmount.Count);

        if (playerManager.playerInventoryManager.currentAmmoInventory.ammoRemaining < 100)
        {
            AddAmmo(ammoAmount[randomAmount]);
        }
    }

    private void AddAmmo(int amount)
    {
        playerManager.playerInventoryManager.currentAmmoInventory.ammoRemaining += amount;

        if (playerManager.playerInventoryManager.currentAmmoInventory.ammoRemaining >= 100)
        {
            playerManager.playerInventoryManager.currentAmmoInventory.ammoRemaining = 99;
        }

        playerManager.playerUIManager.currentAmmoCountText.text = playerManager.playerEquipmentManager.weapon.remainingAmmo.ToString();
        playerManager.playerUIManager.reservedAmmoCountText.text = playerManager.playerInventoryManager.currentAmmoInventory.ammoRemaining.ToString();

    }

    IEnumerator DestroyPickUp()
    {
        yield return new WaitForSeconds(30);
        Destroy(this.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAnimatorManager : MonoBehaviour
{
    private PlayerManager player;
    private Animator weaponAnimator;
    private WeaponSoundManager weaponSoundManager;
    private InputManager inputManager;

    [Header("Weapon FX")]
    public GameObject weaponMuzzleFlashFX; // the muzzle flash FX that is instantiated when the weapon is fired
    public GameObject weaponBulletCaseFX; // the bullet case FX that is ejected from the weapon, when the weapon is fired

    [Header("Weapon FX Transform")]
    public Transform weaponMuzzleFlashTransform; // the location muzzle flash fx will instantiated
    public Transform weaponBulletCaseTransform; // the location the bullet case will instantiated

    [Header("Weapon Bullet Range")]
    [SerializeField] private float bulletRange = 100f;

    [Header("Shootable Layers")]
    public LayerMask shootableLayers;

    private void Awake()
    {
        weaponAnimator = GetComponentInChildren<Animator>();
        player = GetComponentInParent<PlayerManager>();
        weaponSoundManager = GetComponent<WeaponSoundManager>();
        inputManager = FindObjectOfType<InputManager>();
    }

    public void ShootWeapon(PlayerCamera playerCamera)
    {
        // animate pistol
        weaponAnimator.Play("Shoot");
        if (inputManager.aimingInput)
        {
            // play shooting sound
            weaponSoundManager.PlayShootingSound();
        }
        // Instantiate muzzle flash FX
        GameObject muzzleFlash = Instantiate(weaponMuzzleFlashFX, weaponMuzzleFlashTransform);
        muzzleFlash.transform.parent = null;
        // Instantiate empty bullet case
        GameObject bulletCase  = Instantiate(weaponBulletCaseFX, weaponBulletCaseTransform);
        bulletCase.transform.parent = null;
        // shooting something
        RaycastHit hit;
        if (Physics.Raycast(playerCamera.cameraObject.transform.position, playerCamera.cameraObject.transform.forward, out hit, bulletRange, shootableLayers))
        {
            Debug.Log(hit.collider.gameObject.layer);

            ZombieEffectsManager zombie = hit.collider.gameObject.GetComponentInParent<ZombieEffectsManager>();

            if (zombie != null && zombie.zombie.isDead == false)
            {
                if (hit.collider.gameObject.layer == 8)
                {
                    zombie.DamageZombieHead(player.playerEquipmentManager.weapon.damage, hit.collider.gameObject.transform);
                }
                else if (hit.collider.gameObject.layer == 9)
                {
                    zombie.DamageZombieTorso(player.playerEquipmentManager.weapon.damage, hit.collider.gameObject.transform);
                }
                else if (hit.collider.gameObject.layer == 10)
                {
                    zombie.DamageZombieRightArm(player.playerEquipmentManager.weapon.damage, hit.collider.gameObject.transform);
                }
                else if (hit.collider.gameObject.layer == 11)
                {
                    zombie.DamageZombieLeftArm(player.playerEquipmentManager.weapon.damage, hit.collider.gameObject.transform);
                }
                else if (hit.collider.gameObject.layer == 12)
                {
                    zombie.DamageZombieLeftLeg(player.playerEquipmentManager.weapon.damage, hit.collider.gameObject.transform);
                }
                else if (hit.collider.gameObject.layer == 13)
                {
                    zombie.DamageZombieRightLeg(player.playerEquipmentManager.weapon.damage, hit.collider.gameObject.transform);
                }
            }
        }
    }
}

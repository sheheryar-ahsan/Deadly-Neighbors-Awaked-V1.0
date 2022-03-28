using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public PlayerCamera playerCamera;
    private InputManager inputManager;
    public PlayerLocomotionManager playerLocomotionManager;
    public PlayerEquipmentManager playerEquipmentManager;
    private Animator animator;
    public AnimatorManager animatorManager;
    public PlayerUIManager playerUIManager;
    public PlayerInventoryManager playerInventoryManager;
    private GameUIManager gameUIManager;
    private ZombieManager zombieManager;
    private AttackState attackState;

    [Header("Player Flags")]
    public bool disableRootMotion;
    public bool isPerformingAction;
    public bool isPerformingQuickTurn;
    public bool isAiming;
    public bool isEmpty = false;
    private bool isAttack = false;
    public float gameDifficulty = 0.05f;

    private void Awake()
    {
        playerUIManager = FindObjectOfType<PlayerUIManager>();
        playerCamera = FindObjectOfType<PlayerCamera>();
        inputManager = GetComponent<InputManager>();
        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        animator = GetComponent<Animator>();
        animatorManager = GetComponent<AnimatorManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        gameUIManager = GameObject.FindGameObjectWithTag("Canvas").GetComponent<GameUIManager>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
        disableRootMotion = animator.GetBool("disableRootMotion");
        isAiming = animator.GetBool("isAiming");
        isPerformingAction = animator.GetBool("isPerformingAction");
        isPerformingQuickTurn = animator.GetBool("isPerformingQuickTurn");
        HandleHealthUI();
    }

    private void FixedUpdate() // due to movement on a rigidbody, we have to use fix update
    {
        playerLocomotionManager.HandleAllLocomotion();
    }

    private void LateUpdate()
    {
        playerCamera.HandleAllCameraMovement();
    }

    public void UseCurrentWeapon()
    {
        if (isPerformingAction)
        {
            return;
        }

        if (playerEquipmentManager.weapon.remainingAmmo > 0)
        {
            playerEquipmentManager.weapon.remainingAmmo = playerEquipmentManager.weapon.remainingAmmo - 1;
            playerUIManager.currentAmmoCountText.text = playerEquipmentManager.weapon.remainingAmmo.ToString();
            //animatorManager.PlayAnimationWithOurRootMotion("Pistol_Shoot", true);
            playerEquipmentManager.weaponAnimator.ShootWeapon(playerCamera);
        }
        else
        {
            isEmpty = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Zombie"))
        {
            zombieManager = other.gameObject.GetComponentInParent<ZombieManager>();
            attackState = zombieManager.gameObject.GetComponentInChildren<AttackState>();
            isAttack = true;
            //Debug.Log("zombie manager: " + zombieManager);
            //Debug.Log("attack state: " + attackState);
        }
        if (other.gameObject.CompareTag("GameOver"))
        {
            gameUIManager.gameWin.gameObject.SetActive(true);
            gameUIManager.GameWon();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        isAttack = false;
    }

    private void HandleHealthUI()
    {
        if (zombieManager != null && attackState != null)
        {
            if (attackState.isAttack && isAttack == true)
            {
                StartCoroutine(SubtractionFromPlayerHealth());
                attackState.isAttack = false;
            }
        }
    }

    IEnumerator SubtractionFromPlayerHealth()
    {
        yield return new WaitForSeconds(0.5f);
        if (zombieManager.distanceFromCurrentTarget <= 1f)
        {
            gameUIManager.healthSlider.value -= gameDifficulty;
            Debug.Log("Zombie is Attacked us");
        }
    }

}

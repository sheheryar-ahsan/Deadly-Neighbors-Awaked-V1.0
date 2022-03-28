using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSoundManager : MonoBehaviour
{
    [Header("Shooting Audio Clips")]
    public List<AudioClip> shootingAudioClips;

    [Header("Reloading Audio Clips")]
    public AudioClip ReloadAudioClip;

    [Header("No Ammo Clips")]
    public AudioClip noAmmoAudioClip;

    [Header("Ammo Pickup")]
    public AudioClip ammoPickupClip;

    private AudioSource audioSource;
    private InputManager inputManager;
    private PlayerManager playerManager;
    private AmmoPickUp ammoPickUp;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        inputManager = FindObjectOfType<InputManager>();
        playerManager = FindObjectOfType<PlayerManager>();
    }

    private void Update()
    {
        if (inputManager.isReloading)
        {
            inputManager.isReloading = false;
            PlayReloadSound();
        }
        if (playerManager.isEmpty)
        {
            playerManager.isEmpty = false;
            Debug.Log("Playing Sound");
            PlayAmmoSound();
        }
    }

    public void PlayShootingSound()
    {
        int random = Random.Range(0, shootingAudioClips.Count);

        audioSource.PlayOneShot(shootingAudioClips[1], 0.5f);
    }

    public void PlayReloadSound()
    {
        audioSource.PlayOneShot(ReloadAudioClip, 1f);
    }

    private void PlayAmmoSound()
    {
        audioSource.PlayOneShot(noAmmoAudioClip, 1f);
    }

    public void PlayAmmoPickupSound()
    {
        audioSource.PlayOneShot(ammoPickupClip, 1f);
    }
}

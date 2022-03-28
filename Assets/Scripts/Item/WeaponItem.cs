using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon Item")]
public class WeaponItem : Item
{
    [Header("Weapon Animation")]
    public AnimatorOverrideController weaponAnimator;

    [Header("Weapon Damage")]
    public int damage = 20;

    [Header("Ammo")]
    public int remainingAmmo = 0;
    public int maxAmmo = 12;
    public AmmoType ammoType;
}

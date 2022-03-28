using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName =  "Items/Box of Ammo Item")]
public class BoxOfAmmoItem : Item
{
    public AmmoType ammoType;
    public int ammoRemaining = 50;
    public int boxOfAmmoCapacity = 50;
}

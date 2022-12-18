using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Ammo")]
public class RangedAmmoItem : Item
{
    [Header("Ammo Type")]
    public AmmoType ammoType;

    [Header("Ammo Velocity")]
    public float forwardVelocity = 375;
    public float upwardVelocity = 0;
    public float ammoMass = 0;
    public bool useGravity = false;

    [Header("Ammo Capacity")]
    public int carryLimit = 99;
    public int currentAmount = 99;

    [Header("Ammo Base Damage")]
    public int physicalDamage = 50;

    [Header("Item Models")]
    public GameObject loadedItemModel;
    public GameObject liveAmmoModel;
    public GameObject penetratedModel;
}

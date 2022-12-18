using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInventoryManager : MonoBehaviour
{
    protected CharacterWeaponSlotManager characterWeaponSlotManager;

    [Header("Current Item Being Used")]
    public Item currentItemBeingUsed;

    [Header("Quick Slot Items")]
    public ConsumableItem currentConsumable;
    public SpellItem currentSpell;
    public WeaponItem rightWeapon;
    public WeaponItem leftWeapon;
    public RangedAmmoItem currentAmmo;
    
    [Header("Current Equipment")]

    public WeaponItem[] weaponsInRightHandSlots = new WeaponItem[1];
    public WeaponItem[] weaponsInLeftHandSlots = new WeaponItem[1];

    public int currentRightWeaponIndex = 0;
    public int currentLeftWeaponIndex = 0;

    private void Awake()
    {
        characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();
    }
    private void Start()
    {
        characterWeaponSlotManager.LoadBothWeaponsOnSlots();
    }
}

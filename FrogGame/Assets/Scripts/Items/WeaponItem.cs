using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Item/Weapon Item")]
public class WeaponItem : Item
{
    public GameObject modelPrefab;
    public bool isUnarmed;

    [Header("Weapon Animator")]
    public AnimatorOverrideController weaponAnimator;
    //public string offHandIdleAnimation = "Left Arm Idle_01";

    [Header("Weapon Type")]
    public WeaponType weaponType;

    [Header("Damage Modifiers")]
    public float lightAttackDamageMultiplier = 1;
    public float heavyAttackDamageMultiplier = 1.5f;
    public int criticalDamageMultiplier = 4;

    [Header("Damage")]
    public int baseDamage = 25;
    

    [Header("Absorption")]
    public float physicalDamageAbsorption;

    [Header("Poise")]
    public float poiseBreak;
    public float offensivePoiseBonus;

    [Header("Stamina Costs")]
    public int baseStamina = 25;
    public float lightAttackStaminaMultiplier = 1f;
    public float heavyAttackStaminaMultiplier = 1.5f;

    [Header("Item Actions")]
    public ItemAction tap_RB_Action;
    public ItemAction hold_RB_Action;

    public ItemAction tap_LB_Action;
    public ItemAction hold_LB_Action;

    public ItemAction tap_RT_Action;
    public ItemAction hold_RT_Action;

    public ItemAction tap_LT_Action;
    public ItemAction hold_LT_Action;

    [Header("Two Handed Actions")]
    public ItemAction th_tap_RB_Action;
    public ItemAction th_hold_RB_Action;

    public ItemAction th_tap_LB_Action;
    public ItemAction th_hold_LB_Action;

    public ItemAction th_tap_RT_Action;
    public ItemAction th_hold_RT_Action;

    public ItemAction th_tap_LT_Action;
    public ItemAction th_hold_LT_Action;

    [Header("Sound FX")]
    public AudioClip[] weaponWhooses;
}

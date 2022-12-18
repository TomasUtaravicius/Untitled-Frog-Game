using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public Animator animator;
    public CharacterAnimatorHandler characterAnimatorHandler;
    public CharacterWeaponSlotManager characterWeaponSlotManager;
    public CharacterInventoryManager characterInventoryManager;
    public CharacterEffectsManager characterEffectsManager;
    public CharacterStatsManager characterStatsManager;
    public CharacterSoundFXManager characterSoundFXManager;
    public CharacterCombatManager characterCombatManager;
    public CharacterEquipmentManager characterEquipmentManager;
    [Header("Status")]
    public bool isDead;

    [Header("Lock On Transform")]
    public Transform lockOnTransform;

    [Header("Combat Transforms")]
    public Transform criticalAttackRayCastStartPoint;

    [Header("Combat Colliders")]
    public CriticalDamageCollider backstabCollider;
    public CriticalDamageCollider riposteColllider;
    public BlockingCollider blockingCollider;

    [Header("Combat Flags")]
    public bool canBeRiposted;
    public bool canBeParried;
    public bool isParrying;
    public bool isBlocking;
    public bool isInvulnerable;
    public bool canDoCombo;
    public bool isTwoHanding;
    public bool isHoldingArrow;
    public bool isAiming;
    public bool isPerformingFullyChargedAttack;
    public bool isAttacking;

    [Header("Spells")]
    public bool isFiringSpell;

    [Header("Movement")]
    public bool isRotatingWithRootMotion;
    public bool canRotate;
    public bool isSprinting;
    public bool isInAir;
    public bool isGrounded;

    [Header("Player Flags")]
    public bool isInteracting;

    


    public bool isUsingLeftHand;
    public bool isUsingRightHand;

    public int pendingCriticalDamage;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        characterAnimatorHandler = GetComponent<CharacterAnimatorHandler>();
        characterWeaponSlotManager = GetComponent<CharacterWeaponSlotManager>();
        characterEffectsManager = GetComponent<CharacterEffectsManager>();
        characterStatsManager = GetComponent<CharacterStatsManager>();
        characterInventoryManager = GetComponent<CharacterInventoryManager>();
        characterSoundFXManager = GetComponent<CharacterSoundFXManager>();
        characterCombatManager = GetComponent<CharacterCombatManager>();
        blockingCollider = GetComponentInChildren<BlockingCollider>();
    }
    protected virtual void FixedUpdate()
    {
        characterAnimatorHandler.CheckHandIKWeight(characterWeaponSlotManager.rightHandIKTarget, characterWeaponSlotManager.leftHandIKTarget, isTwoHanding);
    }
    public virtual void UpdateUsingHand(bool usingRightHand)
    {
        if(usingRightHand)
        {
            isUsingRightHand = true;
            isUsingLeftHand = false;   
        }
        else
        {
            isUsingLeftHand = true;
            isUsingRightHand = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCollider : MonoBehaviour
{
    public CharacterManager characterManager;
    Collider damageCollider;
    public int currentWeaponDamage;

    [Header("Team I.D")]
    public int teamIDNumber = 0;
    [Header("Poise")]
    public float poiseBreak;
    public float offensivePoiseBonus;
    [Header("Damage")]
    public bool enabledDamageColliderOnStartUp = false;

    protected bool shieldHasBeenHit;
    protected bool hasBeenParried;
    protected string currentDamageAnimation;
    private void Awake()
    {
        damageCollider = GetComponent<Collider>();

        damageCollider.gameObject.SetActive(true);
        damageCollider.enabled = enabledDamageColliderOnStartUp;
        damageCollider.isTrigger = true;
    }

    public void EnableDamageCollider()
    {
        damageCollider.enabled = true;
    }
    public void DisableDamageCollider()
    {
        damageCollider.enabled = false;
    }
    protected virtual void DealDamage(CharacterStatsManager enemyStats)
    {
        float finalDamage = currentWeaponDamage;
        if(characterManager.isUsingRightHand)
        {
            if (characterManager.characterCombatManager.currentAttackType == AttackType.Light)
            {
                finalDamage = characterManager.characterInventoryManager.rightWeapon.lightAttackDamageMultiplier * currentWeaponDamage;
            }
            else if (characterManager.characterCombatManager.currentAttackType == AttackType.Heavy)
            {
                finalDamage = characterManager.characterInventoryManager.rightWeapon.heavyAttackDamageMultiplier * currentWeaponDamage;
            }
        }
        else if(characterManager.isUsingLeftHand)
        {
            if (characterManager.characterCombatManager.currentAttackType == AttackType.Light)
            {
                finalDamage = characterManager.characterInventoryManager.leftWeapon.lightAttackDamageMultiplier * currentWeaponDamage;
            }
            else if (characterManager.characterCombatManager.currentAttackType == AttackType.Heavy)
            {
                finalDamage = characterManager.characterInventoryManager.leftWeapon.heavyAttackDamageMultiplier * currentWeaponDamage;
            }
        }
        if (enemyStats.totalPoiseDefense > poiseBreak)
        {
            enemyStats.TakeDamageNoAnimation(Mathf.RoundToInt(finalDamage));
        }
        else
        {
            enemyStats.TakeDamage(Mathf.RoundToInt(finalDamage), currentDamageAnimation, characterManager);
        }

    }
    protected virtual void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer==9)
        {
            shieldHasBeenHit = false;
            hasBeenParried = false;

            CharacterStatsManager enemyStats = collision.GetComponent<CharacterStatsManager>();
            CharacterManager enemyManager = collision.GetComponent<CharacterManager>();
            CharacterEffectsManager enenmyEffects = collision.GetComponent<CharacterEffectsManager>();
            BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

            if (enemyManager != null)
            {
                if (enemyStats.teamIDNumber == teamIDNumber)
                    return;

                CheckForParry(enemyManager);
                CheckForBlock(enemyManager, enemyStats, shield);
            }

            if (enemyStats != null)
            {
                if (enemyStats.teamIDNumber == teamIDNumber)
                    return;

                if (hasBeenParried)
                    return;

                if (shieldHasBeenHit)
                    return;

                enemyStats.poiseResetTimer = enemyStats.totalPoiseResetTime;
                enemyStats.totalPoiseDefense = enemyStats.totalPoiseDefense - poiseBreak;

                //DETECTS WHERE ON THE COLLIDER OUR WEAPON FIRST MAKES CONTACT
                Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                float directionHitFrom = Vector3.SignedAngle(characterManager.transform.forward, enemyManager.transform.forward, Vector3.up);
                GetDamageDirection(directionHitFrom);
                enenmyEffects.PlayBloodSplatterFX(contactPoint);

                DealDamage(enemyStats);
            }
        }
    }
    protected virtual void CheckForBlock(CharacterManager enemyManager, CharacterStatsManager enemyStatsManager, BlockingCollider shield)
    {
        if (shield != null && enemyManager.isBlocking)
        {
            float physicalDamageAfterBlock = currentWeaponDamage - (currentWeaponDamage * shield.blockPhysicalDamageAbsorption) / 100;

            if (enemyStatsManager != null)
            {
                enemyStatsManager.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block Impact", characterManager);
                shieldHasBeenHit = true;
            }
        }
    }
    protected virtual void CheckForParry(CharacterManager characterManager)
    {
        if (characterManager.isParrying)
        {
            characterManager.GetComponentInChildren<CharacterAnimatorHandler>().PlayTargetAnimation("Parried", true);
            hasBeenParried = true;
        }
    }
    protected virtual void GetDamageDirection(float direction)
    {
        Debug.Log("Direction: " + direction);
        if(direction>= 145 && direction <=180)
        {
            currentDamageAnimation = "Damage_Forward_01";
        }
        else if(direction <= -145 && direction >=-180)
        {
            currentDamageAnimation = "Damage_Forward_01";
        }
        else if (direction >= -45 && direction <= 45)
        {
            currentDamageAnimation = "Damage_Back_01";
        }
        else if (direction >= -144 && direction <= -45)
        {
            currentDamageAnimation = "Damage_Left_01";
        }
        else if(direction >= 45 && direction <=144)
        {
            currentDamageAnimation = "Damage_Right_01";
        }
    }

}

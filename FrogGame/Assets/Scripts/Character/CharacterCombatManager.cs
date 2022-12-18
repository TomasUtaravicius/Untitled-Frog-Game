using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombatManager : MonoBehaviour
{
    CharacterManager character;
    [Header("Attack Type")]
    public AttackType currentAttackType;

    [Header("Weapon Animations")]
    public string oh_light_attack_01 = "OH_LIGHT_ATTACK_01";
    public string oh_light_attack_02 = "OH_LIGHT_ATTACK_02";
    public string oh_heavy_attack_01 = "OH_HEAVY_ATTACK_01";
    public string oh_heavy_attack_02 = "OH_HEAVY_ATTACK_02";
    public string oh_running_attack_01 = "OH_RUNNING_ATTACK_01";
    public string oh_jumping_attack_01 = "OH_JUMPING_ATTACK_01";

    public string th_light_attack_01 = "TH_LIGHT_ATTACK_01";
    public string th_light_attack_02 = "TH_LIGHT_ATTACK_02";
    public string th_heavy_attack_01 = "TH_HEAVY_ATTACK_01";
    public string th_heavy_attack_02 = "TH_HEAVY_ATTACK_02";
    public string th_running_attack_01 = "TH_RUNNING_ATTACK_01";
    public string th_jumping_attack_01 = "TH_JUMPING_ATTACK_01";

    public string oh_charging_attack_01 = "OH_CHARGING_ATTACK_CHARGE_01";
    public string oh_charging_attack_02 = "OH_CHARGING_ATTACK_CHARGE_02";

    public string th_charging_attack_01 = "TH_CHARGING_ATTACK_CHARGE_01";
    public string th_charging_attack_02 = "TH_CHARGING_ATTACK_CHARGE_02";

    public string weapon_art = "WEAPON_ART";

    public string lastAttack;

    LayerMask backStabLayer = 1 << 12;
    LayerMask riposteLayer = 1 << 13;
    private void Awake()
    {
        character = GetComponent<CharacterManager>();
    }
    public virtual void DrainStaminaBasedOnAttack()
    {

    }
    private void SuccesfullyCastSpell()
    {
        character.characterInventoryManager.currentSpell.SuccessfullyCastSpell(character);
        character.animator.SetBool("IsFiringSpell", true);
    }
    public void AttemptBackstabOrRiposte()
    {
        if (character.characterStatsManager.currentStamina <= 0)
        {
            return;
        }

        RaycastHit hit;

        if (Physics.Raycast(character.criticalAttackRayCastStartPoint.position,
            transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
        {
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            DamageCollider rightWeapon = character.characterWeaponSlotManager.rightHandDamageCollider;
            if (enemyCharacterManager != null)
            {
                character.transform.position = enemyCharacterManager.backstabCollider.criticalDamagerStandPoint.position;
                Vector3 rotationDirection = character.transform.root.eulerAngles;
                rotationDirection = hit.transform.position - character.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(character.transform.rotation, tr, 500 * Time.deltaTime);
                character.transform.rotation = targetRotation;

                int criticalDamage = character.characterInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                character.characterAnimatorHandler.PlayTargetAnimation("Backstab", true);
                enemyCharacterManager.GetComponentInChildren<CharacterAnimatorHandler>().PlayTargetAnimation("Getting Backstabbed", true);
            }
        }
        else if (Physics.Raycast(character.criticalAttackRayCastStartPoint.position,
            transform.TransformDirection(Vector3.forward), out hit, 0.7f, riposteLayer))
        {
            CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
            DamageCollider rightWeapon = character.characterWeaponSlotManager.rightHandDamageCollider;
            if (enemyCharacterManager != null && enemyCharacterManager.canBeRiposted)
            {
                Debug.LogError("Riposte");
                character.transform.position = enemyCharacterManager.riposteColllider.criticalDamagerStandPoint.position;

                Vector3 rotationDirection = character.transform.root.eulerAngles;
                rotationDirection = hit.transform.position - character.transform.position;
                rotationDirection.y = 0;
                rotationDirection.Normalize();
                Quaternion tr = Quaternion.LookRotation(rotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(character.transform.rotation, tr, 500 * Time.deltaTime);
                character.transform.rotation = targetRotation;

                int criticalDamage = character.characterInventoryManager.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                character.characterAnimatorHandler.PlayTargetAnimation("Riposte", true);
                enemyCharacterManager.GetComponentInChildren<CharacterAnimatorHandler>().PlayTargetAnimation("Riposted", true);
            }
        }
    }

    
}

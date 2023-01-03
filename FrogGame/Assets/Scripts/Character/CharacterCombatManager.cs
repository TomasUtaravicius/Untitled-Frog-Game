using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCombatManager : MonoBehaviour
{
    CharacterManager character;
    [Header("Attack Type")]
    public AttackType currentAttackType;

    [Header("Combat Transforms")]
    public Transform backstabReceiverTransform;

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
    public float criticalAttackRange = 1f;
    public int pendingCriticalDamage;
    public LayerMask characterLayer;
    protected void Awake()
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
    IEnumerator ForceMoveCharacterToEnemyBackstabPosition(CharacterManager characterPerformingBackstab)
    {
        for(float timer = 0.05f; timer <0.5f; timer+=0.05f)
        {
            Quaternion backstabRotation = Quaternion.LookRotation(characterPerformingBackstab.transform.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, backstabRotation, 1);
            transform.parent = characterPerformingBackstab.characterCombatManager.backstabReceiverTransform;
            transform.localPosition = characterPerformingBackstab.characterCombatManager.backstabReceiverTransform.localPosition;
            transform.parent = null;
            Debug.Log("Running Coroutine");
            yield return new WaitForSeconds(0.05f);
        }
    }
    public void AttemptBackstabOrRiposte()
    {
        if(character.isInteracting)
        {
            return;
        }
        if (character.characterStatsManager.currentStamina <= 0)
        {
            return;
        }

        RaycastHit hit;

        if(Physics.Raycast(character.criticalAttackRayCastStartPoint.transform.position, character.transform.TransformDirection(Vector3.forward), out hit, criticalAttackRange, characterLayer))
        {
            CharacterManager enemyCharacter = hit.transform.GetComponent<CharacterManager>();
            Vector3 directionFromCharacterToEnemy = transform.position - enemyCharacter.transform.position;

            float dotValue = Vector3.Dot(directionFromCharacterToEnemy, enemyCharacter.transform.forward);

            Debug.LogWarning("Current Dot Value is: " + dotValue);

            if(enemyCharacter.canBeRiposted)
            {
                if(dotValue<=1.2f && dotValue>=0.6f)
                {
                    AttemptRiposte(hit);
                    return;
                }
            }
            if(dotValue >= -1f && dotValue <=-0.8f)
            {
                AttemptBackstab(hit);
            }
        }

        
    }
    private void AttemptBackstab(RaycastHit hit)
    {
        CharacterManager enemyCharacter = hit.transform.GetComponent<CharacterManager>();

        if (enemyCharacter != null)
        {
            if (!enemyCharacter.isBeingBackstabbed || !enemyCharacter.isBeingRiposted)
            {
                EnableIsInvulnerable();

                character.isPerformingBackstab = true;
                character.characterAnimatorHandler.EraseHandIKForWeapon();

                character.characterAnimatorHandler.PlayTargetAnimation("Backstab",true);


                int criticalDamage = 100;
                enemyCharacter.pendingCriticalDamage = criticalDamage;
                enemyCharacter.characterCombatManager.GetBackstabbed(character);
            }
        }
    }
    private void AttemptRiposte(RaycastHit hit)
    {
        CharacterManager enemyCharacter = hit.transform.GetComponent<CharacterManager>();

        if (enemyCharacter != null)
        {
            if (!enemyCharacter.isBeingBackstabbed || !enemyCharacter.isBeingRiposted)
            {
                EnableIsInvulnerable();

                character.isPerformingRiposte = true;
                character.characterAnimatorHandler.EraseHandIKForWeapon();

                character.characterAnimatorHandler.PlayTargetAnimation("Riposte", true);


                int criticalDamage = 100;
                enemyCharacter.pendingCriticalDamage = criticalDamage;
                enemyCharacter.characterCombatManager.GetRiposted(character);
            }
        }
    }
    public void GetRiposted(CharacterManager characterPerformingRiposte)
    {
        character.isBeingRiposted = true;

        StartCoroutine(ForceMoveCharacterToEnemyBackstabPosition(characterPerformingRiposte));
        character.characterAnimatorHandler.PlayTargetAnimation("Getting Backstabbed", true);
    }
    public void GetBackstabbed(CharacterManager characterPerformingBackstab)
    {
        character.isBeingBackstabbed = true;

        StartCoroutine(ForceMoveCharacterToEnemyBackstabPosition(characterPerformingBackstab));
        character.characterAnimatorHandler.PlayTargetAnimation("Getting Backstabbed", true);
    }
    private void EnableIsInvulnerable()
    {
        character.animator.SetBool("IsInvulnerable", true);
    }
    public void ApplyPendingDamage()
    {
        character.characterStatsManager.TakeDamageNoAnimation(pendingCriticalDamage);
    }

    
}

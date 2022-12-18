using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Spells/Projectile Spell")]
public class ProjectileSpell : SpellItem
{
    [Header("Projectile Damage")]
    public float baseDamage;
    [Header("Projectile Physics")]
    public float projectileVelocity;
    public float projectileUpwardVelocity;
    public float projectileMass;
    public bool isAffectedByGravity;
    
    Rigidbody rigidbody;
    GameObject instantiatedWarmUpSpellFX;

    public override void AttemptToCastSpell(CharacterManager character)
    {
        base.AttemptToCastSpell(character);
        if(character.isUsingLeftHand)
        {
            
            instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFx, character.characterWeaponSlotManager.leftHandSlot.transform);
            character.characterAnimatorHandler.PlayTargetAnimation(spellAnimation, true, false, character.isUsingLeftHand);
        }
        else
        {
            instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFx, character.characterWeaponSlotManager.rightHandSlot.transform);
            character.characterAnimatorHandler.PlayTargetAnimation(spellAnimation, true, false, character.isUsingLeftHand);
        }
        
    }
    public override void SuccessfullyCastSpell(CharacterManager character)
    {
        base.SuccessfullyCastSpell(character);
        GameObject instantiatedSpellFX;

        PlayerManager player = character as PlayerManager;
        //Player Cast
        if (player != null)
        {
            if (character.isUsingLeftHand)
            {

                instantiatedSpellFX = Instantiate(spellCastFx, player.playerWeaponSlotManager.leftHandSlot.transform.position, player.cameraHandler.cameraPivotTransform.rotation);
                SpellDamageCollider spellDamageCollider = instantiatedSpellFX.GetComponent<SpellDamageCollider>();
                spellDamageCollider.teamIDNumber = character.characterStatsManager.teamIDNumber;
                rigidbody = instantiatedSpellFX.GetComponent<Rigidbody>();
            }
            else
            {
                instantiatedSpellFX = Instantiate(spellCastFx, player.playerWeaponSlotManager.rightHandSlot.transform.position, player.cameraHandler.cameraPivotTransform.rotation);
                SpellDamageCollider spellDamageCollider = instantiatedSpellFX.GetComponent<SpellDamageCollider>();
                spellDamageCollider.teamIDNumber = player.playerStatsManager.teamIDNumber;
                rigidbody = instantiatedSpellFX.GetComponent<Rigidbody>();
            }

            if (player.cameraHandler.currentLockOnTarget != null)
            {
                instantiatedSpellFX.transform.LookAt(player.cameraHandler.currentLockOnTarget.transform);
            }
            else
            {
                instantiatedSpellFX.transform.rotation = Quaternion.Euler(
                    player.cameraHandler.cameraPivotTransform.eulerAngles.x,
                    player.playerStatsManager.transform.eulerAngles.y,
                    0);
            }
            rigidbody.AddForce(instantiatedSpellFX.transform.forward * projectileVelocity);
            rigidbody.AddForce(instantiatedSpellFX.transform.up * projectileUpwardVelocity);
            rigidbody.useGravity = isAffectedByGravity;
            rigidbody.mass = projectileMass;
            Destroy(instantiatedWarmUpSpellFX);
            instantiatedSpellFX.transform.parent = null;
        }
        //AI Cast
        else
        {

        }
        
    }
}

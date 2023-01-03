using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item Actions/Fire Arrow Action")]
public class FireArrowAction : ItemAction
{
    public override void PerformAction(CharacterManager character)
    {
        PlayerManager player = character as PlayerManager;
        Debug.LogWarning("FIRING ARROW ACTION");
        
        //Get Live Arrow Create Transform
        ArrowInstantiationTransform arrowInstantiationTransform;
        arrowInstantiationTransform = character.characterWeaponSlotManager.rightHandSlot.GetComponentInChildren<ArrowInstantiationTransform>();

        //Animate the Bow
        Animator bowAnimator = character.characterWeaponSlotManager.rightHandSlot.currentWeaponModel.GetComponentInChildren<Animator>();
        bowAnimator.SetBool("IsDrawn", false);
        bowAnimator.Play("Bow_Fire_01");

        Destroy(character.characterEffectsManager.instantiatedFXModel);

        character.characterAnimatorHandler.PlayTargetAnimation("Bow_TH_Fire_01", true);
        character.animator.SetBool("IsHoldingArrow", false);

        //Fire Arrow as a player
        if(player!=null)
        {
            GameObject liveArrow = Instantiate(player.playerInventoryManager.currentAmmo.liveAmmoModel, arrowInstantiationTransform.transform.position, player.cameraHandler.cameraPivotTransform.rotation);
            Rigidbody rigidBody = liveArrow.GetComponent<Rigidbody>();
            RangedProjectileDamageCollider damageCollider = liveArrow.GetComponent<RangedProjectileDamageCollider>();
            damageCollider.characterManager = player;
            damageCollider.teamIDNumber = character.GetComponent<CharacterStatsManager>().teamIDNumber;

            if (player.isAiming)
            {
                Ray ray = player.cameraHandler.cameraObject.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
                RaycastHit hitPoint;

                if (Physics.Raycast(ray, out hitPoint, 100f))
                {
                    liveArrow.transform.LookAt(hitPoint.point);
                }
                else
                {
                    liveArrow.transform.rotation = Quaternion.Euler(player.cameraHandler.cameraTransform.localEulerAngles.x, character.lockOnTransform.eulerAngles.y, 0);
                }
            }
            else
            {
                if (player.cameraHandler.currentLockOnTarget != null)
                {
                    Quaternion arrowRotation = Quaternion.LookRotation(player.cameraHandler.currentLockOnTarget.lockOnTransform.position - liveArrow.gameObject.transform.position);
                    liveArrow.transform.rotation = arrowRotation;
                }
                else
                {
                    liveArrow.transform.rotation = Quaternion.Euler(player.cameraHandler.cameraPivotTransform.eulerAngles.x, player.lockOnTransform.eulerAngles.y, 0);
                }
            }
            rigidBody.AddForce(liveArrow.transform.forward * character.characterInventoryManager.currentAmmo.forwardVelocity);
            rigidBody.AddForce(liveArrow.transform.up * character.characterInventoryManager.currentAmmo.upwardVelocity);
            rigidBody.useGravity = character.characterInventoryManager.currentAmmo.useGravity;
            rigidBody.mass = character.characterInventoryManager.currentAmmo.ammoMass;
            liveArrow.transform.parent = null;

            damageCollider.ammoItem = character.characterInventoryManager.currentAmmo;
            damageCollider.currentWeaponDamage = character.characterInventoryManager.currentAmmo.physicalDamage;
        }
        //Fire Arrow as AI
        else
        {
            EnemyManager enemy = character as EnemyManager;
            GameObject liveArrow = Instantiate(character.characterInventoryManager.currentAmmo.liveAmmoModel, arrowInstantiationTransform.transform.position, Quaternion.identity);
            Rigidbody rigidBody = liveArrow.GetComponent<Rigidbody>();
            RangedProjectileDamageCollider damageCollider = liveArrow.GetComponent<RangedProjectileDamageCollider>();
            damageCollider.characterManager = character;
            damageCollider.teamIDNumber = character.GetComponent<CharacterStatsManager>().teamIDNumber;

            if (enemy.currentTarget!=null)
            {
                Quaternion arrowRotation = Quaternion.LookRotation(enemy.currentTarget.lockOnTransform.position - liveArrow.gameObject.transform.position);
                liveArrow.transform.rotation = arrowRotation;
            }

            rigidBody.AddForce(liveArrow.transform.forward * character.characterInventoryManager.currentAmmo.forwardVelocity);
            rigidBody.AddForce(liveArrow.transform.up * character.characterInventoryManager.currentAmmo.upwardVelocity);
            rigidBody.useGravity = character.characterInventoryManager.currentAmmo.useGravity;
            rigidBody.mass = character.characterInventoryManager.currentAmmo.ammoMass;
            liveArrow.transform.parent = null;

            damageCollider.characterManager = character;
            damageCollider.ammoItem = character.characterInventoryManager.currentAmmo;
            damageCollider.currentWeaponDamage = character.characterInventoryManager.currentAmmo.physicalDamage;
            damageCollider.teamIDNumber = enemy.enemyStatsManager.teamIDNumber;
        }

        
    }
}

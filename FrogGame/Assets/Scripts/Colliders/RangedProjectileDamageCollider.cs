using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedProjectileDamageCollider : DamageCollider
{
    public RangedAmmoItem ammoItem;
    protected bool hasAlreadyPenetratedASurface;
    Rigidbody arrowRigidbody;
    CapsuleCollider collider;

    private void Awake()
    {
        collider = GetComponent<CapsuleCollider>();
        arrowRigidbody = GetComponent<Rigidbody>();
        collider.gameObject.SetActive(true);
        collider.enabled = enabledDamageColliderOnStartUp;

    }
    private void OnCollisionEnter(Collision collision)
    {
        shieldHasBeenHit = false;
        hasBeenParried = false;

        CharacterStatsManager enemyStats = collision.gameObject.GetComponentInParent<CharacterStatsManager>();
        CharacterManager enemyManager = collision.gameObject.GetComponentInParent<CharacterManager>();
        CharacterEffectsManager enenmyEffects = collision.gameObject.GetComponentInParent<CharacterEffectsManager>();
        BlockingCollider shield = collision.gameObject.transform.GetComponentInChildren<BlockingCollider>();

        if (enemyManager != null)
        {
            if (enemyStats.teamIDNumber == teamIDNumber)
                return;

            CheckForParry(enemyManager);
            CheckForBlock(enemyManager, enemyStats, shield);

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

                if (enemyStats.totalPoiseDefense > poiseBreak)
                {
                    enemyStats.TakeDamageNoAnimation(currentWeaponDamage);
                }
                else
                {
                    enemyStats.TakeDamage(currentWeaponDamage, currentDamageAnimation, characterManager);
                }
            
        }

        
        if (!hasAlreadyPenetratedASurface)
        {
            hasAlreadyPenetratedASurface = true;
            arrowRigidbody.isKinematic = true;
            collider.enabled = false;

            gameObject.transform.position = collision.GetContact(0).point;
            gameObject.transform.rotation = Quaternion.LookRotation(transform.forward);
            gameObject.transform.parent = collision.collider.transform;
        }
    }

    private void FixedUpdate()
    {
        if (arrowRigidbody.velocity != Vector3.zero)
        {
            arrowRigidbody.rotation = Quaternion.LookRotation(arrowRigidbody.velocity);
        }
    }
}

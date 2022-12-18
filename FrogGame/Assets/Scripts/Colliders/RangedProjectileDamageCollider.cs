using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedProjectileDamageCollider : DamageCollider
{
    public RangedAmmoItem ammoItem;
    protected bool hasAlreadyPenetratedASurface;
    protected GameObject penetratedProjectile;
    protected override void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.layer == 9)
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

                if (enemyStats.totalPoiseDefense > poiseBreak)
                {
                    enemyStats.TakeDamageNoAnimation(currentWeaponDamage);
                }
                else
                {
                    enemyStats.TakeDamage(currentWeaponDamage, currentDamageAnimation, characterManager);
                }
            }
            
        }
        if (!hasAlreadyPenetratedASurface && penetratedProjectile == null)
        {
            hasAlreadyPenetratedASurface = true;
            Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

            GameObject penetratedArrow = Instantiate(ammoItem.penetratedModel, contactPoint, Quaternion.Euler(0, 0, 0));

            penetratedProjectile = penetratedArrow;

            penetratedArrow.transform.parent = collision.transform;
            penetratedArrow.transform.rotation = Quaternion.LookRotation(gameObject.transform.forward);


        }

        Destroy(transform.root.gameObject);
    }
}

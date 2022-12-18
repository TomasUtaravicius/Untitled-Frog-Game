using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDamageCollider : DamageCollider
{
    public GameObject impactParticles;
    public GameObject projectileParticles;
    public GameObject muzzleParticles;

    bool hasCollided = false;

    CharacterStatsManager spellTarget;
    Rigidbody rigidbody;

    Vector3 impactNormal;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        projectileParticles = Instantiate(projectileParticles, transform.position, transform.rotation);
        projectileParticles.transform.parent = transform;

        if(muzzleParticles)
        {
            muzzleParticles = Instantiate(muzzleParticles, transform.position, transform.rotation);
            Destroy(muzzleParticles, 2f);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(!hasCollided)
        {
            spellTarget = collision.transform.GetComponent<CharacterStatsManager>();
            CharacterManager enemyManager = collision.gameObject.GetComponent<CharacterManager>();
            if (spellTarget!=null && spellTarget.teamIDNumber!=teamIDNumber)
            {
                float directionHitFrom = Vector3.SignedAngle(characterManager.transform.forward, enemyManager.transform.forward, Vector3.up);
                GetDamageDirection(directionHitFrom);
                spellTarget.TakeDamage(currentWeaponDamage, currentDamageAnimation, characterManager);
            }
            hasCollided = true;
            impactParticles = Instantiate(impactParticles, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal));

            Destroy(projectileParticles);
            Destroy(impactParticles, 2f);
            Destroy(gameObject, 5f);
        }
    }

}

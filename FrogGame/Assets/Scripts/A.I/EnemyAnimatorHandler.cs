using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorHandler : CharacterAnimatorHandler
{
    EnemyManager enemy;
    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<EnemyManager>();
    }
    public void AwardSoulsOnDeath()
    {
        PlayerStatsManager playerStats = FindObjectOfType<PlayerStatsManager>();
        SoulCountBar soulCountBar = FindObjectOfType<SoulCountBar>();
        if (playerStats != null)
        {
            playerStats.AddSouls(enemy.enemyStatsManager.soulsAwardedOnDeath);
            if (soulCountBar != null)
            {
                soulCountBar.SetSoulCountText(playerStats.soulCount);
            }
        }
        
    }
    public void PlayWeaponTrailFX()
    {
        enemy.enemyEffectsManager.PlayWeaponFX(false);
    }
    
    public void PowerUp()
    {
        character.transform.localScale = new Vector3(1f, 1f, 1f);
    }
    private void OnAnimatorMove()
    {
        float delta = Time.deltaTime;
        enemy.rigidbody.drag = 0;
        Vector3 deltaPosition = character.animator.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / delta;
        enemy.rigidbody.velocity = velocity;

        if(character.isRotatingWithRootMotion)
        {
            enemy.transform.rotation *= enemy.animator.deltaRotation;
        }
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatsManager : CharacterStatsManager
{
    EnemyManager enemy;
    public EnemyHealthBarUI enemyHealthBar;


    public bool isBoss;
    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<EnemyManager>();
        currentHealth = maxHealth;
    }
    private void Start()
    {
        if(!isBoss)
        {
            enemyHealthBar.SetMaxHealth(maxHealth);
        }
    }
    public override void HandlePoiseResetTimer()
    {
        if (poiseResetTimer > 0)
        {
            poiseResetTimer = poiseResetTimer - Time.deltaTime;
        }
        else if (poiseResetTimer <= 0 && !enemy.isInteracting)
        {
            totalPoiseDefense = armorPoiseBonus;
        }
    }
  
    public override void TakeDamageNoAnimation(int damage)
    {
        if (enemy.isDead)
        {
            return;
        }

        base.TakeDamageNoAnimation(damage);

        if (!isBoss)
        {
            enemyHealthBar.SetHealth(currentHealth);
        }
        else if (isBoss && enemy.enemyBossManager != null)
        {
            enemy.enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);

        }
        if(currentHealth<=0)
        {
            enemy.isDead = true;
        }
    }
    public void  BreakGuard()
    {
        enemy.enemyAnimatorHandler.PlayTargetAnimation("Break Guard", true);
    }
    public override void TakeDamage(int damage,string damageAnimation, CharacterManager characterDamagingMe)
    {
        Debug.LogWarning("Taking Damage with animation");
        if (enemy.isDead)
        {
            return;
        }
        if(!isBoss)
        {
            base.TakeDamage(damage, damageAnimation, characterDamagingMe);
            enemyHealthBar.SetHealth(currentHealth);
            enemy.enemyAnimatorHandler.PlayTargetAnimation(damageAnimation, true);
        }
        else if(isBoss && enemy.enemyBossManager!=null)
        {
            if (enemy.isShiftingPhase)
            {
                TakeDamageNoAnimation(damage);
                return;
            }
            else
            {
                base.TakeDamage(damage, damageAnimation, characterDamagingMe);
                enemy.enemyAnimatorHandler.PlayTargetAnimation(damageAnimation, true);
                enemy.enemyBossManager.UpdateBossHealthBar(currentHealth, maxHealth);

            }
        }

        if (currentHealth <= 0)
        {
            HandleDeath();
        }
    }
    private void HandleDeath()
    {
        currentHealth = 0;
        enemy.enemyAnimatorHandler.PlayTargetAnimation("Death_01", true);
        enemy.isDead = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossManager : MonoBehaviour
{
    EnemyManager enemyManager;

    public string bossName;

    BossHealthBarUI bossHealthBar;
    BossCombatStanceState bossCombatStanceState;

    [Header("Second Phase FX")]
    public GameObject secondPhaseFX;
    
    private void Awake()
    {
        bossHealthBar = FindObjectOfType<BossHealthBarUI>();
        enemyManager = GetComponent<EnemyManager>();
        bossCombatStanceState = GetComponentInChildren<BossCombatStanceState>();
    }
    private void Start()
    {
        bossHealthBar.SetBossName(bossName);
        bossHealthBar.SetBossMaxHealth(enemyManager.enemyStatsManager.maxHealth);
    }
    public void UpdateBossHealthBar(int currentHealth, int maxHealth)
    {
        bossHealthBar.SetBossCurrentHealth(currentHealth);
        if (currentHealth <= maxHealth / 2)
        {
            if (bossCombatStanceState.hasPhaseShifted)
                return;
            ShiftToSecondPhase();
            enemyManager.animator.speed = 1.5f;
        }
    }
    public void ShiftToSecondPhase()
    {
        enemyManager.animator.SetBool("IsInvulnerable",true);
        enemyManager.animator.SetBool("IsShiftingPhase", true);
        enemyManager.enemyAnimatorHandler.PlayTargetAnimation("Power Shift", true);
        bossCombatStanceState.hasPhaseShifted = true;
    }
}

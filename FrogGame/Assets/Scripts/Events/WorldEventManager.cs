using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldEventManager : MonoBehaviour
{
    BossHealthBarUI bossHealthBar;
    EnemyBossManager boss;

    public bool bossFightIsActive;
    public bool bossHasBeenAwakened;
    public bool bossHasBeenDefeated;


    private void Awake()
    {
        bossHealthBar = FindObjectOfType<BossHealthBarUI>();
    }

    public void ActivateBossFight()
    {
        bossFightIsActive = true;
        bossHasBeenAwakened = true;
        bossHealthBar.SetUIHealthBarToActive();
    }

    public void BossHasBeenDefeated()
    {
        bossFightIsActive = false;
        bossHasBeenDefeated = false;
    }
}

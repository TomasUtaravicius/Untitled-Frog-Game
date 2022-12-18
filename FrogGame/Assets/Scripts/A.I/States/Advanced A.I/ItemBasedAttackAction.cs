using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "A.I/Humanoid Actions/Item Based Attack Action")]
public class ItemBasedAttackAction : ScriptableObject
{
    [Header("Attack Type")]
    public AIAttackActionType attackActionType = AIAttackActionType.MeleeAttackAction;
    public AttackType attackType = AttackType.Light;

    [Header("Action Combo Settings")]
    public bool actionCanCombo = false;

    [Header("Right Hand or Left Hand Action")]
    bool isRightHandedAction = true;
    public int attackScore = 3;
    public float recoveryTime = 2;

    public float maximumAttackAngle = 35;
    public float minimumAttackAngle = -35;

    public float minimumDistanceNeededToAttack = 0;
    public float maximumDistanceNeededToAttack = 3;

    public void PerformAttackAction(EnemyManager enemy)
    {
        if(isRightHandedAction)
        {
            enemy.UpdateUsingHand(true);
            PerformRightHandItemAction(enemy);
        }
        else
        {
            enemy.UpdateUsingHand(false);
            PerformLeftHandItemAction(enemy);
        }
    }
    private void PerformRightHandItemAction(EnemyManager enemy)
    {
        if(attackActionType == AIAttackActionType.MeleeAttackAction)
        {
            PerformRightHandedMeleeAction(enemy);
        }
        else if(attackActionType == AIAttackActionType.RangedAttackAction)
        {

        }
        else if(attackActionType == AIAttackActionType.MagicAttackAction)
        {

        }
    }
    private void PerformLeftHandItemAction(EnemyManager enemy)
    {
        if (attackActionType == AIAttackActionType.MeleeAttackAction)
        {

        }
        else if (attackActionType == AIAttackActionType.RangedAttackAction)
        {

        }
        else if (attackActionType == AIAttackActionType.MagicAttackAction)
        {

        }
    }
    private void PerformRightHandedMeleeAction(EnemyManager enemy)
    {
        if(enemy.isTwoHanding)
        {
            if(attackType== AttackType.Light)
            {
                enemy.characterInventoryManager.rightWeapon.th_tap_RB_Action.PerformAction(enemy);
            }
            else if(attackType == AttackType.Heavy)
            {
                enemy.characterInventoryManager.rightWeapon.th_tap_RT_Action.PerformAction(enemy);
            }
        }
        else
        {
            if (attackType == AttackType.Light)
            {
                enemy.characterInventoryManager.rightWeapon.tap_RB_Action.PerformAction(enemy);
            }
            else if (attackType == AttackType.Heavy)
            {
                enemy.characterInventoryManager.rightWeapon.tap_RT_Action.PerformAction(enemy);
            }
        }
    }

}

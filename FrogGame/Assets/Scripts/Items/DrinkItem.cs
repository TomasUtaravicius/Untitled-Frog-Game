using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Consumables/Drink")]
public class DrinkItem : ConsumableItem
{
    [Header("Flask Type")]
    public bool healthDrink;
    public bool focusDrink;

    [Header("Recovery Amount")]
    public int healthRecoverAmount;
    public int focusPointsRecoverAmount;

    [Header("Recovery FX")]
    public GameObject recoveryFX;

    public override void AttemptToConsumeItem(PlayerAnimatorHandler playerAnimatorHandler, PlayerWeaponSlotManager weaponSlotManager, PlayerEffectsManager playerEffectsManager)
    {
        base.AttemptToConsumeItem(playerAnimatorHandler,weaponSlotManager,playerEffectsManager);
        playerEffectsManager.currentParticleFX = recoveryFX;
        playerEffectsManager.amountToBeHealed = healthRecoverAmount;
        playerEffectsManager.instantiatedFXModel = Instantiate(itemModel, weaponSlotManager.leftHandSlot.transform);
        weaponSlotManager.leftHandSlot.UnloadItem();
        

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsManager : CharacterEffectsManager
{
    PlayerManager player;

    public GameObject currentParticleFX;
    public GameObject instantiatedFXModel;
    public int amountToBeHealed;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }
    public void HealPlayerFromEffect()
    {
        player.playerStatsManager.HealCharacter(amountToBeHealed);
        GameObject healFX = Instantiate(currentParticleFX, player.playerStatsManager.transform);
        Destroy(instantiatedFXModel);
        player.playerWeaponSlotManager.LoadBothWeaponsOnSlots();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Item Actions/Blocking Action")]
public class BlockingAction : ItemAction
{
    public override void PerformAction(CharacterManager character)
    {
        if (character.isInteracting)
        {
            return;
        }
        if (character.isBlocking)
        {
            return;
        }

        character.characterAnimatorHandler.PlayTargetAnimation("Block Start", false, true);
        character.characterEquipmentManager.OpenBlockingCollider();
        character.isBlocking = true;
    }
}

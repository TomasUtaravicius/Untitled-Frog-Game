using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEquipmentManager : MonoBehaviour
{
    CharacterManager character;
    private void Awake()
    {
        character = GetComponent<CharacterManager>();
    }
    public virtual void OpenBlockingCollider()
    {
        if (character.isTwoHanding)
        {
            character.blockingCollider.SetColliderDamageAbsorption(character.characterInventoryManager.rightWeapon);
        }
        else
        {
            character.blockingCollider.SetColliderDamageAbsorption(character.characterInventoryManager.leftWeapon);
        }

        character.blockingCollider.EnableBlockingCollider();
    }
    public virtual void CloseBlockingCollider()
    {
        character.blockingCollider.DisableBlockingCollider();
    }
}

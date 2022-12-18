using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickSlotsUI : MonoBehaviour
{
    public Image leftHandIcon;
    public Image rightHandIcon;
    public Image currentConsumableIcon;
    public Image currentSpellIcon;

    public void UpdateWeaponQuickSlotsUI(bool isLeft,WeaponItem weapon)
    {
        if(isLeft)
        {
            if(weapon.itemIcon!=null)
            {
                leftHandIcon.enabled = true;
                leftHandIcon.sprite = weapon.itemIcon;
            }
            else
            {
                leftHandIcon.sprite = null;
                leftHandIcon.enabled = false; 
            }
         
        }
        else
        {
            if(weapon.itemIcon)
            {
                rightHandIcon.enabled = true;
                rightHandIcon.sprite = weapon.itemIcon;
            }
            else
            {
                rightHandIcon.sprite = null;
                rightHandIcon.enabled = false;
            }
            
        }
    }
    public void UpdateCurrentSpellIcon(SpellItem spell)
    {
        if(spell.itemIcon!=null)
        {
            currentSpellIcon.sprite = spell.itemIcon;
            currentSpellIcon.enabled = true;
        }
        else
        {
            currentSpellIcon.sprite = null;
            currentSpellIcon.enabled = false;
        }
    }
    public void UpdateCurrentConsumableIcon(ConsumableItem consumable)
    {
        if (consumable.itemIcon != null)
        {
            currentConsumableIcon.sprite = consumable.itemIcon;
            currentConsumableIcon.enabled = true;
        }
        else
        {
            currentConsumableIcon.sprite = null;
            currentConsumableIcon.enabled = false;
        }
    }
}

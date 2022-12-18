using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffectsManager : MonoBehaviour
{
    public CharacterManager character;
    [Header("Character FX")]
    public GameObject bloodSplatterFX;
    [Header("Weapon FX")]
    public WeaponFX rightWeaponFX;
    public WeaponFX leftWeaponFX;

    [Header("Current Range FX")]
    public GameObject currentRangeFX;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
    }
   public virtual void PlayWeaponFX(bool isLeft)
    {
        if(!isLeft)
        {
            if(rightWeaponFX!=null)
            {
                rightWeaponFX.PlayWeaponFX();
            }
        }
        else
        {
            if(leftWeaponFX!=null)
            {
                leftWeaponFX.PlayWeaponFX();
            }

        }
    }
    public virtual void StopWeaponFX(bool isLeft)
    {
        if (!isLeft)
        {
            if (rightWeaponFX != null)
            {
                rightWeaponFX.StopWeaponFX();
            }
        }
        else
        {
            if (leftWeaponFX != null)
            {
                leftWeaponFX.StopWeaponFX();
            }

        }
    }
    public virtual void PlayBloodSplatterFX(Vector3 bloodSplatterLocation)
   {
        GameObject blood = Instantiate(bloodSplatterFX, bloodSplatterLocation, Quaternion.identity);
   }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFX : MonoBehaviour
{
    [Header("Weapon FX")]
    public ParticleSystem normalWeaponTrail;
    public MeleeWeaponTrail weaponTrail;
    public GameObject normalWeaponTrailGO;

    public void PlayWeaponFX()
    {
        //weaponTrail.Emit = false;
        weaponTrail.Emit = true;
        //Invoke("StopWeaponFX", 0.8f);
    }
    public void StopWeaponFX()
    {
        weaponTrail.Emit = false;
    }
}

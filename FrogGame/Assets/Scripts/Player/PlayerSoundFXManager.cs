using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundFXManager : CharacterSoundFXManager
{
    PlayerManager player;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        player = GetComponent<PlayerManager>();
    }
    public override void PlayRandomDamageSoundFX()
    {
        potentialDamageSounds = new List<AudioClip>();

        foreach (var damageSound in takingDamageSounds)
        {
            if (damageSound != lastDamageSoundPlayed)
            {
                potentialDamageSounds.Add(damageSound);
            }
        }

        
        int randomValue = Random.Range(0, potentialDamageSounds.Count);
        lastDamageSoundPlayed = takingDamageSounds[randomValue];
        audioSource.PlayOneShot(lastDamageSoundPlayed);
    }
    public override void PlayRandomWeaponWhoosh()
    {
        potentialWeaponWhooses = new List<AudioClip>();


            foreach (var whooshSound in player.playerInventoryManager.meleeWeapon.weaponWhooses)
            {
                if (whooshSound != lastWeaponWhoosh)
                {
                    potentialWeaponWhooses.Add(whooshSound);
                }
                int randomValue = Random.Range(0, potentialWeaponWhooses.Count);
                lastWeaponWhoosh = player.playerInventoryManager.meleeWeapon.weaponWhooses[randomValue];
                audioSource.PlayOneShot(player.playerInventoryManager.meleeWeapon.weaponWhooses[randomValue]);
            }
        


    }
}

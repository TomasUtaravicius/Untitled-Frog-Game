using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundFXManager : MonoBehaviour
{
    CharacterManager player;
    public AudioSource audioSource;
    //Attacking Grunts

    //Taking Damage Grunts

    [Header("Taking Damage Sounds")]
    public AudioClip[] takingDamageSounds;
    protected List<AudioClip> potentialDamageSounds;
    protected AudioClip lastDamageSoundPlayed;

    [Header("Weapon Whooses")]
    protected List<AudioClip> potentialWeaponWhooses;
    protected AudioClip lastWeaponWhoosh;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        player = GetComponent<CharacterManager>();
    }
    public virtual void PlayRandomDamageSoundFX()
    {
        potentialDamageSounds = new List<AudioClip>();

        foreach(var damageSound in takingDamageSounds)
        {
            if(damageSound!=lastDamageSoundPlayed)
            {
                potentialDamageSounds.Add(damageSound);
            }
        }

        int randomValue = Random.Range(0, potentialDamageSounds.Count);
        lastDamageSoundPlayed = takingDamageSounds[randomValue];
        audioSource.PlayOneShot(lastDamageSoundPlayed);
    }
    public virtual void PlayRandomWeaponWhoosh()
    {
        potentialWeaponWhooses = new List<AudioClip>();
        if(player.isUsingLeftHand)
        {
            foreach(var whooshSound in player.characterInventoryManager.leftWeapon.weaponWhooses)
            {
                if(whooshSound!=lastWeaponWhoosh)
                {
                    potentialWeaponWhooses.Add(whooshSound);
                }
                int randomValue = Random.Range(0, potentialWeaponWhooses.Count);
                lastWeaponWhoosh = player.characterInventoryManager.leftWeapon.weaponWhooses[randomValue];
                audioSource.PlayOneShot(player.characterInventoryManager.leftWeapon.weaponWhooses[randomValue]);
            }
        }
        else if(player.isUsingRightHand)
        {
            foreach (var whooshSound in player.characterInventoryManager.rightWeapon.weaponWhooses)
            {
                if (whooshSound != lastWeaponWhoosh)
                {
                    potentialWeaponWhooses.Add(whooshSound);
                }
                int randomValue = Random.Range(0, potentialWeaponWhooses.Count);
                lastWeaponWhoosh = player.characterInventoryManager.rightWeapon.weaponWhooses[randomValue];
                audioSource.PlayOneShot(player.characterInventoryManager.rightWeapon.weaponWhooses[randomValue]);
            }
        }
        

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSoundFXManager : MonoBehaviour
{
    CharacterManager character;
    public AudioSource audioSource;
    //Attacking Grunts

    //Taking Damage Grunts

    [Header("Taking Damage Sounds")]
    public AudioClip[] takingDamageSounds;
    private List<AudioClip> potentialDamageSounds;
    private AudioClip lastDamageSoundPlayed;

    [Header("Weapon Whooses")]
    private List<AudioClip> potentialWeaponWhooses;
    private AudioClip lastWeaponWhoosh;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        character = GetComponent<CharacterManager>();
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
        if(character.isUsingLeftHand)
        {
            foreach(var whooshSound in character.characterInventoryManager.leftWeapon.weaponWhooses)
            {
                if(whooshSound!=lastWeaponWhoosh)
                {
                    potentialWeaponWhooses.Add(whooshSound);
                }
                int randomValue = Random.Range(0, potentialWeaponWhooses.Count);
                lastWeaponWhoosh = character.characterInventoryManager.leftWeapon.weaponWhooses[randomValue];
                audioSource.PlayOneShot(character.characterInventoryManager.leftWeapon.weaponWhooses[randomValue]);
            }
        }
        else if(character.isUsingRightHand)
        {
            foreach (var whooshSound in character.characterInventoryManager.rightWeapon.weaponWhooses)
            {
                if (whooshSound != lastWeaponWhoosh)
                {
                    potentialWeaponWhooses.Add(whooshSound);
                }
                int randomValue = Random.Range(0, potentialWeaponWhooses.Count);
                lastWeaponWhoosh = character.characterInventoryManager.rightWeapon.weaponWhooses[randomValue];
                audioSource.PlayOneShot(character.characterInventoryManager.rightWeapon.weaponWhooses[randomValue]);
            }
        }
        

    }
}

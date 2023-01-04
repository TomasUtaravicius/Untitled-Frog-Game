using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundFXManager : CharacterSoundFXManager
{
    PlayerManager player;
    public AudioSource footStepSource;
    public AudioClip[] stoneClips;
    public AudioClip[] dirtClips;
    public AudioClip[] sandClips;
    public AudioClip[] grassClips;
    public CheckTerrainTexture terrainChecker;
    AudioClip previousClip;

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
    
    public void PlayFootstep()
    {
        terrainChecker.GetTerrainTexture();
        if (terrainChecker.textureValues[0] > 0)
        {
            footStepSource.PlayOneShot(GetClip(stoneClips), terrainChecker.textureValues[0]);
        }
        if (terrainChecker.textureValues[1] > 0)
        {
            footStepSource.PlayOneShot(GetClip(grassClips), terrainChecker.textureValues[1]);
        }
        if (terrainChecker.textureValues[2] > 0)
        {
            footStepSource.PlayOneShot(GetClip(dirtClips), terrainChecker.textureValues[2]);
        }
        if (terrainChecker.textureValues[3] > 0)
        {
            footStepSource.PlayOneShot(GetClip(sandClips), terrainChecker.textureValues[3]);
        }
    }
    AudioClip GetClip(AudioClip[] clipArray)
    {
        int attempts = 3;
        AudioClip selectedClip =
        clipArray[Random.Range(0, clipArray.Length - 1)];
        while (selectedClip == previousClip && attempts > 0)
        {
            selectedClip =
            clipArray[Random.Range(0, clipArray.Length - 1)];

            attempts--;
        }
        previousClip = selectedClip;
        return selectedClip;
    }

}

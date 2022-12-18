using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    public CharacterManager character;
    [Header("Team I.D")]
    public int teamIDNumber = 0;
    public int healthLevel = 10;
    public int maxHealth;
    public int currentHealth;

    public int staminaLevel = 10;
    public float maxStamina;
    public float currentStamina;


    public int focusLevel;
    public float maxFocusPoints;
    public float currentFocusPoints;

    public int soulCount = 0;

    [Header("Poise")]
    public float totalPoiseDefense;
    public float offensivePoiseBonus;
    public float armorPoiseBonus;
    public float totalPoiseResetTime = 15;
    public float poiseResetTimer = 0;
    public int soulsAwardedOnDeath = 50;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
        maxHealth = SetMaxHealthFromHealthLevel();
        currentHealth = maxHealth;
    }
    private void Start()
    {
        totalPoiseDefense = armorPoiseBonus;
    }
    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }
    protected virtual void Update()
    {
        HandlePoiseResetTimer();
    }
    public virtual void HealCharacter(int healAmount)
    {
        currentHealth += healAmount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        
    }
    public virtual void TakeDamage(int damage, string damageAnimation, CharacterManager enemyCharacterDamagingMe)
    {
        if (character.isDead)
            return;

        
        character.characterAnimatorHandler.EraseHandIKForWeapon();

        int finalDamage = damage;
        if (enemyCharacterDamagingMe.isPerformingFullyChargedAttack)
        {
            finalDamage = finalDamage * 2; //Double Damage if attack is fully charged
        }
        currentHealth = currentHealth - finalDamage;
        if(currentHealth<=0)
        {
            currentHealth = 0;
            character.isDead = true;
        }
        character.characterSoundFXManager.PlayRandomDamageSoundFX();
    }
    public virtual void TakeDamageNoAnimation(int damage)
    {
        if (character.isDead)
            return;

        currentHealth = currentHealth - damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            character.isDead = true;
        }
        character.characterSoundFXManager.PlayRandomDamageSoundFX();
    }
    public virtual void HandlePoiseResetTimer()
    {
        if(poiseResetTimer >0)
        {
            poiseResetTimer = poiseResetTimer - Time.deltaTime;
        }
        else
        {
            totalPoiseDefense = armorPoiseBonus;
        }
    }
    public virtual void DrainStamina(float amount)
    {
        currentStamina = currentStamina - amount;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : CharacterStatsManager
{
    PlayerManager player;

    public float staminaRegenerationAmount = 30f;
    public float staminaRegenerationTimer = 1f;
    public HealthBar healthBar;
    public StaminaBar staminaBar;
    public FocusPointBar focusPointBar;
    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }
    private void Start()
    {
        
        maxHealth = SetMaxHealthFromHealthLevel();
        maxStamina = SetMaxStaminaFromStaminaLevel();
        maxFocusPoints = SetMaxFocusPointsFromFocusLevel();

        currentStamina = maxStamina;
        currentHealth = maxHealth;
        currentFocusPoints = maxFocusPoints;

        //healthBar.SetMaxHealth(maxHealth);
        //staminaBar.SetMaxStamina(maxStamina);
        //focusPointBar.SetMaxFocusPoints(maxFocusPoints);
        //focusPointBar.SetCurrentFocusPoints(currentFocusPoints);
    }
    public override void HandlePoiseResetTimer()
    {
        if (poiseResetTimer > 0)
        {
            poiseResetTimer = poiseResetTimer - Time.deltaTime;
        }
        else if(poiseResetTimer <=0 && !player.isInteracting)
        {
            totalPoiseDefense = armorPoiseBonus;
        }
    }
    private int SetMaxHealthFromHealthLevel()
    {
        maxHealth = healthLevel * 10;
        return maxHealth;
    }
    private float SetMaxFocusPointsFromFocusLevel()
    {
        maxFocusPoints = focusLevel * 10;
        return maxFocusPoints;
    }
    private float SetMaxStaminaFromStaminaLevel()
    {
        maxStamina = staminaLevel * 10;
        return maxStamina;
    }

    public override void HealCharacter(int healAmount)
    {
        base.HealCharacter(healAmount);
        healthBar.SetCurrentHealth(currentHealth);
    }
    public override void TakeDamageNoAnimation(int damage)
    {
        if (player.isDead)
            return;

        base.TakeDamageNoAnimation(damage);
        healthBar.SetCurrentHealth(currentHealth);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            player.playerAnimatorHandler.PlayTargetAnimation("Death_01", true);
            player.isDead = true;
        }
    }
    public override void TakeDamage(int damage, string damageAnimation, CharacterManager characterDamagingMe)
    {
        if(player.isInvulnerable)
        {
            return;
        }
        if (player.isDead)
            return;

        base.TakeDamage(damage, damageAnimation,characterDamagingMe);
        healthBar.SetCurrentHealth(currentHealth);

        player.playerWeaponSlotManager.CloseDamageCollider();
        player.playerAnimatorHandler.PlayTargetAnimation(damageAnimation, true);
    
        if (currentHealth <=0)
        {
            currentHealth = 0;
            player.playerAnimatorHandler.PlayTargetAnimation("Death_01", true);
            player.isDead = true;
        }
    }
    public override void DrainStamina(float amount)
    {
        base.DrainStamina(amount);
        staminaBar.SetCurrentStamina(currentStamina);
    }
    public void TakeFocusPointDrain(float drainAmount)
    {
        currentFocusPoints = currentFocusPoints - drainAmount;

        Debug.Log("Focus points drained");
        if(currentFocusPoints < 0)
        {
            currentFocusPoints = 0;
        }
        focusPointBar.SetCurrentFocusPoints(currentFocusPoints);
    }
    public void RegenerateStamina()
    {
        if(player.isInteracting)
        {
            staminaRegenerationTimer = 0;
        }
        else
        {
            staminaRegenerationTimer += Time.deltaTime;
            if (currentStamina < maxStamina && staminaRegenerationTimer > 1f)
            {
                currentStamina += staminaRegenerationAmount * Time.deltaTime;
                staminaBar.SetCurrentStamina(Mathf.RoundToInt(currentStamina));
            }
        }
        
        
    }
    public void AddSouls(int souls)
    {
        soulCount = soulCount + souls;
    }
    
}

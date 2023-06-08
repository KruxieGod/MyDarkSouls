using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG;
using static Models;

public class PlayerStats : CharacterStats
{
    public bool IsInvulnerability;
    public PlayerStatistics playerStatistics;

    public int MaxHealth;
    public int CurrentHealth;

    public int MaxStamina;
    public int CurrentStamina;

    private AnimatorManager animator;
    private PlayerManager playerManager;

    public HealthBar healthBar;
    public StaminaBar staminaBar;
    private float time = 0;
    private Coroutine energyRechargeCorountine = null;
    public float energyRecharge;
    private void Awake()
    {
        healthBar = FindObjectOfType<HealthBar>();
        staminaBar = FindObjectOfType<StaminaBar>();
        playerManager = GetComponent<PlayerManager>();
        animator = GetComponent<AnimatorManager>();
    }

    void Start()
    {
        MaxStamina = SetMaxStaminaFromStaminaLevel();
        CurrentStamina = MaxStamina;
        staminaBar.SetMaxStamina(CurrentStamina);

        MaxHealth = SetMaxHealthFromHealthLevel();
        CurrentHealth = MaxHealth;
        healthBar.SetMaxHealth(MaxHealth);
    }

    private int SetMaxStaminaFromStaminaLevel()
    {
        MaxStamina += playerStatistics.StaminaLevel * 10;
        return MaxStamina;
    }

    private int SetMaxHealthFromHealthLevel()
    {
        MaxHealth = playerStatistics.HealthLevel * 10;
        return MaxHealth;
    }

    public void AddHealth(int damage)
    {
        CurrentHealth = Mathf.Clamp(damage+ CurrentHealth,0,MaxHealth);
        healthBar.SetCurrentHealth(CurrentHealth);
    }

    public void TakeDamage(int damage,bool withInteracting = true) 
    {
        CurrentHealth -= damage;
        healthBar.SetCurrentHealth(CurrentHealth);
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            animator.PlayTargetAnimation("Death", true,true);
            return;
        }
        animator.PlayTargetAnimation("Damage", false,withInteracting? true: playerManager.isInteracting);
    }

    public void TakeStaminaDamage(int damage)
    {
        CurrentStamina-= damage;
        CurrentStamina = Mathf.Clamp(CurrentStamina, 0, MaxStamina);
        staminaBar.SetCurrentStamina(CurrentStamina);
        if (energyRechargeCorountine != null)
            StopCoroutine(energyRechargeCorountine);
        energyRechargeCorountine = StartCoroutine(StaminaBreak());
    }

    private void Update()
    {
        if (energyRechargeCorountine == null && CurrentStamina != MaxStamina)
            StaminaRest();
    }

    private void StaminaRest()
    {
        time += Time.deltaTime;
        if (time > energyRecharge)
        {
            time = 0f;
            CurrentStamina++;
            staminaBar.SetCurrentStamina(CurrentStamina);
        }
    }

    private IEnumerator StaminaBreak()
    {
        yield return new WaitForSeconds(2f);
        energyRechargeCorountine = null;
    }

    public void EnableInvulnerability()
    {
        IsInvulnerability = true;
    }

    public void DisableInvulnerability()
    {
        IsInvulnerability = false;
    }
}

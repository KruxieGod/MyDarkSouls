using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG;
using static Models;

public class PlayerStats : CharacterStats
{
    private PlayerAttacker playerAttacker;
    public override bool IsHeavyAttack => playerAttacker.IsHeavy;
    public int SoulsAmount { get; private set; }
    public override bool IsInvulnerability { get; protected set; }
    public PlayerStatistics playerStatistics;

    public int MaxHealth;

    public int MaxStamina;
    public int CurrentStamina;

    private AnimatorManager animator;
    private PlayerManager playerManager;
    private PlayerInventory playerInventory;
    private IHealthBar healthBar;
    public StaminaBar staminaBar;
    private float time = 0;
    private Coroutine energyRechargeCorountine = null;
    public float energyRecharge;
    private SoulsBar soulsBar;
    private void Awake()
    {
        playerInventory = GetComponent<PlayerInventory>();  
        playerAttacker = GetComponent<PlayerAttacker>();    
        soulsBar = FindObjectOfType<SoulsBar>();
        staminaBar = FindObjectOfType<StaminaBar>();
        playerManager = GetComponent<PlayerManager>();
        animator = GetComponent<AnimatorManager>();
    }

    void Start()
    {
        healthBar = FindObjectOfType<UIManager>().GetComponentInChildren<HealthBar>();
        healthBar.Initialize();
        MaxStamina = SetMaxStaminaFromStaminaLevel();
        CurrentStamina = MaxStamina;
        staminaBar.SetMaxStamina(CurrentStamina);

        MaxHealth = SetMaxHealthFromHealthLevel();
        CurrentHealth = MaxHealth;
        healthBar.SetMaxHealth(MaxHealth);
    }

    public void AddSouls(EnemyStats enemyStats)
    {
        SoulsAmount += enemyStats.SoulsCount;
        ISoulsBar soulsbar = soulsBar;
        soulsbar.SetSouls(SoulsAmount);
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

    public override void TakeDamage(int damage,bool withInteracting = true) 
    {
        base.TakeDamage(damage);

        healthBar.SetCurrentHealth(CurrentHealth);
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            animator.PlayTargetAnimation("Death", true,true);
            return;
        }
        animator.PlayTargetAnimation(playerInventory.LeftWeapon.DamageAnimation, false,withInteracting? true: playerManager.isInteracting);
    }

    public override void TakeStaminaDamage(int damage)
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

    public override float GetSofteningBlowInPercent()
    {
        return playerInventory.LeftWeapon.GetSofteningBlowInPercent();
    }
}

internal interface ISoulsBar
{
    internal void SetSouls(int count);
}

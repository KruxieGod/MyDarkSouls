using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    [SerializeField] private float speedCollision;
    private IHealthBar healthBar;
    public Func<IHealthBar> UpdateHealthBar;
    public override bool IsInvulnerability { get => animator.GetBool("IsInvulnerability"); protected set => IsInvulnerability = value; }
    private EnemyManager enemyManager;
    public override bool IsHeavyAttack { get { if (enemyManager.CurrentAttackAction.Item1 != null) return enemyManager.CurrentAttackAction.Item1.IsHeavy;
                    return false; } }
    [SerializeField]private int soulsCount;
    public int SoulsCount => soulsCount;
    public int HealthLevel = 10;
    public int MaxHealth; 

    public bool IsDeath { get; private set; }
    private Animator animator;

    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        animator = GetComponent<Animator>();
    }

    public void InitializeEvents()
    {
        MaxHealth = SetMaxHealthFromHealthLevel();
        CurrentHealth = MaxHealth;
        healthBar = UpdateHealthBar();
        healthBar.Initialize();
        healthBar.SetMaxHealth(CurrentHealth);
    }

    public void SetName(string name)
    {
        healthBar.SetName(name);
    }

    private int SetMaxHealthFromHealthLevel()
    {
        MaxHealth = HealthLevel * 10;
        return MaxHealth;
    }

    public void TakeCriticalDamage()
    {
        TakeDamage(CriticalDamage, true);
    }

    public override void TakeDamage(int damage,bool withoutAnimation = false)
    {
        CurrentHealth -= damage;
        healthBar.SetCurrentHealth(CurrentHealth);
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            IsDeath = true;
            animator.SetBool("IsDead", true);
            if (!withoutAnimation)
                animator.CrossFade("Death", 0.2f);
        }
        animator.CrossFade("Damage",0.2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("TAG : "+ collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.GetComponent<CharacterController>().SimpleMove((collision.transform.position - transform.position)*speedCollision);
    }
}

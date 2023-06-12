using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    public override bool IsInvulnerability { get => animator.GetBool("IsInvulnerability"); protected set => IsInvulnerability = value; }
    private EnemyManager enemyManager;
    public override bool IsHeavyAttack => enemyManager.CurrentAttackAction.Item1.IsHeavy;
    [SerializeField]private int soulsCount;
    public int SoulsCount => soulsCount;
    public int HealthLevel = 10;
    public int MaxHealth;
    public int CurrentHealth;
    public bool IsDeath { get; private set; }
    private Animator animator;

    private void Awake()
    {
        enemyManager = GetComponent<EnemyManager>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        MaxHealth = SetMaxHealthFromHealthLevel();
        CurrentHealth = MaxHealth;
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
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            IsDeath = true;
            animator.SetBool("IsDead", true);
        }
        if (withoutAnimation)
            return;
        else if (CurrentHealth == 0)
        {
            IsDeath = true;
            animator.CrossFade("Death",0.2f);
            return;
        }
        animator.CrossFade("Damage",0.2f);
    }
}

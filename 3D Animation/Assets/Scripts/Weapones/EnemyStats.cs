using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : CharacterStats
{
    public int HealthLevel = 10;
    public int MaxHealth;
    public int CurrentHealth;

    private Animator animator;

    private void Awake()
    {
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

    public void TakeDamage(int damage,bool withoutAnimation = false)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            animator.SetBool("IsDead", true);
        }
        if (withoutAnimation)
            return;
        else if (CurrentHealth == 0)
        {
            animator.CrossFade("Death",0.2f);
            return;
        }
        animator.CrossFade("Damage",0.2f);
    }
}

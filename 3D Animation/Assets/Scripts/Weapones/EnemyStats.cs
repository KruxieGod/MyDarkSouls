using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyStats 
    : CharacterStats
{
    private EnemyAnimatorManager enemyAnimatorManager;
    [SerializeField] private float speedCollision;
    private IHealthBar healthBar;
    internal Func<IHealthBar> UpdateHealthBar;
    public override bool IsInvulnerability { get => animator.GetBool("IsInvulnerability"); 
        set { 
            animator.SetBool("IsInvulnerability", value); 
            animator.Play("Empty", animator.GetLayerIndex("Damage")); 
        } 
    }
    private EnemyManager enemyManager;
    public override bool IsHeavyAttack => enemyManager.CurrentAttackAction.Item1 != null && enemyManager.CurrentAttackAction.Item1.IsHeavy;
    [SerializeField]private int soulsCount;
    public int SoulsCount => soulsCount;
    [SerializeField]private int healthLevel = 10;
    [SerializeField]private int maxHealth; 

    public bool IsDeath { get; private set; }
    private Animator animator;

    private void Awake()
    {
        enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
        enemyManager = GetComponent<EnemyManager>();
        animator = GetComponent<Animator>();
    }

    internal override void Update()
    {
        base.Update();
    }

    public void SetOffHealthBar()
    {
        Debug.Log("OFF ENEMYSTATS");
        healthBar.SetOff();
    }

    public void InitializeEvents()
    {
        maxHealth = SetMaxHealthFromHealthLevel();
        CurrentHealth = maxHealth;
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
        maxHealth = healthLevel * 10;
        return maxHealth;
    }

    public void TakeCriticalDamage()
    {
        TakeDamage(CriticalDamage, true);
    }

    public override void TakeDamage(int damage,bool withoutAnimation = false)
    {
        base.TakeDamage(damage);
        GlobalEventManager.OnBossPhases.Invoke();
        healthBar.SetCurrentHealth(CurrentHealth);
        if (CurrentHealth <= 0)
        {
            CurrentHealth = 0;
            IsDeath = true;
            animator.SetBool("IsDead", true);
            if (!withoutAnimation)
                animator.CrossFade("Death", 0.2f);
        }
        if (!damageIsAnimated || enemyAnimatorManager.IsInteracting) return;
        enemyAnimatorManager.PlayTargetAnimation("Damage",true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("TAG : "+ collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.GetComponent<CharacterController>().SimpleMove((collision.transform.position - transform.position)*speedCollision);
    }
}

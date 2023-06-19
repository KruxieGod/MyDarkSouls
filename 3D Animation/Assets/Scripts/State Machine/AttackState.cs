using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    [SerializeField] private CombatStanceState combatStance;
    public PursueTargetState PursueTarget;
    [SerializeField] private DeathState deathState;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager,EnemyLocomotion enemyLocomotion)
    {
        enemyLocomotion.Agent.isStopped = true;
        if (enemyAnimatorManager.Animator.GetBool("IsParried"))
        {
            enemyAnimatorManager.CanDoCombo = false;
            enemyAnimatorManager.DisableCombo();
            return combatStance;
        }
        enemyManager.PlayAttack();
        enemyLocomotion.Agent.isStopped = false ;

        if (enemyStats.IsDeath)
            return deathState;
        //Attack the Player and if Player is far from enemy will switch to Combat Stance State
        //Select attack random or Player is far or nearby
        //Timer recovery
        return combatStance;
    }
}

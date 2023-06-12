using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PursueTargetState : State
{
    [SerializeField] private CombatStanceState combatStanceState;
    public AttackState AttackStanceState;
    public IdleState IdleState;
    [SerializeField]private DeathState deathState;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager,EnemyLocomotion enemyLocomotion)
    {
        enemyLocomotion.Agent.isStopped = false;
        enemyLocomotion.PursuitPlayer();
        if (enemyLocomotion.CharacterManager == null)
            return IdleState;
        if (enemyManager.GetAttack())
            return combatStanceState;
        if (enemyStats.IsDeath)
            return deathState;
        // Chase the target
        // If necessary switch to Combat Stance 
        return this;
    }
}

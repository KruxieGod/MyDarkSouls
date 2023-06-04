using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PursueTargetState : State
{
    public AttackState AttackStanceState;
    public IdleState IdleState;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager,EnemyLocomotion enemyLocomotion)
    {
        enemyLocomotion.Agent.isStopped = false;
        enemyLocomotion.PursuitPlayer();
        if (enemyLocomotion.CharacterManager == null)
            return IdleState;
        if (enemyManager.GetAttack())
            return AttackStanceState;
        // Chase the target
        // If necessary switch to Combat Stance 
        return this;
    }
}

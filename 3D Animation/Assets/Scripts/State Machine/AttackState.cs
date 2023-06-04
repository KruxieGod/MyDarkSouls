using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    public PursueTargetState PursueTarget;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager,EnemyLocomotion enemyLocomotion)
    {
        enemyLocomotion.Agent.isStopped = true;
        enemyManager.PlayAttack();
        if (!enemyAnimatorManager.Animator.GetBool("IsUsingRootMotion"))
            return PursueTarget;
        //Attack the player and if player is far from enemy will switch to Combat Stance State
        //Select attack random or player is far or nearby
        //Timer recovery
        return this;
    }
}

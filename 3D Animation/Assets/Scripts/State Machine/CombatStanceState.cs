using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStanceState : State
{
    public PursueTargetState PursueTarget;
    [SerializeField] private AttackState attackState;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager,EnemyLocomotion enemyLocomotion)
    {
        if (enemyManager.GetAttack())
            return attackState;

        if (!enemyAnimatorManager.Animator.GetBool("IsUsingRootMotion"))
            return PursueTarget;
        // Switch to attack if target is nearby or another movement, circle Player
        return this;
    }
}

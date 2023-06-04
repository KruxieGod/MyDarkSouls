using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStanceState : State
{
    public PursueTargetState PursueTarget;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager,EnemyLocomotion enemyLocomotion)
    {
        enemyManager.PlayAttack();
        if (!enemyAnimatorManager.Animator.GetBool("IsUsingRootMotion"))
            return PursueTarget;
        // Switch to attack if target is nearby or another movement, circle player
        return this;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PursueTargetState : State
{
    internal override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager,EnemyLocomotion enemyLocomotion)
    {
        if (enemyManager.IsInteracting)
            return this;
        enemyLocomotion.PursuitPlayer();
        if (enemyLocomotion.CharacterManager == null)
            return new IdleState();
        if (enemyManager.GetAttack())
            return new CombatStanceState();
        if (enemyStats.IsDeath)
            return new DeathState();
        //// Chase the target
        //// If necessary switch to Combat Stance 
        return this;
    }
}

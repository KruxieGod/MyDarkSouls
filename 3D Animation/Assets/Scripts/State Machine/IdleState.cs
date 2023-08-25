using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : State
{
    internal override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager,EnemyLocomotion enemyLocomotion)
    {
        //enemyLocomotion.Wandering();
        if (enemyManager.SearchTarget())
        {
            return new PursueTargetState();
        }
        if (enemyStats.IsDeath)
            return new DeathState();
        enemyAnimatorManager.ResetMovementValues();
        return this;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class IdleState : State
{
    public PursueTargetState PursueTarget;
    [SerializeField]private DeathState deathState;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager,EnemyLocomotion enemyLocomotion)
    {
        //enemyLocomotion.Wandering();
        if (enemyManager.SearchTarget())
        {
            Debug.Log(enemyManager.SearchTarget());
            return PursueTarget;
        }
        if (enemyStats.IsDeath)
            return deathState;
        // Look for a target
        // Switch to pursue target state
        return this;
    }
}

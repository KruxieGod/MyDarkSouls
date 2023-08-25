using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class SleepState : State
{
    private bool isSleeping;
    internal override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager, EnemyLocomotion enemyLocomotion)
    {
        if (!isSleeping)
        {
            isSleeping = true;
            enemyAnimatorManager.PlayTargetAnimation("LayingIdle");
        }
        if (enemyManager.SearchTarget())
        {
            enemyAnimatorManager.PlayTargetAnimation("GettingUp",true,false,0.03f);
            return new PursueTargetState();
        }
        return this;
    }
}

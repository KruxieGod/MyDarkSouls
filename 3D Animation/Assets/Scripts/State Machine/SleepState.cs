using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class SleepState : State
{
    public PursueTargetState TargetState;
    private bool isSleeping;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager, EnemyLocomotion enemyLocomotion)
    {
        if (!isSleeping)
        {
            isSleeping = true;
            Debug.Log("Sleep");
            enemyAnimatorManager.PlayTargetAnimation("LayingIdle");
        }
        if (enemyManager.SearchTarget())
        {
            Debug.Log("Зашел");
            enemyAnimatorManager.PlayTargetAnimation("GettingUp",true,0.03f);
            return TargetState;
        }
        return this;
    }
}

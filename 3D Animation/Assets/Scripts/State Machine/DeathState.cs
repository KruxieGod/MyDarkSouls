using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : State
{
    internal override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager, EnemyLocomotion enemyLocomotion)
    {
        return this;
    }
}

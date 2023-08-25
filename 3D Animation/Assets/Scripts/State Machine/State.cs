using SG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
    internal static MonoBehaviour GameObject;

    internal abstract State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager, EnemyLocomotion enemyLocomotion);
}

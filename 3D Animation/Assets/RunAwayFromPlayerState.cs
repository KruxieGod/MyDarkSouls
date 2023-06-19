using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAwayFromPlayerState : State
{
    [SerializeField] private CombatStanceState combatStance;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager, EnemyLocomotion enemyLocomotion)
    {
        if (enemyManager.AnyAttackIsRecovered())
        {
            return combatStance;
        }
        enemyAnimatorManager.Animator.SetFloat("Vertical",-0.5f,0.1f,Time.deltaTime);
        enemyAnimatorManager.Animator.SetFloat("Horizontal", 0f, 0.1f, Time.deltaTime);
        enemyLocomotion.CombatStanceUpdateRotation();
        return this;
    }
}

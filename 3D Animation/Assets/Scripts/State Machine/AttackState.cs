using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State
{
    internal override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager,EnemyLocomotion enemyLocomotion)
    {
        if (enemyManager.IsInteracting)
            return new CombatStanceState();
        enemyLocomotion.Agent.isStopped = true;
        if (enemyAnimatorManager.Animator.GetBool("IsParried"))
        {
            enemyAnimatorManager.CanDoCombo = false;
            enemyAnimatorManager.DisableCombo();
            return new CombatStanceState();
        }
        enemyManager.GetComponent<EnemyWeaponSlotManager>().PlayParticlesWeapon();
        enemyManager.PlayAttack();
        enemyLocomotion.Agent.isStopped = false ;

        if (enemyStats.IsDeath)
            return new DeathState();
        //Attack the Player and if Player is far from enemy will switch to Combat Stance State
        //Select attack random or Player is far or nearby
        //Timer recovery
        return new CombatStanceState();
    }
}

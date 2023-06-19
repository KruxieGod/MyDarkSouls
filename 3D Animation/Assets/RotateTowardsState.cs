using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsState : State
{
    [SerializeField] private CombatStanceState combatStanceState;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager, EnemyLocomotion enemyLocomotion)
    {
        if (enemyAnimatorManager.IsUsingRootMotion)
            return combatStanceState;
        enemyAnimatorManager.Animator.SetFloat("Horizontal", 0,0.1f,Time.deltaTime);
        enemyAnimatorManager.Animator.SetFloat("Vertical", 0,0.1f, Time.deltaTime);
        float angleToTarget = Vector3.SignedAngle(enemyManager.transform.forward, enemyLocomotion.CharacterManager.transform.position - enemyManager.transform.position, Vector3.up);

        if (angleToTarget > 45 && angleToTarget <= 100)
            enemyAnimatorManager.PlayerTargetAnimationWithRootRotation("TurnRight",true);
        else if (angleToTarget < -45 && angleToTarget >= -100)
            enemyAnimatorManager.PlayerTargetAnimationWithRootRotation("TurnLeft",true);
        else if ((angleToTarget <= -100 && angleToTarget >= -181) || (angleToTarget >= 100 && angleToTarget <= 181))
            enemyAnimatorManager.PlayerTargetAnimationWithRootRotation("TurnBack", true);

        return combatStanceState;
    }
}

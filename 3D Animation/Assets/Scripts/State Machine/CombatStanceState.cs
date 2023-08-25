
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStanceState : State
{
    private bool isWandering;
    private float horizontalDirection;
    private float verticalDirection;
    private IEnumerator wanderingRecoverCoroutine;
    private bool canPursue;

    internal override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager,EnemyLocomotion enemyLocomotion)
    {
        if (enemyManager.IsInteracting)
            return new CombatStanceState();
        if (enemyStats.IsDeath)
            return new DeathState();
        bool isAttacking = enemyAnimatorManager.Animator.GetBool(enemyManager.IsAttacking);
        if (enemyAnimatorManager.Animator.GetBool("IsParried"))
        {
            ResetCombo(enemyAnimatorManager);
            return this;
        }
        if (!isWandering && !isAttacking)
        {
            if (wanderingRecoverCoroutine != null)
                GameObject.StopCoroutine(wanderingRecoverCoroutine);
            wanderingRecoverCoroutine = WanderingRandomAround();
            GameObject.StartCoroutine(wanderingRecoverCoroutine);
        }

        if (enemyManager.GetAttack())
        {
            if (wanderingRecoverCoroutine != null)
                GameObject.StopCoroutine(wanderingRecoverCoroutine);
            return new AttackState();
        }
        bool anyAttackIsRecovered = enemyManager.AnyAttackIsRecovered();
        if (!anyAttackIsRecovered && enemyLocomotion.IsClosedWithTarget())
        {
            if (wanderingRecoverCoroutine != null)
                GameObject.StopCoroutine(wanderingRecoverCoroutine);
            ResetCombo(enemyAnimatorManager);
            return new RunAwayFromPlayerState();
        }

        if (!isAttacking && anyAttackIsRecovered && canPursue)
        {
            ResetCombo(enemyAnimatorManager);
            return new PursueTargetState();
        }

        float angleToTarget = Vector3.SignedAngle(enemyManager.transform.forward, enemyLocomotion.CharacterManager.transform.position - enemyManager.transform.position, Vector3.up);
        if (angleToTarget > 45 || angleToTarget < -45)
        {
            return new RotateTowardsState();
        }
        enemyAnimatorManager.Animator.SetFloat("Horizontal", horizontalDirection, 0.1f, Time.deltaTime);
        enemyAnimatorManager.Animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
        if (!isAttacking)
            enemyLocomotion.CombatStanceUpdateRotation();
        return this;
    }

    private static void ResetCombo(EnemyAnimatorManager enemyAnimatorManager)
    {
        enemyAnimatorManager.IsComboing = false;
        enemyAnimatorManager.CanDoCombo = false;
        enemyAnimatorManager.DisableCombo();
    }

    private IEnumerator WanderingRandomAround()
    {
        horizontalDirection = Random.Range(0, 1f) >= 0.5f? 0.5f: -0.5f;
        verticalDirection = Random.Range(0, 1f) >= 0.5f?0.5f:0f;
        isWandering = true;
        yield return new WaitForSeconds(3f);
        Debug.Log("Reset Coroutine");
        canPursue = true;
    }
}

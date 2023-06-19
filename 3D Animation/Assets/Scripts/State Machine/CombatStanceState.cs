
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatStanceState : State
{
    [SerializeField] private RunAwayFromPlayerState runFromPlayerState;
    [SerializeField]private RotateTowardsState rotateTowardsState;
    public PursueTargetState PursueTarget;
    [SerializeField] private AttackState attackState;
    private bool isWandering;
    private float horizontalDirection;
    private float verticalDirection;
    private IEnumerator wanderingRecoverCoroutine;
    private bool canPursue;
    public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager,EnemyLocomotion enemyLocomotion)
    {
        bool isUsedRootMotion = enemyAnimatorManager.Animator.GetBool("IsUsingRootMotion");
        if (enemyAnimatorManager.Animator.GetBool("IsParried"))
        {
            ResetCombo(enemyAnimatorManager);
            return this;
        }
        if (!isWandering && !isUsedRootMotion)
        {
            if (wanderingRecoverCoroutine != null)
                StopCoroutine(wanderingRecoverCoroutine);
            wanderingRecoverCoroutine = WanderingRandomAround();
            StartCoroutine(wanderingRecoverCoroutine);
        }

        if (enemyManager.GetAttack())
        {
            if (wanderingRecoverCoroutine != null)
                StopCoroutine(wanderingRecoverCoroutine);
            ResetFlags();
            return attackState;
        }
        bool anyAttackIsRecovered = enemyManager.AnyAttackIsRecovered();
        if (!anyAttackIsRecovered && enemyLocomotion.IsClosedWithTarget())
        {
            if (wanderingRecoverCoroutine != null)
                StopCoroutine(wanderingRecoverCoroutine);
            ResetCombo(enemyAnimatorManager);
            ResetFlags();
            return runFromPlayerState;
        }

        if (!isUsedRootMotion && anyAttackIsRecovered && canPursue)
        {
            ResetCombo(enemyAnimatorManager);
            ResetFlags();
            return PursueTarget;
        }

        float angleToTarget = Vector3.SignedAngle(enemyManager.transform.forward, enemyLocomotion.CharacterManager.transform.position - enemyManager.transform.position, Vector3.up);
        if (angleToTarget > 45 || angleToTarget < -45)
        {
            ResetFlags();
            return rotateTowardsState;
        }
        enemyAnimatorManager.Animator.SetFloat("Horizontal", horizontalDirection, 0.1f, Time.deltaTime);
        enemyAnimatorManager.Animator.SetFloat("Vertical", 0f, 0.1f, Time.deltaTime);
        enemyLocomotion.CombatStanceUpdateRotation();
        return this;
    }

    private static void ResetCombo(EnemyAnimatorManager enemyAnimatorManager)
    {
        enemyAnimatorManager.IsComboing = false;
        enemyAnimatorManager.CanDoCombo = false;
        enemyAnimatorManager.DisableCombo();
    }

    private void ResetFlags()
    {
        isWandering = false;
        canPursue = false;
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

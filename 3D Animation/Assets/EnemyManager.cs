using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG;
using UnityEngine.AI;
using System.Linq;

public class EnemyManager : CharacterManager
{
    private EnemyStats enemyStats;
    public State CurrentState;
    private EnemyLocomotion enemyLocomotion;
    private EnemyAnimatorManager enemyAnimatorManager;
    public EnemyAttackAction[] EnemyAttackActions;
    public EnemyAttackAction CurrentAttackAction;
    public Coroutine RecoverCoroutine;
    public bool IsUsingRootMotion { get { return enemyAnimatorManager.Animator.GetBool("IsUsingRootMotion"); } }
    private void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        enemyLocomotion = GetComponent<EnemyLocomotion>();
        enemyAnimatorManager= GetComponent<EnemyAnimatorManager>();
    }

    private void FixedUpdate()
    {
        enemyLocomotion.HandleAllMovement();
    }

    private void Update()
    {
        HandleStateMachine();
    }

    private void HandleStateMachine()
    {
        if (CurrentState != null)
        {
            State nextState = CurrentState.Tick(this, enemyStats, enemyAnimatorManager, enemyLocomotion);
            if (nextState != null)
            {
                SwitchToNextState(nextState);
            }
        }
    }

    private void SwitchToNextState(State state)
    {
        CurrentState = state; 
    }

    public bool GetAttack()
    {
        if (RecoverCoroutine != null ||
            enemyAnimatorManager.Animator.GetBool("IsUsingRootMotion"))
            return false;
        Vector3 targetDirection = enemyLocomotion.CharacterManager.transform.position - transform.position;
        float viewAngle = Vector3.Angle(transform.forward, targetDirection);
        EnemyAttackAction attack = EnemyAttackActions[Random.Range(0, EnemyAttackActions.Length)];
        if (targetDirection.magnitude <= attack.MaximumAttackDistance &&
            targetDirection.magnitude >= attack.MinimumAttackDistance &&
            RecoverCoroutine == null &&
            viewAngle <= enemyLocomotion.Angle)
        {
            Debug.Log("Attack");
            CurrentAttackAction = attack;
            return true;
        }
        else 
             CurrentAttackAction = null;
        return false;
    }

    public void PlayAttack()
    {
        if (CurrentAttackAction == null)
            return;
        if (enemyAnimatorManager.Animator.GetBool("IsUsingRootMotion"))
            return;
        if (RecoverCoroutine != null)
            return;
        enemyAnimatorManager.PlayTargetAnimation(CurrentAttackAction.AnimationName,true);
        RecoverCoroutine = StartCoroutine(RecoveryTimer(CurrentAttackAction.RecoveryTime));
    }

    private IEnumerator RecoveryTimer(float time)
    {
        Debug.Log(time);
        yield return new WaitForSeconds(time);
        RecoverCoroutine = null;
    }

    public bool SearchTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, enemyLocomotion.DistanceDetection, enemyLocomotion.LayerDetections);
        if (colliders.Length > 0)
        {
            var target = colliders.OrderBy(x => Vector3.Distance(transform.position, x.transform.position))
                .FirstOrDefault(x =>
                {
                    return Vector3.Distance(x.transform.position, transform.position) <= enemyLocomotion.DistanceDetection &&
                    Vector3.Angle(transform.forward, x.transform.position - transform.position) <= enemyLocomotion.Angle && x.transform != transform;
                });
            if (target != null && target.transform != transform)
            {
                Debug.Log(target.name);
                enemyLocomotion.CharacterManager = target.GetComponent<CharacterManager>();
            }
            if (enemyLocomotion.CharacterManager != null)
                return true;
        }
        return false;
    }
}

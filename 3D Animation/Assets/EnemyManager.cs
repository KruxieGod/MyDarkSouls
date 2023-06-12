using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG;
using UnityEngine.AI;
using System.Linq;
using System;

public class EnemyManager : CharacterManager
{
    public override bool IsParried { get { return enemyAnimatorManager.Animator.GetBool("IsParried"); } }
    private EnemyStats enemyStats;
    public State CurrentState;
    private EnemyLocomotion enemyLocomotion;
    private EnemyAnimatorManager enemyAnimatorManager;
    public EnemyAttackAction[] EnemyAttackActions;
    private Dictionary<int,Coroutine> recoveryAttacks= new Dictionary<int,Coroutine>();
    public Tuple<EnemyAttackAction,int> CurrentAttackAction { get; private set; }
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
        foreach (var item in recoveryAttacks)
        {
            Debug.Log("Dictionary key: "+ item.Key +" "+ item.Value);
        }
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
        if ((enemyAnimatorManager.Animator.GetBool("IsUsingRootMotion")&&!enemyAnimatorManager.CanDoCombo) || enemyAnimatorManager.IsComboing)
            return false;
        if (enemyAnimatorManager.CanDoCombo)
        {
            enemyAnimatorManager.CanDoCombo = false;
            enemyAnimatorManager.IsComboing = true;
        }
        var excludedAttacks = EnemyAttackActions
            .Where(
            (x,index) => !recoveryAttacks.ContainsKey(index) || recoveryAttacks[index] == null)
            .ToArray();
        if (excludedAttacks.Length == 0)
            return false;
        var index = UnityEngine.Random.Range(0, excludedAttacks.Length);
        EnemyAttackAction attack = excludedAttacks[index];
        Vector3 targetDirection = enemyLocomotion.CharacterManager.transform.position - transform.position;
        float viewAngle = Vector3.Angle(transform.forward, targetDirection);
        if (targetDirection.magnitude <= attack.MaximumAttackDistance &&
            targetDirection.magnitude >= attack.MinimumAttackDistance &&
            viewAngle <= enemyLocomotion.Angle)
        {
            Debug.Log("Attack");
            CurrentAttackAction = new (attack, index);
            return true;
        }
        else 
             CurrentAttackAction = null;
        return false;
    }

    public void PlayAttack()
    {
        if ((recoveryAttacks.ContainsKey(CurrentAttackAction.Item2) && recoveryAttacks[CurrentAttackAction.Item2] != null) || 
            enemyAnimatorManager.Animator.GetBool("IsUsingRootMotion"))
            return;
        Debug.Log("HEISATTACKING");
        enemyAnimatorManager.PlayTargetAnimation(CurrentAttackAction.Item1.AnimationName,true);
        recoveryAttacks[CurrentAttackAction.Item2] = StartCoroutine(
            RecoveryTimer(CurrentAttackAction.Item2, CurrentAttackAction.Item1.RecoveryTime));
    }

    private IEnumerator RecoveryTimer(int index,float time)
    {
        yield return new WaitForSeconds(time);
        recoveryAttacks[index] = null;
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

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Вошел в коллайдер");
    }
}

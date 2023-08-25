using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class EnemyManager : CharacterManager
{
    public string IsAttacking => "IsAttacking";
    public string IsInteractingAnimation => "IsInteracting";
    public override bool IsParried { get { return enemyAnimatorManager.Animator.GetBool("IsParried"); } }
    private EnemyStats enemyStats;
    public State CurrentState { get; private set; }
    private EnemyLocomotion enemyLocomotion;
    private EnemyAnimatorManager enemyAnimatorManager;
    [SerializeField]private EnemyAttackAction[] enemyAttackActions;
    private List<(int, EnemyAttackAction)> enemyActions = new List<(int, EnemyAttackAction)>();
    private List<Coroutine> recoveryAttacks= new List<Coroutine>();
    public Tuple<EnemyAttackAction,int> CurrentAttackAction { get; private set; }
    public bool IsInteracting { get { return enemyAnimatorManager.Animator.GetBool(IsInteractingAnimation); } }

    private void Start()
    {
        State.GameObject = this;
        CurrentState = new IdleState();
        enemyStats = GetComponent<EnemyStats>();
        enemyLocomotion = GetComponent<EnemyLocomotion>();
        enemyAnimatorManager= GetComponent<EnemyAnimatorManager>();
        AddNewAttacks(enemyAttackActions);
    }

    internal void AddNewAttacks(EnemyAttackAction[] enemyAttacks)
    {
        enemyActions = enemyAttacks.Select((item, index) => (enemyActions.Count + index, item)).ToList();
        for (int i = 0; i < enemyAttacks.Length; i++)
            recoveryAttacks.Add(null);
    }

    internal void ResetAttacks()
    {
        recoveryAttacks.Clear();
        enemyActions = enemyAttackActions.Select((item, index) => (index, item)).ToList();
        for (int i = 0; i < enemyActions.Count; i++)
            recoveryAttacks.Add(null);
    }

    internal bool IsClosedWithTarget()
    {
        if (enemyLocomotion.Agent.remainingDistance < 0.01f)
            return false;
        return enemyLocomotion.Agent.remainingDistance < enemyLocomotion.Agent.stoppingDistance+0.1f;
    }

    internal bool AnyAttackIsRecovered()
    {
        return recoveryAttacks.Any(x => x == null);
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

    internal bool GetAttack()
    {
        if (enemyAnimatorManager.Animator.GetBool(IsInteractingAnimation))
            return false;
        if ((enemyAnimatorManager.Animator.GetBool(IsAttacking)&&!enemyAnimatorManager.CanDoCombo) || enemyAnimatorManager.IsComboing)
            return false;
        if (enemyAnimatorManager.CanDoCombo)
        {
            enemyAnimatorManager.CanDoCombo = false;
            enemyAnimatorManager.IsComboing = true;
        }
        var excludedAttacks = enemyActions
            .Where(
            x => recoveryAttacks[x.Item1] == null)
            .ToArray();
        if (excludedAttacks.Length == 0)
            return false;
        var index = UnityEngine.Random.Range(0, excludedAttacks.Length);
        EnemyAttackAction attack = excludedAttacks[index].Item2;
        Vector3 targetDirection = enemyLocomotion.CharacterManager.transform.position - transform.position;
        float viewAngle = Vector3.Angle(transform.forward, targetDirection);
        if (targetDirection.magnitude <= attack.MaximumAttackDistance &&
            targetDirection.magnitude >= attack.MinimumAttackDistance &&
            viewAngle <= enemyLocomotion.Angle)
        {
            CurrentAttackAction = new (attack, excludedAttacks[index].Item1);
            return true;
        }
        else 
             CurrentAttackAction = null;
        return false;
    }

    internal void PlayAttack()
    {
        enemyLocomotion.Agent.isStopped = true;
        enemyAnimatorManager.Animator.SetBool(IsAttacking,true);
        enemyAnimatorManager.PlayTargetAnimation(CurrentAttackAction.Item1.AnimationName);
        recoveryAttacks[CurrentAttackAction.Item2] = StartCoroutine(
            RecoveryTimer(CurrentAttackAction.Item2, CurrentAttackAction.Item1.RecoveryTime));
    }

    private IEnumerator RecoveryTimer(int index,float time)
    {
        yield return new WaitForSeconds(time);
        recoveryAttacks[index] = null;
    }

    internal bool SearchTarget()
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

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimatorManager : CharacterAnimator
{
    public Animator Animator;
    private EnemyLocomotion enemyLocomotion;
    private EnemyStats enemyStats;
    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        enemyLocomotion = GetComponent<EnemyLocomotion>();
        Animator = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        Animator.SetFloat("MovementSpeed", enemyLocomotion.Agent.velocity.magnitude,0.2f,Time.deltaTime);
    }

    public override void PlayTargetAnimation(string targetAnimation, bool useRootMotion = false, bool isInteracting = false, float time = 0)
    {
        Animator.SetBool("IsUsingRootMotion", useRootMotion);
        float animationTransition = time < 0.00001f ? 0.2f : time;
        Animator.CrossFade(targetAnimation, animationTransition);
    }

    private void OnAnimatorMove()
    {
        if (Animator.GetBool("IsUsingRootMotion") && Time.timeScale > 0.1f)
        {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.isStopped = true;
            Vector3 deltaPosition = Animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = Time.deltaTime > 0? deltaPosition / Time.deltaTime: Vector3.zero;
            agent.velocity = velocity;
        }
    }
}

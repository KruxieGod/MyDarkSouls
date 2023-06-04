using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimatorManager : MonoBehaviour
{
    public Animator Animator;
    private EnemyLocomotion enemyLocomotion;
    void Start()
    {
        enemyLocomotion = GetComponent<EnemyLocomotion>();
        Animator = GetComponent<Animator>();
    }

    private void LateUpdate()
    {
        Animator.SetFloat("MovementSpeed", enemyLocomotion.Agent.velocity.magnitude,0.2f,Time.deltaTime);
    }

    public void PlayTargetAnimation(string targetAnimation,bool useRootMotion = false,float time = 0f)
    {
        Animator.SetBool("IsUsingRootMotion", useRootMotion);
        float animationTransition = time < 0.00001f ? 0.2f: time;
        Animator.CrossFade(targetAnimation, animationTransition);
    }

    private void OnAnimatorMove()
    {
        if (Animator.GetBool("IsUsingRootMotion") && Time.timeScale > 0.1f)
        {
            Debug.Log("useRootMotion");
            Rigidbody rb = GetComponent<Rigidbody>();
            var agent = GetComponent<NavMeshAgent>();
            agent.enabled = false;
            rb.drag = 0;
            Vector3 deltaPosition = Animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = Time.deltaTime > 0? deltaPosition / Time.deltaTime: Vector3.zero;
            rb.velocity = velocity;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics.Internal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class EnemyAnimatorManager : CharacterAnimator
{
    public bool IsComboing { get { return Animator.GetBool("IsComboing"); } set { Animator.SetBool("IsComboing",value); } }
    public bool CanDoCombo { get { return Animator.GetBool("CanDoCombo"); } set { Animator.SetBool("CanDoCombo", value); } }
    public void EnableCombo() => Animator.SetBool("CanDoCombo",true);
    public void DisableCombo() { Animator.SetBool("IsComboing", false); }
    public bool IsInteracting => Animator.GetBool("IsInteracting");
    [SerializeField] private GameObject particlesAfterDeath;
    public GameObject ParticlesAfterDeath => particlesAfterDeath;
    public Animator Animator;
    private EnemyLocomotion enemyLocomotion;
    private EnemyStats enemyStats;
    private NavMeshAgent agent;
    private CharacterController character;
    void Start()
    {
        character = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        enemyStats = GetComponent<EnemyStats>();
        enemyLocomotion = GetComponent<EnemyLocomotion>();
        Animator = GetComponent<Animator>();
    }

    public void AddAndSpawnSouls()
    {
        GlobalEventManager.OnBossFightEventEnd.Invoke();
        Destroy(Instantiate(particlesAfterDeath, transform.position, Quaternion.identity),10f);
        FindObjectOfType<PlayerStats>().AddSouls(enemyStats);
        Destroy(this.gameObject);
    }

    private void LateUpdate()
    {
        
    }

    public override void PlayTargetAnimation(string targetAnimation, bool useRootMotion = false, bool isInteracting = false, float time = 0)
    {
        Animator.SetBool("IsInteracting", useRootMotion);
        float animationTransition = time < 0.00001f ? 0.2f : time;
        Animator.CrossFade(targetAnimation, animationTransition);
    }

    public void PlayerTargetAnimationWithRootRotation(string targetAnimation, bool useRootMotion = false, bool isInteracting = false, float time = 0)
    {
        PlayTargetAnimation(targetAnimation, useRootMotion, isInteracting, time);
        Animator.SetBool("IsUsingRotationRootMotion", true);
    }

    public void ResetMovementValues()
    {
        Animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
        Animator.SetFloat("Horizontal", 0, 0.1f, Time.deltaTime);
    }

    private void OnAnimatorMove()
    {
        agent.isStopped = true;
        agent.updateRotation = false;
        Vector3 deltaPosition = Animator.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = Time.deltaTime > 0 ? deltaPosition / Time.deltaTime : Vector3.zero;
        character.SimpleMove(velocity);
        if (Animator.GetBool("IsUsingRotationRootMotion"))
        {
            agent.transform.rotation *= Animator.deltaRotation;
        }
    }
}

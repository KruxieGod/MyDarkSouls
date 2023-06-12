using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimatorManager : CharacterAnimator
{
    public bool IsComboing { get { return Animator.GetBool("IsComboing"); } set { Animator.SetBool("IsComboing",value); } }
    public bool CanDoCombo { get { return Animator.GetBool("CanDoCombo"); } set { Animator.SetBool("CanDoCombo", value); } }
    public void EnableCombo() => Animator.SetBool("CanDoCombo",true);
    public void DisableCombo() { Animator.SetBool("IsComboing", false); }
    public bool IsUsingRootMotion => Animator.GetBool("IsUsingRootMotion");
    [SerializeField] private GameObject particlesAfterDeath;
    public GameObject ParticlesAfterDeath => particlesAfterDeath;
    public Animator Animator;
    private EnemyLocomotion enemyLocomotion;
    private EnemyStats enemyStats;
    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        enemyLocomotion = GetComponent<EnemyLocomotion>();
        Animator = GetComponent<Animator>();
    }

    public void AddAndSpawnSouls()
    {
        Destroy(Instantiate(particlesAfterDeath, transform.position, Quaternion.identity),10f);
        FindObjectOfType<PlayerStats>().AddSouls(enemyStats);
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

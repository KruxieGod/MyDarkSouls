using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.Services.Analytics.Internal;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.HID;

public class EnemyLocomotion : MonoBehaviour
{
    private EnemyAnimatorManager enemyAnimator;
    public LayerMask LayerDetections;
    private EnemyManager enemyManager;
    public NavMeshAgent Agent;
    public CharacterManager CharacterManager;
    private Vector3 wanderPos = Vector3.zero;
    public float DistanceDetection;
    private bool isPursuit;
    private Coroutine coroutine;
    private bool waitForIdle;
    public float Angle = 60f;
    public bool CanRotate = true;
    [SerializeField] private float slerpRotation;
    void Awake()
    {
        enemyAnimator = GetComponent<EnemyAnimatorManager>();
        enemyManager = GetComponent<EnemyManager>();
        Agent = GetComponent<NavMeshAgent>();
        //Agent.updatePosition = false;
        Agent.updateRotation = false;
    }

    public void HandleAllMovement()
    {

    }

    public bool IsClosedWithTarget()
    {
        return Vector3.Distance(CharacterManager.transform.position, transform.position) <= Agent.stoppingDistance;
    }

    public void CombatStanceUpdateRotation()
    {
        Vector3 targetDirection = CharacterManager.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
        transform.rotation = Quaternion.RotateTowards(transform.rotation,targetRotation,slerpRotation*Time.deltaTime);
    }

    public void Seek(Vector3 targetPosition)
    {
        Agent.SetDestination(targetPosition);
    }

    private void PursueUpdateRotation()
    {
        Agent.SetDestination(this.CharacterManager.transform.position);
        var v = Agent.path.corners[Mathf.Clamp(1, 0, Agent.path.corners.Length-1)] - transform.position;
        v.y = 0;
        Quaternion targetRotation = Quaternion.LookRotation(v);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, slerpRotation * Time.deltaTime);
    }

    public void PursuitPlayer()
    {
        if (enemyAnimator.Animator.GetBool("IsUsingRootMotion"))
            return;
        PursueUpdateRotation();
        Agent.speed = 3.5f;
        Vector3 targetPosition = this.CharacterManager.transform.position;
        Vector3 direction = targetPosition - Agent.transform.position;
        direction.y = 0;
        float distance = direction.magnitude;
        if (distance <=Agent.stoppingDistance)
        {
            enemyAnimator.Animator.SetFloat("Vertical", 0f);
            enemyAnimator.Animator.SetFloat("Horizontal", 0f);
            return;
        }
        if (distance < DistanceDetection)
        {
            float angle = Vector3.Angle(direction, transform.forward);
            float angleBetweenForwards = Vector3.Angle(transform.forward, this.CharacterManager.transform.forward);
            if (angle <= Angle)
                isPursuit = true;
            if (isPursuit)
            {
                var characterControllerPlayer = CharacterManager.GetComponent<CharacterController>();
                if ((angleBetweenForwards < 30 && angle > 90) || characterControllerPlayer.velocity.magnitude < 0.01f)
                {
                    Seek(targetPosition);
                }
                else
                {
                    float offset = direction.magnitude / (Agent.speed + characterControllerPlayer.velocity.magnitude); // Sum of speeds = time to target
                    Seek(targetPosition + CharacterManager.transform.forward * offset * 1.5f);
                }
                enemyAnimator.Animator.SetFloat("Vertical", 1f, 0.1f, Time.deltaTime);
                enemyAnimator.Animator.SetFloat("Horizontal", 0f, 0.1f, Time.deltaTime);
            }
        }
        else
        {
            enemyAnimator.ResetMovementValues();
            isPursuit = false;
            CharacterManager = null;
        }
    }

    private IEnumerator TimeToBack()
    {
        yield return new WaitForSeconds(1f);
        coroutine= null;
        waitForIdle = true;
        yield return new WaitForSeconds(1f);
        waitForIdle= false;
    }

    public void Wandering()
    {
        Agent.speed = 3f;
        wanderPos += new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        wanderPos.Normalize();
        wanderPos *= 2f;
        if (!NavMesh.SamplePosition(wanderPos, out var hit, 0.1f, NavMesh.AllAreas))
        {
            Debug.Log("Obstacle");
            wanderPos = -transform.forward;
        }
        Seek(wanderPos + transform.position);
    }
}

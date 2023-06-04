using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
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
    void Awake()
    {
        enemyAnimator = GetComponent<EnemyAnimatorManager>();
        enemyManager = GetComponent<EnemyManager>();
        Agent = GetComponent<NavMeshAgent>();
    }

    public void HandleAllMovement()
    {

    }

    private void Seek(Vector3 targetPosition)
    {
        //if (Vector3.Angle(targetPosition, transform.forward) >= 90 &&
        //    !enemyAnimator.Animator.GetCurrentAnimatorStateInfo(0).IsName("RunToStop") &&
        //    Agent.speed > 2f)
        //    enemyAnimator.PlayTargetAnimation("RunToStop"); // Нужно сделать , чтобы объект при повороте тормозил , но адекватно
        Agent.SetDestination(targetPosition);
    }

    public void PursuitPlayer()
    {
        if (enemyAnimator.Animator.GetBool("IsUsingRootMotion"))
            return;
        Agent.speed = 3.5f;
        Vector3 targetPosition = this.CharacterManager.transform.position;
        Vector3 direction = targetPosition - Agent.transform.position;
        float distance = direction.magnitude;
        
        if (distance < 1.5 || coroutine != null)
        {
            Debug.Log("A");
            Agent.updateRotation = false;
            Agent.speed = 2f;
            transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0,direction.z));
            Seek(transform.position - direction.normalized*Mathf.Clamp(direction.magnitude,2f, direction.magnitude));
            if (coroutine != null && distance < 1)
            {
                StopCoroutine(coroutine);
                coroutine = StartCoroutine(TimeToBack());
            }
            else if (coroutine == null)
                coroutine = StartCoroutine(TimeToBack());
        }
        else if(distance < DistanceDetection)
        {
            float angle = Vector3.Angle(direction, transform.forward);
            float angleBetweenForwards = Vector3.Angle(transform.forward, this.CharacterManager.transform.forward);
            Agent.updateRotation = true;
            if (angle <= Angle)
                isPursuit = true;
            if (isPursuit)
            {
                if ((angleBetweenForwards < 30 && angle > 90) || CharacterManager.GetComponent<Rigidbody>().velocity.magnitude < 0.01f)
                {
                    Seek(targetPosition);
                    Debug.Log("2");
                }
                else
                {
                    float offset = direction.magnitude / (Agent.speed + this.CharacterManager.GetComponent<Rigidbody>().velocity.magnitude); // Sum of speeds = time to target
                    Seek(targetPosition + CharacterManager.transform.forward * offset * 1.5f);
                    Debug.Log("1");
                }
            }
        }
        else
        {
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

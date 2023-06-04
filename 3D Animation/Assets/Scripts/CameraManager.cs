using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Unity.PlasticSCM.Editor.UI;
using Unity.VisualScripting;
using UnityEngine;
using static Models;
using static System.Collections.Specialized.BitVector32;

public class CameraManager : MonoBehaviour
{
    private InputManager inputManager;
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Transform cameraPivot;
    private Transform cameraTransform;
    [SerializeField] private LayerMask collisionLayers;
    public LayerMask IgnoreLayers;
    public LayerMask Enemy;
    private float defaultPos;
    private Vector3 cameraFollowVelocity = Vector3.zero;
    private Vector3 cameraPosition;
    private PlayerManager playerManager;

    private float lookAngle; //Camera looking Up/Down
    private float pivotAngle;//Camera looking Left/Right

    public bool IsEmptyCollection { get { return availableTargets.Count == 0; } }
    public CameraSettings Settings;
    private HashSet<CharacterManager> availableTargets =  new HashSet<CharacterManager>();
    public Transform NearestLockOnTarget;
    public Transform CurrentLockOnTarget;

    private void Awake()
    {
        inputManager = FindObjectOfType<InputManager>();
        targetTransform = FindObjectOfType<PlayerManager>().transform;
        playerManager = targetTransform.GetComponent<PlayerManager>();
        cameraTransform = Camera.main.transform;
        defaultPos = cameraTransform.localPosition.z;
    }

    public void HandleAllCameraMovement()
    {
        FollowTarget();
        RotateCamera();
        HandleCameraCollisions();
    }

    private void FollowTarget()
    {
        Vector3 targetPos =
            Vector3.SmoothDamp(transform.position,targetTransform.position, ref cameraFollowVelocity, Settings.cameraFollowSpeed);
        transform.position = targetPos;
    }

    private void RotateCamera()
    {
        if (CurrentLockOnTarget != null && inputManager.LockOnInput)
        {
            Vector3 direction = CurrentLockOnTarget.position - transform.position;
            direction.y = 0;
            direction.Normalize();
            Quaternion targetRotat = Quaternion.LookRotation(direction);
            transform.rotation = targetRotat;

            direction = CurrentLockOnTarget.position - cameraPivot.position;
            direction.Normalize();

            targetRotat = Quaternion.LookRotation(direction);
            float eulerAngle = targetRotat.eulerAngles.x;
            cameraPivot.localEulerAngles = new Vector3(eulerAngle,0,0);

            return;
        }
        Vector3 rotation;
        Quaternion targetRotation;
        lookAngle = Mathf.Lerp(lookAngle, lookAngle + (inputManager.cameraInputX * Settings.cameraLookSpeed), 1f* Time.deltaTime);
        pivotAngle = Mathf.Lerp(pivotAngle, pivotAngle - (inputManager.cameraInputY * Settings.cameraPivotSpeed), 1f * Time.deltaTime);
        pivotAngle = Mathf.Clamp(pivotAngle, Settings.minPivotAngle, Settings.maxPivotAngle);

        rotation = Vector3.zero;
        rotation.y = lookAngle;
        targetRotation = Quaternion.Euler(rotation);
        transform.rotation = targetRotation;

        rotation = Vector3.zero;
        rotation.x = pivotAngle;
        targetRotation = Quaternion.Euler(rotation);
        cameraPivot.localRotation = targetRotation;
    }
    
    private void HandleCameraCollisions()
    {
        float targetPos = defaultPos;
        RaycastHit hit;
        Vector3 direction = cameraTransform.position - cameraPivot.position;
        direction.Normalize();

        if (Physics.SphereCast
            (cameraPivot.transform.position, Settings.cameraCollisionRadius, direction,out hit, Mathf.Abs(targetPos), collisionLayers))
        {
            float distance = Vector3.Distance(cameraPivot.position,hit.point);
            
            targetPos = -distance + Settings.cameraCollisionOffSet;
        }
        
        if (Mathf.Abs(targetPos) < Settings.minimumCollisionOffSet)
        {
            targetPos -= Settings.minimumCollisionOffSet;
        }

        cameraPosition.z = Mathf.Lerp(cameraTransform.localPosition.z, targetPos,0.2f);
        cameraTransform.localPosition = cameraPosition;
    }

    public void HandleOnLock()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position,Settings.maximumLockOnDistance*2f,Enemy);
        availableTargets.IntersectWith(colliders.Select(c => c.GetComponent<CharacterManager>()));
        foreach (var collider in colliders)
        {
            if (collider.TryGetComponent<CharacterManager>(out var characterManager)
                && !availableTargets.Contains(characterManager)
                && !Physics.Raycast(playerManager.LockOnTransform.position, characterManager.LockOnTransform.position - playerManager.LockOnTransform.position,
                (characterManager.LockOnTransform.position - playerManager.LockOnTransform.position).magnitude, collisionLayers))
            {
                availableTargets.Add(characterManager);
                Debug.Log(availableTargets.Count);
            }
        }
        SearchShortnest();
        if (inputManager.Arrow != 0)
            ChangeLockOnArrows();
    }

    private void SearchShortnest()
    {
        float shortestDistance = Mathf.Infinity;
        foreach (var target in availableTargets)
        {
            Vector3 velocity = target.transform.position - transform.position;
            float distance = velocity.magnitude;
            if (distance <= Settings.maximumLockOnDistance && distance < shortestDistance &&
                CurrentLockOnTarget != target.LockOnTransform)
            {
                shortestDistance = distance;
                NearestLockOnTarget = target.LockOnTransform;
            }
        }
        if (availableTargets.Count == 0)
            CurrentLockOnTarget = null;
        else if (CurrentLockOnTarget == null)
            CurrentLockOnTarget = NearestLockOnTarget;
    }

    private void ChangeLockOnArrows()
    {
        Func<float,float, bool> comparer = inputManager.Arrow > 0 ? (x, y) => x > y : (x, y) => x < y ;
        float shortest = Mathf.Infinity;
        foreach (var target in availableTargets)
        {
            Vector3 enemyDirectionLocal = transform.InverseTransformPoint(target.transform.position);
            float x = enemyDirectionLocal.x;
            float angle = Vector3.Angle(transform.forward,target.transform.position - transform.position);
            if (comparer(x, 0) && angle <shortest && angle !=0 &&
                CurrentLockOnTarget!= target.LockOnTransform)
            {
                shortest = angle;
                NearestLockOnTarget = target.LockOnTransform;
            }
        }
        if (shortest != Mathf.Infinity)
        {
            Debug.Log(availableTargets.Count) ;
            CurrentLockOnTarget = NearestLockOnTarget;
        }
    }

    public void ClearLockOnTarget()
    {
        Debug.Log("кеее");
        CurrentLockOnTarget= null;
        NearestLockOnTarget= null;
    }
}

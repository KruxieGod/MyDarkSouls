using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.TextCore.Text;
using static Models;

public class PlayerLocomotion : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve;
    private float targetRotation;
    private PlayerStats playerStats;
    private CameraManager cameraManager;
    private AnimatorManager animatorManager;
    private PlayerManager playerManager;
    public PlayerSettings Settings;
    InputManager inputManager;
    private WeaponSlotManager weaponSlotManager;
    private Coroutine jumpCoroutine;
    private Action updateRotation = null;
    private Vector3 moveDirection;
    Transform cameraObject;
    private CharacterController characterController;
    private Vector3 diretionRotation;
    

    [Header("Falling")]
    public float inAirTimer;

    [Header("Movement")]
    public bool IsSprinting;
    public bool IsGrounded;
    public bool IsJumping;

    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        cameraManager = FindObjectOfType<CameraManager>();
        weaponSlotManager = GetComponent<WeaponSlotManager>();
        diretionRotation = transform.forward;
        animatorManager = GetComponent<AnimatorManager>();  
        inputManager = GetComponent<InputManager>();
        characterController = GetComponent<CharacterController>();
        cameraObject = Camera.main.transform;
        playerManager = GetComponent<PlayerManager>();
    }

    public void HandleAllMovement()
    {

    }

    public void HandleRotationUpdate()
    {

        HandleFallingAndLanding();
        if (updateRotation != null)
            updateRotation();
        Debug.Log(playerManager.isInteracting);
        if (playerManager.isInteracting)
        {
            if (jumpCoroutine == null)
                characterController.Move(moveDirection);
            return;
        }
        HandleRotation();
        HandleMovement();
        characterController.Move(moveDirection);
    }

    public void DisableVelocity()
    {
        moveDirection= Vector3.zero;
    }

    public void EnableRotation()
    {
        updateRotation = () => { UpdateRotation();transform.rotation = Quaternion.LookRotation(diretionRotation); };
    }

    public void DisableRotation()
    {
        updateRotation = null;
    }

    private void HandleMovement()
    {
        if (IsJumping)
            return;
        var gravity = moveDirection.y;
        moveDirection = cameraObject.forward * inputManager.Vertical;
        moveDirection += cameraObject.right * inputManager.Horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;
        if (IsSprinting && playerStats.CurrentStamina > 0)
        {
            moveDirection *= Settings.sprintingSpeed*Time.deltaTime;
        }
        else if (inputManager.moveAmount > 0.5f)
        {
            moveDirection *= Settings.runningSpeed * Time.deltaTime;
        }
        else
        {
            moveDirection *= Settings.walkingSpeed * Time.deltaTime;
        }
        moveDirection.y = gravity;
    }

    private void HandleRotation()
    {
        if (IsJumping)
            return;
        if (inputManager.LockOnInput)
        {
            Vector3 rotation = cameraObject.rotation.eulerAngles;
            rotation.x = 0;
            rotation.z = 0;
            transform.eulerAngles = rotation;
            return;
        }
        UpdateRotation();
        targetRotation = Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.y, Quaternion.LookRotation(diretionRotation).eulerAngles.y, Settings.rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(new Vector3(0, targetRotation, 0));
    }

    public void UpdateRotation()
    {
        Vector3 targetDir = Vector3.zero;
        targetDir = cameraObject.forward * inputManager.Vertical;
        targetDir += cameraObject.right * inputManager.Horizontal; 
        targetDir.Normalize();
        targetDir.y = 0;
        if (targetDir == Vector3.zero)
            targetDir = diretionRotation;
        else
            diretionRotation = targetDir;
        Debug.Log("updating");
    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y += Settings.RayCastHeightOffSet;
        if (!IsGrounded && !IsJumping)
        {
            if (!playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Falling", false,true);
            }
            Debug.Log("Falling");
            moveDirection /= 2f;
            animatorManager.animator.SetBool("IsUsingRootMotion", false);
            inAirTimer += Time.deltaTime;
            moveDirection.y += Physics.gravity.y * Time.deltaTime * Settings.FallingVelocity;
        }

        if (Physics.SphereCast(rayCastOrigin,0.3f,-Vector3.up,out hit,1f, Settings.GroundLayer))
        {
            if (!IsGrounded)
            {
                animatorManager.PlayTargetAnimation("Land", false,true);
                moveDirection = Vector3.zero;
            }
            if (hit.point.y + 0.01f >= transform.position.y)
                moveDirection.y = 0;
            else
                moveDirection.y += Physics.gravity.y * Time.deltaTime * Settings.GravityModifier;
            inAirTimer = 0;
            IsGrounded = true;
        }
        else
            IsGrounded = false;
        if (animatorManager.animator.GetBool("isJumping") && !IsGrounded)
            moveDirection.y += Physics.gravity.y * Time.deltaTime * Settings.GravityModifier * Settings.GravityModifier;
    }

    public void HandleJump() 
    { 
        if (IsGrounded && !playerManager.isInteracting && playerStats.CurrentStamina > 0)
        {
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jump", false,true);
            jumpCoroutine = StartCoroutine(JumpCoroutine());
        }
    }

    public IEnumerator JumpCoroutine()
    {
        yield return new WaitForEndOfFrame();

        float jumpingVelocity = Mathf.Sqrt(-2 * Settings.gravityIntensity * Settings.jumpHeight);
        moveDirection = Vector3.ClampMagnitude(moveDirection, (new Vector3(0.01f,0,-0.02f)).magnitude);
        moveDirection.y = jumpingVelocity;
        characterController.Move(moveDirection);
        jumpCoroutine = null;
    }

    public void HandleDodge(string dodge)
    {
        if (playerStats.CurrentStamina <= 0 || playerManager.isInteracting && !animatorManager.animator.GetBool("IsAttacking") ||
            animatorManager.animator.GetBool("IsDodging"))
            return;
        Debug.Log("Dodge");
        weaponSlotManager.CloseRightHandDamageCollider();
        animatorManager.animator.SetBool("IsDodging", true);
        //Invincibility
        animatorManager.PlayTargetAnimation(dodge, true,true);
    }
}

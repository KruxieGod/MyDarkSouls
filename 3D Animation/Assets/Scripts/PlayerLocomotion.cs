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
    int i = 0;
    private CameraManager cameraManager;
    private AnimatorManager animatorManager;
    private PlayerManager playerManager;
    public PlayerSettings Settings;
    InputManager inputManager;
    private WeaponSlotManager weaponSlotManager;

    private Vector3 moveDirection;
    Transform cameraObject;
    [HideInInspector]public Rigidbody Rb;
    private Vector3 diretionRotation;
    public Quaternion TargetRotation;
    

    [Header("Falling")]
    public float inAirTimer;

    [Header("Movement")]
    public bool IsSprinting;
    public bool IsGrounded;
    public bool IsJumping;

    private void Awake()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        weaponSlotManager = GetComponent<WeaponSlotManager>();
        diretionRotation = transform.forward;
        animatorManager = GetComponent<AnimatorManager>();  
        inputManager = GetComponent<InputManager>();
        Rb = GetComponent<Rigidbody>();
        cameraObject = Camera.main.transform;
        playerManager = GetComponent<PlayerManager>();
    }

    public void HandleAllMovement()
    {
        HandleFallingAndLanding();
        if (playerManager.isInteracting)
            return;
        HandleRotation();
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (IsJumping)
            return;
        moveDirection = cameraObject.forward * inputManager.Vertical;
        moveDirection += cameraObject.right * inputManager.Horizontal;
        moveDirection.Normalize();
        moveDirection.y = 0;
        if (IsSprinting)
        {
            moveDirection *= Settings.sprintingSpeed;
        }
        else if (inputManager.moveAmount > 0.5f)
        {
            moveDirection *= Settings.runningSpeed;
        }
        else
        {
            moveDirection *= Settings.walkingSpeed;
        }
        Rb.velocity = moveDirection;
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
            TargetRotation = transform.rotation;
            return;
        }
        UpdateRotation();
        Quaternion playerRotation = Quaternion.Lerp(transform.rotation,TargetRotation, Time.deltaTime*Settings.rotationSpeed);
        transform.rotation = playerRotation;
    }

    public void UpdateRotation()
    {
        Vector3 targetDir = Vector3.zero;
        targetDir = cameraObject.forward * inputManager.Vertical;
        targetDir += cameraObject.right * inputManager.Horizontal; 
        targetDir.Normalize();
        targetDir.y = 0;
        if (targetDir == Vector3.zero)
        {
            targetDir = diretionRotation;
            if (inputManager.LockOnInput)
            {
                TargetRotation = transform.rotation;
                return;
            }
        }
        else
            diretionRotation = targetDir;
        TargetRotation = Quaternion.LookRotation(targetDir);
    }

    private void HandleFallingAndLanding()
    {
        RaycastHit hit;
        Vector3 rayCastOrigin = transform.position;
        rayCastOrigin.y += Settings.RayCastHeightOffSet;
        Vector3 targetPosition = transform.position;

        if(playerManager.isInteracting  
            && !animatorManager.animator.GetBool("IsLanding") 
            && !IsGrounded)
        {
            Vector3 gravity = new Vector3(0,Physics.gravity.y*Time.deltaTime,0);
            Rb.velocity += gravity;
        }
        if (animatorManager.animator.GetBool("IsDeath"))
        {
            Rb.velocity += new Vector3(0, Physics.gravity.y * Time.deltaTime*6f, 0);
            return;
        }
        if (!IsGrounded && !IsJumping)
        {
            if (!playerManager.isInteracting)
            {
                animatorManager.PlayTargetAnimation("Falling", true);
            }
            animatorManager.animator.SetBool("IsUsingRootMotion", false);
            inAirTimer += Time.deltaTime;
            Rb.AddForce(transform.forward * Settings.LeapingVelocity * 0);
            Rb.AddForce(-Vector3.up * inAirTimer * Settings.FallingVelocity);
        }

        if (Physics.SphereCast(rayCastOrigin,0.25f,-Vector3.up,out hit,1f, Settings.GroundLayer))
        {
            if (!IsGrounded)
            {
                animatorManager.PlayTargetAnimation("Land", true);
                Rb.velocity = Vector3.zero;
            }
            targetPosition = hit.point;
            inAirTimer = 0;
            IsGrounded = true;
        }
        else
        {
            IsGrounded = false;
        }

        if (!IsJumping && IsGrounded)
        {
            if (inputManager.moveAmount > 0 || playerManager.isInteracting)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / 0.1f);
            }
            else 
                transform.position = targetPosition;
        }
    }

    public void HandleJump() 
    { 
        if (IsGrounded && !playerManager.isInteracting)
        {
            animatorManager.animator.SetBool("isJumping", true);
            animatorManager.PlayTargetAnimation("Jump", false);
            float jumpingVelocity = Mathf.Sqrt(-2 * Settings.gravityIntensity * Settings.jumpHeight);
            moveDirection.y = jumpingVelocity;
            Rb.velocity = moveDirection;
        }
    }

    public void HandleDodge(string dodge)
    {
        if (playerManager.isInteracting && !animatorManager.animator.GetBool("IsAttacking") ||
            animatorManager.animator.GetBool("IsDodging"))
            return;
        Debug.Log("Dodge");
        weaponSlotManager.CloseRightHandDamageCollider();
        UpdateRotation();
        animatorManager.animator.SetBool("IsDodging", true);
        //Invincibility
        animatorManager.PlayTargetAnimation(dodge, true,true);
    }
}

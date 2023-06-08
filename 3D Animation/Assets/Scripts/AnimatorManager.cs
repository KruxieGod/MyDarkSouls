using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : CharacterAnimator
{
    private PlayerStats playerStats;
    private InputManager inputManager;
    public Animator animator;
    private PlayerManager playerManager;
    private PlayerLocomotion playerLocomotion;
    private PlayerInventory playerInventory;
    private int horizontal;
    private int vertical;
    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
       playerInventory = GetComponent<PlayerInventory>();
        inputManager = GetComponent<InputManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerManager = GetComponent<PlayerManager>();
        animator = GetComponent<Animator>();
        horizontal = Animator.StringToHash("Horizontal");
        vertical = Animator.StringToHash("Vertical");
    }

    public override void PlayTargetAnimation(string targetAnimation, bool useRootMotion = false, bool isInteracting = false, float time = 0)
    {
        animator.SetBool("isInteracting", isInteracting);
        animator.SetBool("IsUsingRootMotion", useRootMotion);
        float animationTransition = 0.2f;
        if (animator.GetBool("IsDodging"))
            animationTransition = 0.05f;
        animator.CrossFade(targetAnimation, animationTransition);
    }

    public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement,bool isSprinting)
    {
        // Animation Snapping
        float snappedHorizontal = HelpedFunc(horizontalMovement);
        float snappedVertical = HelpedFunc(verticalMovement);
        if (isSprinting && !inputManager.LockOnInput && playerStats.CurrentStamina > 0)
            snappedVertical = 2f;
        float dampTime = animator.GetFloat(vertical) < 0.1f && snappedVertical == 1 ? 0.1f : 0.2f;
        //if ((snappedHorizontal > 0.5 || snappedHorizontal < -0.5 || snappedVertical > 0.5 || snappedVertical < -0.5)
        //    && animator.GetCurrentAnimatorStateInfo(5).IsName("Two Handed Idle"))
        //{
        //    animator.CrossFade("Two Handed Run", dampTime);
        //}
        animator.SetFloat(horizontal, snappedHorizontal, dampTime, Time.deltaTime);
        animator.SetFloat(vertical, snappedVertical, dampTime, Time.deltaTime);
        if (snappedVertical == 0 && animator.GetFloat(vertical) > 0.5f && !animator.GetCurrentAnimatorStateInfo(0).IsName("Run To Stop"))
        {
            animator.CrossFade("Run To Stop", 0.05f, 0);
        }
    }

    private float HelpedFunc(float directionMovement)
    {
        float snappedValue;
        if (directionMovement > 0 && directionMovement < 0.55f)
            snappedValue = 0.5f;
        else if (directionMovement > 0.55f)
            snappedValue = 1f;
        else if (directionMovement < 0 && directionMovement > -0.55f)
            snappedValue = -0.5f;
        else if (directionMovement < -0.55f)
            snappedValue = -1f;
        else
            snappedValue = 0f;
        return snappedValue;
    }

    public void EnableCombo()
    {
        animator.SetBool("CanDoCombo", true);
    }

    public void DisableCombo() 
    {
        animator.SetBool("CanDoCombo", false);
    }

    private void OnAnimatorMove()
    {
        if (playerManager.IsUsingRootMotion && Time.timeScale > 0.1f)
        {
            playerLocomotion.DisableVelocity();
            CharacterController rb = GetComponent<CharacterController>();
            Vector3 deltaPosition = animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = Time.deltaTime> 0? deltaPosition: Vector3.zero;
            rb.Move(velocity);
        }    
    }

    public void SuccessfullyCast()
    {
        playerInventory.LeftWeapon.Interact(true); // true - значит завершаем каст 
    }
}

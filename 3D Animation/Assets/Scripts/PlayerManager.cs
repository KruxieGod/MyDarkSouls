using SG;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class PlayerManager : CharacterManager
{
    private Animator animator;
    private InputManager inputManager;
    private CameraManager cameraManager;
    private PlayerLocomotion playerLocomotion;
    private ScriptScroll scroll;

    [HideInInspector]public bool CanDoCombo;
    [HideInInspector]public bool isInteracting;
    [HideInInspector]public bool IsUsingRootMotion;
    private void Awake()
    {
        scroll = FindObjectOfType<ScriptScroll>();
        animator = GetComponent<Animator>();
        inputManager = GetComponent<InputManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
    }

    private void Update()
    {
        inputManager.HandleAllInputs();
        TriggerIsHere();
        //CheckOnCollider();
    }

    private void FixedUpdate()
    {
        playerLocomotion.HandleAllMovement();
    }

    private void LateUpdate()
    {
        cameraManager.HandleAllCameraMovement();

        CanDoCombo = animator.GetBool("CanDoCombo");
        isInteracting = animator.GetBool("isInteracting");
        playerLocomotion.IsJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded",playerLocomotion.IsGrounded);
        IsUsingRootMotion = animator.GetBool("IsUsingRootMotion");
    }

    private void TriggerIsHere()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position + new Vector3(0, 1 / 5f, 0), Vector3.one / 3f, transform.rotation, cameraManager.IgnoreLayers);
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                scroll.Add(colliders[i].GetComponent<Interactable>());
            }
        }
        scroll.Remove(colliders.Select(x => x.GetComponent<Interactable>()), colliders.Length);
    }
}

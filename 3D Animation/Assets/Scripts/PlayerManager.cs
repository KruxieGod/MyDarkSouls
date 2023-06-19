using SG;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class PlayerManager : CharacterManager
{
    [SerializeField] private Vector3 sizeSearchingItems = new Vector3(1,0.5f,1);
    [SerializeField]private LayerMask itemsMask;
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
        playerLocomotion.HandleRotationUpdate();
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
        Collider[] colliders = Physics.OverlapBox(transform.position + new Vector3(0, sizeSearchingItems.y / 2f, 0), sizeSearchingItems, transform.rotation, itemsMask);
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                scroll.Add(colliders[i].GetComponent<Interactable>());
            }
        }
        scroll.Remove(colliders.Select(x => x.GetComponent<Interactable>()), colliders.Length);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position + new Vector3(0, sizeSearchingItems.y/2f, 0), sizeSearchingItems);
    }
}

using SG;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class InputManager : MonoBehaviour
{
    CharacterStats characterStats;
    private string idPlayer = Guid.NewGuid().ToString();
    public string IdPlayer => idPlayer;
    public static Dictionary<string, InputManager> Instance = new Dictionary<string, InputManager>();
    public PlayerControls playerControls;
    private AnimatorManager animatorManager;
    private PlayerLocomotion playerLocomotion;
    private PlayerAttacker playerAttacker;
    private PlayerInventory playerInventory;
    private PlayerManager playerManager;
    private UIManager uiManager;
    private CameraManager cameraManager;
    private WeaponSlotManager weaponSlotManager;

    private bool eIsPressed;
    private float timePressedE = 0f;
    private bool isRightClick { get; set; }
    private Vector2 movementInput;
    private Vector2 cameraInput;
    public bool Alt;
    private bool ctrl;
    public bool Ctrl { get 
        {
            if (playerControls.PlayerActions.CTRL.WasPressedThisFrame())
                ctrl = !ctrl;
            return ctrl;
        } }
    private bool g;
    public bool G
    {
        get
        {
            if (playerControls.PlayerActions.G.WasPressedThisFrame())
                g = !g;
            return g;
        }
        set { g = value; }
    }
    public bool sprint;
    public bool IsJump;
    public bool ComboFlag;
    public bool LockOnInput = false;
    public bool InventoryIsOpened = false;
    public bool InventoryInput { get { return playerControls.PlayerActions.Inventory.WasPressedThisFrame(); } }
    public bool FIsPressed { get { return playerControls.PlayerActions.F.WasPerformedThisFrame(); } }
    public bool ScrollUp { get { return axisScroll > 0; } }
    public bool ScrollDown { get { return axisScroll < 0; } }

    private float axisScroll;
    public float Arrow;
    public float ArrowDownUp;
    public float cameraInputX { get { return cameraInput.x; } }
    public float cameraInputY { get { return cameraInput.y; } }

    public float moveAmount;

    public float Vertical { get { return movementInput.y; } }
    public float Horizontal { get { return movementInput.x; } }
    private Action action;

    private void Awake()
    {
        characterStats = GetComponent<CharacterStats>();
        Instance.Add(IdPlayer,this);
        cameraManager = FindObjectOfType<CameraManager>();
        uiManager = FindObjectOfType<UIManager>();
        playerManager = GetComponent<PlayerManager>();
        playerInventory = GetComponent<PlayerInventory>();
        playerAttacker = GetComponent<PlayerAttacker>();
        animatorManager = GetComponent<AnimatorManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        weaponSlotManager = GetComponent<WeaponSlotManager>();
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            playerControls.PlayerActions.G.started += i => playerInventory.TwoHandedChange();
            playerControls.PlayerActions.B.performed += i => sprint = true;
            playerControls.PlayerActions.B.canceled += i => sprint = false;
            playerControls.PlayerActions.Arrows.started+= i => StartCoroutine(WaitEndOfFrame(i.ReadValue<float>()));
            playerControls.PlayerActions.ArrowsDownUp.started += i => StartCoroutine(WaitEndOfFrameDown(i.ReadValue<float>()));
            playerControls.PlayerActions.Alt.performed += i => Alt = true;
            playerControls.PlayerActions.RightClick.started += i => { if (!animatorManager.animator.GetBool("IsAttacking"))
                {
                    isRightClick = true; 
                    HandleRightClick();
                }
            };
            playerControls.PlayerActions.RightClick.canceled += i => { if (!animatorManager.animator.GetBool("IsAttacking") && isRightClick)
                {
                    HandleRightClick();
                    isRightClick = false;
                }
            }; 
            playerControls.PlayerActions.Jump.performed += i => IsJump = true;
            playerControls.PlayerActions.BackStabInput.performed += i => BackStabInput();
            playerControls.PlayerActions.LightAttack.performed += i => AttackInput(true);
            playerControls.PlayerActions.HeavyAttack.performed += i => AttackInput(false);
            playerControls.PlayerActions.E.performed += i => timePressedE+= 0.1f;
            playerControls.PlayerActions.E.canceled += i => timePressedE = 0f;
            playerControls.PlayerActions.Scroll.performed += i => axisScroll = i.ReadValue<float>();
        }
        playerControls.Enable();
    }

    private void BackStabInput()
    {
        playerControls.PlayerActions.BackStabInput.Reset();
        playerAttacker.AttemptToBackStabAttack();
    }

    private void ParryInput()
    {
        if (playerControls.PlayerActions.E.WasPerformedThisFrame() || eIsPressed)
        {
            eIsPressed = true;
            if (timePressedE == 0f)
            {
                playerAttacker.AttemptToParryAttack();
                eIsPressed = false;
                playerControls.PlayerActions.E.Reset();
            }
        }
    }

    private IEnumerator WaitEndOfFrame(float i)
    {
        Arrow = i;
        yield return new WaitForEndOfFrame();
        Arrow = 0;
    }

    private IEnumerator WaitEndOfFrameDown(float i)
    {
        ArrowDownUp = i;
        yield return new WaitForEndOfFrame();
        ArrowDownUp = 0;
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void HandleJumpingInput()
    {
        if (IsJump)
        {
            IsJump = false;
            playerLocomotion.HandleJump();
        }
    }

    public void HandleAllInputs()
    {
        ParryInput();
        HandleQuickSlotsInput();
        HandleJumpingInput();
        HandleMovementInput();
        HandleSprintingInput();
        HandleDodgeInput();
        HandleInventoryInput();
        HandleLockOn();
        HandleLockOnInput();
    }

    private void HandleRightClick()
    {
        if (playerManager.isInteracting)
        {
            UnBlock();
            ResetInputRightClick();
            return;
        }
        if (isRightClick && !animatorManager.animator.GetBool("IsAttacking") && playerInventory.LeftWeapon.GetType() != typeof(Weapon) )
        {
            playerInventory.LeftWeapon.Interact();
            if (playerInventory.LeftWeapon.IsSpell())
                isRightClick= false;
        }
    }

    private void HandleMovementInput()
    {
        if(LockOnInput && !Ctrl)
        {
            moveAmount = Mathf.Clamp01(Mathf.Abs(Vertical) + Mathf.Abs(Horizontal));
            animatorManager.UpdateAnimatorValues(Horizontal, Vertical, playerLocomotion.IsSprinting);
        }
        else
        {
            moveAmount = !Ctrl ? Mathf.Clamp01(Mathf.Abs(Vertical) + Mathf.Abs(Horizontal)) :
            Mathf.Clamp01(Mathf.Abs(Vertical) + Mathf.Abs(Horizontal)) / 2f;
            animatorManager.UpdateAnimatorValues(0, moveAmount, playerLocomotion.IsSprinting);
        }
    }

    private void HandleSprintingInput()
    {
        if (sprint && moveAmount > 0.5f)
            playerLocomotion.IsSprinting = true;
        else
            playerLocomotion.IsSprinting = false;
    }    

    private void HandleDodgeInput()
    {
        if ( Alt && !animatorManager.animator.GetCurrentAnimatorStateInfo(1).IsName("BackStabAttack"))
        {
            UnBlock();
            ResetInputRightClick();
            Alt = false;
            playerLocomotion.HandleDodge("Rolling");
        }
    }

    private void AttackInput(bool isLight)
    {
        if (playerManager.isInteracting && !playerManager.CanDoCombo)
            return;
        Debug.Log("ATTACK");
        UnBlock();
        ResetInputRightClick();
        playerInventory.RightWeapon.Interact(isLight);
    }

    private void UnBlock()
    {
        if (playerInventory.LeftWeapon.IsShield() && playerInventory.LeftWeapon.GetShield().isBlocking)
            playerInventory.LeftWeapon.Interact();
    }

    private void ResetInputRightClick()
    {
        isRightClick = false;
    }

    public void WeaponAttack(bool isLight, Weapon weapon)
    {
        if (isLight)
        {
            if (playerManager.CanDoCombo)
            {
                ComboFlag = true;
                playerAttacker.HandleWeaponCombo(weapon);
                ComboFlag = false;
            }
            else
                playerAttacker.HandleLightAttack(weapon);
        }
        else
        {
            playerAttacker.HandleHeavyAttack(weapon);
        }
    }

    private void HandleQuickSlotsInput()
    {
        if (animatorManager.animator.GetBool("IsAttacking") || isRightClick)
            return;
        if (axisScroll != 0)
            playerInventory.ChangeRightWeapon();
        if (ArrowDownUp != 0)
            playerInventory.ChangeLeftWeapon();
    }

    private void HandleInventoryInput()
    {
        if (InventoryInput)
        {
            InventoryIsOpened = !InventoryIsOpened;
            if (InventoryIsOpened)
            {
                Time.timeScale = 0f;
                uiManager.OpenSelectWindow();
                uiManager.UpdateUi();
                uiManager.HudWindow.SetActive(false);
            }
            else
            {
                Time.timeScale = 1f;
                uiManager.CloseSelectWindow();
                uiManager.CloseAllInventoryWindows();
                uiManager.HudWindow.SetActive(true);
                uiManager.CloseEquipmentWindow();
            }
        }
    }

    public void HandleLockOnInput()
    {
        if (LockOnInput)
        {
            action = null;
            cameraManager.HandleOnLock();
            LockOnInput = !cameraManager.IsEmptyCollection;
        }
        else if (action == null)
        {
            action = () => cameraManager.ClearLockOnTarget();
            action();
        }
    }

    private void HandleLockOn()
    {
        if (playerControls.PlayerActions.LockOn.WasPressedThisFrame())
        {
            LockOnInput = !LockOnInput;
        }
    }
}

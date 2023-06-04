using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG;
public class WeaponSlotManager : MonoBehaviour
{
    public WeaponHolderSlot leftHandSlot;
    public WeaponHolderSlot rightHandSlot;
    private WeaponHolderSlot backSlot;
    private DamageCollider leftHandDamageCollider;
    private DamageCollider rightHandDamageCollider;
    private Animator animator;
    private PlayerStats playerStats;
    private InputManager inputManager;

    public Weapon AttackingWeapon;

    public QuickSlotsUI QuickSlots;

    private void Awake()
    {
        inputManager = GetComponent<InputManager>();    
        playerStats = GetComponent<PlayerStats>();
        QuickSlots = FindObjectOfType<QuickSlotsUI>();
        animator = GetComponent<Animator>();
        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
        foreach (var weaponSlot in weaponHolderSlots)
        {
            if (weaponSlot.IsLeftHandSlot)
            {
                leftHandSlot = weaponSlot;
            }
            else if (weaponSlot.IsRightHandSlot)
            {
                rightHandSlot = weaponSlot;
            }
            else if (weaponSlot.IsBackSlot)
                backSlot = weaponSlot;
        }
    }

    public void LoadWeaponOnSlot(Item weaponItem,bool isLeft)
    {
        QuickSlots.UpdateWeaponQuickSlotsUI(weaponItem);
        if (isLeft)
        {
            leftHandSlot.LoadWeaponModel(weaponItem);
            #region Handle Left Weapon Idle,Collider Animations
            if (weaponItem != null)
            {
                animator.CrossFade(weaponItem.LeftHandIdle, 0.2f);
                LoadLeftWeaponDamageCollider();
            }
            else
                animator.CrossFade("Left Arm Empty", 0.2f);
            #endregion
        }
        else
        {
            rightHandSlot.LoadWeaponModel(weaponItem);
            Debug.Log(weaponItem);
            #region Handle Right Weapon Idle,Collider Animations
            if (weaponItem != null)
            {
                animator.CrossFade(weaponItem.RightHandIdle, 0.2f);
                LoadRightWeaponDamageCollider();
            }
            else
            {
                animator.CrossFade("Right Arm Empty", 0.2f);
            }  
            #endregion
        }
    }

    public void LoadToBack(Item weapon,bool upload)
    {
        if (upload)
        {
            backSlot.LoadWeaponModel(weapon);
            leftHandSlot.UnloadWeapon();
        }
        else
        {
            backSlot.UnloadWeaponAndDestroy();
            leftHandSlot.LoadWeaponModel(weapon);
        }
    }

    #region Handle Weapon's Stamina Drainage
    public void DrainStaminaLightAttack()
    {
        playerStats.TakeStaminaDamage(Mathf.RoundToInt(AttackingWeapon.BaseStamina*AttackingWeapon.LightAttackMultiplier));
    }

    public void DrainStaminaHeavyAttack()
    {
        playerStats.TakeStaminaDamage(Mathf.RoundToInt(AttackingWeapon.BaseStamina * AttackingWeapon.HeavyAttackMultiplier));
    }
    #endregion

    #region Handle Weapon's Damage Collider

    private void LoadLeftWeaponDamageCollider()
    {
        leftHandDamageCollider = leftHandSlot.CurrentWeaponModel.GetComponentInChildren<DamageCollider>();
    }

    private void LoadRightWeaponDamageCollider()
    {
        rightHandDamageCollider = rightHandSlot.CurrentWeaponModel.GetComponentInChildren<DamageCollider>();
    }

    public void OpenRightDamageCollider()
    {
        rightHandDamageCollider.EnableDamageCollider();
    }

    public void OpenLeftDamageCollider()
    {
        leftHandDamageCollider.EnableDamageCollider();
    }

    public void CloseRightHandDamageCollider()
    {
        rightHandDamageCollider.DisableDamageCollider();
    }

    public void CloseLeftHandDamageCollider()
    {
        leftHandDamageCollider.DisableDamageCollider();
    }
    #endregion
}

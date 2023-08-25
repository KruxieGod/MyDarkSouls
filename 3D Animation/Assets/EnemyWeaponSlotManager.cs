using SG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponSlotManager : CharacterWeaponHolderSlotManager
{
    private WeaponHolderSlot leftHandSlot;
    public WeaponHolderSlot RightHandSlot { get; private set; }
    private WeaponHolderSlot backSlot;
    private DamageCollider leftHandDamageCollider;
    private DamageCollider rightHandDamageCollider;
    private Animator animator;

    public Weapon AttackingWeapon;

    private void Awake()
    {
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
                RightHandSlot = weaponSlot;
            }
            else if (weaponSlot.IsBackSlot)
                backSlot = weaponSlot;
        }
        AttackingWeapon = Instantiate(AttackingWeapon);
        LoadWeaponOnSlot(AttackingWeapon,false);
    }

    public void LoadWeaponOnSlot(Weapon weaponItem, bool isLeft)
    {
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
            RightHandSlot.LoadWeaponModel(weaponItem);
            var damageCollider = weaponItem.modelPrefab.GetComponentInChildren<DamageCollider>();
            damageCollider?.UploadWeapon(weaponItem,GetComponent<CharacterStats>());
            rightAttackingWeaponFX = weaponItem.modelPrefab.GetComponentInChildren<WeaponFX>();
            rightAttackingWeaponFX.SetActive();
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

    #region Handle Weapon's Damage Collider

    private void LoadLeftWeaponDamageCollider()
    {
        leftHandDamageCollider = leftHandSlot.CurrentWeaponModel.GetComponentInChildren<DamageCollider>();
    }

    private void LoadRightWeaponDamageCollider()
    {
        rightHandDamageCollider = RightHandSlot.CurrentWeaponModel.GetComponentInChildren<DamageCollider>();
    }

    public void OpenRightDamageCollider()
    {
        rightHandDamageCollider.EnableDamageCollider();
    }

    public void OpenLeftDamageCollider()
    {
        leftHandDamageCollider.EnableDamageCollider();
    }

    public override void CloseRightHandDamageCollider()
    {
        rightHandDamageCollider.DisableDamageCollider();
    }

    public void CloseLeftHandDamageCollider()
    {
        leftHandDamageCollider.DisableDamageCollider();
    }
    #endregion
}

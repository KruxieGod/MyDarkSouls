using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG;
using System;
using System.Linq;
using UnityEngine.Purchasing;

public class PlayerInventory : MonoBehaviour
{
    private CharacterStats characterStats;
    private InputManager inputManager;
    private Animator animator;
    WeaponSlotManager weaponSlotManager;
    public AttackingItem RightWeapon;
    public AttackingItem LeftWeapon;
    public AttackingItem UnarmedWeapon;

    public AttackingItem[] WeaponsInRightHandSlots = new AttackingItem[1];
    public AttackingItem[] WeaponsInLeftHandSlots = new AttackingItem[1];
    public AttackingItem[] DoubleWeapons = new AttackingItem[1];

    private int currentRightWeaponIndex = -1;
    private int currentLeftWeaponIndex = -1;

    public List<Item> Inventory;
    private void Awake()
    {
        characterStats = GetComponent<CharacterStats>();
        inputManager = GetComponent<InputManager>();
        animator = GetComponent<Animator>();
        weaponSlotManager = GetComponent<WeaponSlotManager>();
        foreach (var item in Inventory)
        {
            item.CharacterStats = characterStats;
        }
        UnarmedWeapon.CharacterStats = characterStats;
    }

    private void Start()
    {
        RightWeapon = UnarmedWeapon;
        LeftWeapon = UnarmedWeapon;
        weaponSlotManager.LoadWeaponOnSlot(RightWeapon,false);
        weaponSlotManager.LoadWeaponOnSlot(LeftWeapon, true);
    }

    public void ChangeRightWeapon()
    {
        Action<AttackingItem> action = x => {
            RightWeapon = x;
        weaponSlotManager.LoadWeaponOnSlot(RightWeapon, false);
        };
        ChangeWeapon(ref currentRightWeaponIndex,WeaponsInRightHandSlots, action);
    }

    public void ChangeLeftWeapon() 
    {
        if (inputManager.G)
            return;
        Action<AttackingItem> action = x => {
            LeftWeapon = x;
            weaponSlotManager.LoadWeaponOnSlot(LeftWeapon, true);
        };
        ChangeWeapon(ref currentLeftWeaponIndex,WeaponsInLeftHandSlots, action);
    }

    private void ChangeWeapon(ref int index, AttackingItem[] inventory,Action<AttackingItem> action) // передаем делегат действия , чтобы присвоить оружию нужную сторону.
    {
        index++;
        for (; index < inventory.Length; index++)
        {
            if (inventory[index] != null)
            {
                action(inventory[index]);
                break;
            }
        }
        if (index > inventory.Length - 1)
        {
            index = -1;
            action(UnarmedWeapon);
            TwoHandedChange();
        }
    }

    public void TwoHandedChange()
    {
        if (!RightWeapon.GetType().Equals(typeof(Weapon)))
            return;
        var weapon = RightWeapon as Weapon;
        if (inputManager.G && !weapon.IsUnarmed) 
        {
            animator.CrossFade("Two Handed Idle", 0.2f);
            weaponSlotManager.LoadToBack(LeftWeapon, true);
        }
        else
        {
            weaponSlotManager.LoadToBack(LeftWeapon, false);
            inputManager.G = false;
            animator.CrossFade("Two Handed Empty", 0.2f);
        }
    }

    public void RemoveItem(Item item)
    {
        Inventory.Remove(item);
        var indexRight = Array.IndexOf(WeaponsInRightHandSlots, item);
        if (indexRight != -1)
        {
            WeaponsInRightHandSlots[indexRight] = null;
            ChangeRightWeapon();
            return;
        }
        var indexLeft = Array.IndexOf(WeaponsInLeftHandSlots, item);
        if (indexLeft != -1)
        {
            WeaponsInLeftHandSlots[indexLeft] = null;
            Debug.Log(WeaponsInLeftHandSlots[indexLeft]);
            ChangeLeftWeapon();
        }
    }
}

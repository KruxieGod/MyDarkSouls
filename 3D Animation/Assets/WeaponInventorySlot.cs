using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SG;

public class WeaponInventorySlot : MonoBehaviour
{
    [SerializeField]private Image icon;
    public Item Item;
    private static PlayerInventory playerInventory;

    private void Awake()
    {
        if (playerInventory == null)
            playerInventory = FindObjectOfType<PlayerInventory>();
    }

    public void AddItem(Item weapon)
    {
        if (weapon == null)
            return;
        Item = weapon;
        icon.sprite = weapon.ItemIcon;
        icon.enabled= true;
        gameObject.SetActive(true);
    }

    public void ClearInventorySlot()
    {
        Item = null;
        icon.sprite = null;
        icon.enabled = false;
        gameObject.SetActive(false);
    }

    public void Upload()
    {
        var itemOfType = this.Item.GetType();
        if (HandEquipmentSlotUI.Action != null && itemOfType.IsSubclassOf(typeof(Item)))
        {
            HandEquipmentSlotUI current;
            if (itemOfType != typeof(Weapon))
            {
                HandEquipmentSlotUI.Action(out current);
                if (current.Index > 1)
                {
                    Debug.Log("NoWeapon");
                    HandEquipmentSlotUI.Action(out current,Item);
                    playerInventory.WeaponsInLeftHandSlots[current.Index % 2] = Item;
                }
            }
            else
            {
                HandEquipmentSlotUI.Action(out current);
                if (current.Index < 2)
                {
                    HandEquipmentSlotUI.Action(out current, Item);
                    playerInventory.WeaponsInRightHandSlots[current.Index] = Item;
                }
            }
        }
        HandEquipmentSlotUI.Action = null;
    }
}

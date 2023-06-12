using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private static UIManager uIManager;
    private static HandEquipmentSlotUI[] handEquipmentSlots;
    private void Start()
    {
        uIManager = FindObjectOfType<UIManager>();
        handEquipmentSlots = uIManager.EquipmentWindow.GetComponentsInChildren<HandEquipmentSlotUI>();
    }

    public static void RemoveFromAll(Item item)
    {
        RemoveItemFromUI(item);
        RemoveItemFromInventoryPlayer(item);
    }

    public static void RemoveItemFromUI(Item item)
    {
        handEquipmentSlots.FirstOrDefault(x => x.Item == item).ClearItem();
        var obj = uIManager.WeaponInventorySlotsParent.GetComponentsInChildren<WeaponInventorySlot>().FirstOrDefault(x => x.Item == item);
        Destroy(obj.gameObject);
        Debug.Log("Good1");
    }

    public static void RemoveItemFromInventoryPlayer(Item item)
    {
        item.CharacterStats.GetComponent<PlayerInventory>().RemoveItem(item);
        Debug.Log("Good2");
    }
}

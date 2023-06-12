using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;
using System.Linq;

public class HandEquipmentSlotUI : MonoBehaviour
{
    public Image Icon;
    public Item Item;
    public delegate void WeaponAction(out HandEquipmentSlotUI gameObj, Item newWeapon = null);
    public static WeaponAction Action { get; private set; }
    public int Index = 0;
    public static GridLayoutGroup Group;

    public static void ResetAction()
    {
        Action = null;
    }

    private void Awake()
    {
        Debug.Log("Bye");
        if (Group == null)
            Group = FindObjectsOfType<GridLayoutGroup>().First(x => x.name == "UI Background");
        Group.GetComponentsInChildren<HandEquipmentSlotUI>().FirstOrDefault(x => {
            if (x.transform == this.transform)
                return true;
            Index++;
            return false;
        });
        Debug.Log("Bye");
    }

    public void AddItem(out HandEquipmentSlotUI gameObj, Item newWeapon = null)
    {
        if (newWeapon == null)
        {
            gameObj = this;
            return;
        }
        gameObj = this;
        Item = newWeapon;
        Icon.sprite = Item.ItemIcon;
        Icon.enabled= true;
        gameObject.SetActive(true);
    }

    public void ClearItem()
    {
        Item = null;
        Icon.sprite = null;
        Icon.enabled = false;
    }

    public void Upload()
    {
        Action = AddItem;
    }
}

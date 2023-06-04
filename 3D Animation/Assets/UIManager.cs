using SG;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class UIManager : MonoBehaviour
{
    private PlayerInventory playerInventory;

    [Header("UI Windows")]
    public GameObject HudWindow;
    public GameObject SelectWindow;
    public GameObject WeaponInventoryWindow;
    public GameObject EquipmentWindow;

    [Header("Weapon Inventory")]
    public WeaponInventorySlot WeaponInventorySlotPrefab;
    public Transform WeaponInventorySlotsParent;
    private Dictionary<Item,WeaponInventorySlot> weaponInventorySlots;

    private void Start()
    {
        weaponInventorySlots = WeaponInventorySlotsParent
            .GetComponentsInChildren<WeaponInventorySlot>()
            .Where(x =>
        {
            if (x.Item == null)
            {
                WeaponInventorySlotPrefab = Instantiate(x);
                Destroy(x.gameObject);
                WeaponInventorySlotPrefab.gameObject.SetActive(false);
                return false;
            }
            return true;
        }).ToDictionary(x => x.Item);
        playerInventory = FindObjectOfType<PlayerInventory>();
    }

    public void UpdateUi()
    {
        #region Weapon Inventory Slots
        Debug.Log(playerInventory.WeaponsInventory.Count);
        foreach (var item in playerInventory.WeaponsInventory)
        {
            if (!weaponInventorySlots.ContainsKey(item))
            {
                var prefab = Instantiate(WeaponInventorySlotPrefab, WeaponInventorySlotsParent);
                weaponInventorySlots.Add(item, prefab);
                prefab.AddItem(item);
            }
        }

        Debug.Log(playerInventory.WeaponsInventory.Count);

        #endregion
    }

    public void OpenSelectWindow()
    {
        SelectWindow.SetActive(true);
    }

    public void CloseSelectWindow()
    {
        SelectWindow.SetActive(false);
    }

    public void CloseAllInventoryWindows()
    {
        WeaponInventoryWindow.SetActive(false);
    }

    public void CloseEquipmentWindow()
    {
        EquipmentWindow.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EquipmentSlot : MonoBehaviour, IArmorsEquipmentSlot
{
    private Dictionary<string, GameObject> _listArmors = new Dictionary<string, GameObject>();

    void IArmorsEquipmentSlot.Initialize()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            child.gameObject.SetActive(false);
            _listArmors.Add(child.name, child.gameObject);
        }
    }

    bool IArmorsEquipmentSlot.SetActive(string name, bool isActive)
    {
        if (_listArmors.ContainsKey(name))
        {
            _listArmors[name].SetActive(isActive);
            return true;
        }
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class EquipmentTwoArmors : MonoBehaviour, IArmorsEquipmentSlot
{
    [SerializeField] private Transform armorLeft;
    [SerializeField] private Transform armorRight;

    private Dictionary<string, (GameObject, GameObject)> listArmsUpper = new Dictionary<string, (GameObject, GameObject)>(); // First - left, Second - right

    void IArmorsEquipmentSlot.Initialize()
    {
        for (int i = 0; i < armorLeft.childCount; i++)
        {
            var childLeft = armorLeft.GetChild(i);
            var childRight = armorRight.GetChild(i);
            string name = childLeft.name.Replace("Left", "");
            name = name.Replace("Right", "");
            listArmsUpper[name] = new(childLeft.gameObject, childRight.gameObject);
            childLeft.gameObject.SetActive(false);
            childRight.gameObject.SetActive(false);
        }
    }

    bool IArmorsEquipmentSlot.SetActive(string name, bool isActive)
    {
        name = name.Replace("Right","");
        name = name.Replace("Left", "");
        if (listArmsUpper.ContainsKey(name))
        {
            listArmsUpper[name].Item1.SetActive(isActive);
            listArmsUpper[name].Item2.SetActive(isActive);
            return true;
        }
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SG
{
    public class WeaponHolderSlot : MonoBehaviour
    {
        public CharacterStats CharacterStats;
        public Transform parentOverride;
        public bool IsLeftHandSlot;
        public bool IsRightHandSlot;
        public bool IsBackSlot;

        public GameObject CurrentWeaponModel;

        public void UnloadWeapon()
        {
            if (CurrentWeaponModel != null)
            {
                CurrentWeaponModel.SetActive(false);
            }
        }

        public void UnloadWeaponAndDestroy()
        {
            if (CurrentWeaponModel != null)
            {
                Destroy(CurrentWeaponModel);
            }
        }

        public void LoadWeaponModel(Item weaponItem)
        {
            UnloadWeaponAndDestroy();
            if (weaponItem == null)
            {
                UnloadWeapon();
                return;
            }
            GameObject model = Instantiate(weaponItem.modelPrefab);
            if (weaponItem.GetType() == typeof(Weapon))
                model.GetComponentInChildren<DamageCollider>().UploadWeapon((Weapon)weaponItem, CharacterStats);
            else
                weaponItem.CharacterStats = CharacterStats;
            if (model != null)
            {
                if (parentOverride != null)
                {
                    model.transform.parent = parentOverride;
                    model.transform.localPosition = Vector3.zero;
                }
                else
                {
                    model.transform.parent = transform;
                }
                model.transform.localRotation = Quaternion.identity;
            }
            CurrentWeaponModel = model;
        }
    }
}

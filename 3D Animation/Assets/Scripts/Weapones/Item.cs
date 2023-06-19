using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Runtime.InteropServices.WindowsRuntime;

namespace SG
{
    public class Item : ScriptableObject
    {
        public CharacterStats CharacterStats;
        public string Id;
        public GameObject modelPrefab;
        [Header("Item Information")]
        public Sprite ItemIcon;
        public string ItemName;

        public virtual bool IsSpell()
        {
            return false;
        }

        public Item()
        {
            Id = Guid.NewGuid().ToString();
        }

        public virtual void Interact(bool isLight = false)
        {

        }

        public virtual bool IsShield()
        {
            return false;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            return ItemName == (obj as Item).ItemName;
        }

        public virtual float GetSofteningBlowInPercent()
        {
            return 1f;
        }

        public virtual ShieldWeapon GetShield()
        {
            return null;
        }

        public virtual bool IsAttackingWeapon()
        {
            return false;
        }

        public virtual ArmorItem GetArmorItem(out ArmorItem item)
        {
            item = null;
            return null;
        }
    }
}

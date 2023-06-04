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
        public string PlayerId;
        public string Id;
        public GameObject modelPrefab;
        [Header("Item Information")]
        public Sprite ItemIcon;
        public string ItemName;
        [Header("Idle Animations")]
        public string RightHandIdle;
        public string LeftHandIdle;

        public Item()
        {
            Id = Guid.NewGuid().ToString();
        }

        public virtual void Interact(bool isLight = false)
        {

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
    }
}

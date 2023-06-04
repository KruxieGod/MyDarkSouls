using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;

public enum Casters
{
    PyroCaster,
    FaithCaster,
    MagicCaster,
    MeleeCaster
}

namespace SG
{
    [CreateAssetMenu(menuName = "Items/Weapon Item")]
    public class Weapon : Item
    { 
        public bool IsUnarmed;

        [Header("Attack Animations")]
        public string[] LightsAttacks;
        public string[] HeavyAttacks;
        public string[] TwoHandedAttacks;

        [Header("Stamina Costs")]
        public int BaseStamina;
        public float LightAttackMultiplier;
        public float HeavyAttackMultiplier;

        [Header("Weapon Type")]
        public Casters Caster;

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override void Interact(bool isLight)
        {
            InputManager.Instance[PlayerId].WeaponAttack(isLight,this);
        }
    }
}

using SG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackingItem : Item
{
    [Header("Idle Animations")]
    [SerializeField] private string damageAnimation;
    public string DamageAnimation => damageAnimation;
    public string RightHandIdle;
    public string LeftHandIdle;

    public override bool IsAttackingWeapon()
    {
        return true;
    }
}

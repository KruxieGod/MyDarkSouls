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
    [Header("Poise Settings")]
    [SerializeField]
    private int poiseBreak;
    public int PoiseBreak => poiseBreak;

    public override bool IsAttackingWeapon()
    {
        return true;
    }
}

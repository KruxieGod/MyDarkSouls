using SG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Shield")]
public class ShieldWeapon : AttackingItem
{
    public bool isBlocking { get; private set; }
    [SerializeField] private string animationBlocking;
    [SerializeField]private int softeningBlowInPercent;
    public float SofteningBlowInPercent => (100 - softeningBlowInPercent) / 100f;

    public override void Interact(bool isLight = false)
    {
        if (!isBlocking)
            CharacterStats.GetComponent<CharacterAnimator>().PlayTargetAnimation(animationBlocking);
        else
            CharacterStats.GetComponent<CharacterAnimator>().PlayTargetAnimation(LeftHandIdle);
        isBlocking = !isBlocking;
    }

    public override bool IsShield()
    {
        return true;
    }

    public override float GetSofteningBlowInPercent()
    {
        if (isBlocking)
            return SofteningBlowInPercent;
        else
            return 1f;
    }

    public override ShieldWeapon GetShield()
    {
        return this;
    }
}

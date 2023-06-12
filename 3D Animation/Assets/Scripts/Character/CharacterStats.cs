using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public virtual bool IsHeavyAttack { get; }
    public int CriticalDamage;
    public virtual bool IsInvulnerability { get { return false; } protected set { IsInvulnerability = value; } }

    public virtual void TakeStaminaDamage(int damage)
    {

    }

    public virtual void TakeDamage(int damage,bool isInteracting = true)
    {

    }

    public virtual float GetSofteningBlowInPercent()
    {
        return 1f;
    }
}

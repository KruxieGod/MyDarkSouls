using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour , IArmorSettings
{
    public int CurrentHealth;
    public virtual bool IsHeavyAttack { get; }
    public int CriticalDamage;
    public virtual bool IsInvulnerability { get { return false; } protected set { IsInvulnerability = value; } }

    float IArmorSettings.damageAbsorptionHelmet { get; set; }
    float IArmorSettings.damageAbsorptionTorso { get; set; }
    float IArmorSettings.damageAbsorptionUpperArms { get; set; }
    float IArmorSettings.damageAbsorptionLowerArms { get; set; }
    float IArmorSettings.damageAbsorptionHips { get; set; }
    float IArmorSettings.damageAbsorptionLegs { get; set; }
    float IArmorSettings.damageAbsorptionHands { get; set; }

    public virtual void TakeStaminaDamage(int damage)
    {

    }

    public virtual void TakeDamage(int damage,bool isInteracting = true)
    {
        var iArmorSettings = (IArmorSettings)this;

        float totalPhysicalDamageAbsorption = 1 -
            (1 - iArmorSettings.damageAbsorptionTorso / 100) *
            (1 - iArmorSettings.damageAbsorptionHelmet / 100) *
            (1 - iArmorSettings.damageAbsorptionUpperArms / 100) *
            (1 - iArmorSettings.damageAbsorptionLowerArms / 100) *
            (1 - iArmorSettings.damageAbsorptionHips / 100) *
            (1 - iArmorSettings.damageAbsorptionLegs / 100) *
            (1 - iArmorSettings.damageAbsorptionHands / 100);
        damage = Mathf.RoundToInt(damage - (damage * totalPhysicalDamageAbsorption));
        CurrentHealth -= damage;
        Debug.Log("Damage Absorption: " + damage);
    }

    public virtual float GetSofteningBlowInPercent()
    {
        return 1f;
    }
}

internal interface IHealthBar
{
    internal void SetMaxHealth(int maxHealth);
    internal void SetCurrentHealth(int currentHealth);
}

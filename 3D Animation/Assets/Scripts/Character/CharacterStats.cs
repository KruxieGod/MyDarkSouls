using System;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterStats : MonoBehaviour , IArmorSettings
{
    public int CurrentHealth;
    public virtual bool IsHeavyAttack { get; }
    public int CriticalDamage;
    public virtual bool IsInvulnerability { get { return false; } set { } }
    [SerializeField] private PoiseLogic poiseLogic;
    private IPoiseLogic iPoiseLogic;
    protected bool damageIsAnimated;
    float IArmorSettings.damageAbsorptionHelmet { get; set; }
    float IArmorSettings.damageAbsorptionTorso { get; set; }
    float IArmorSettings.damageAbsorptionUpperArms { get; set; }
    float IArmorSettings.damageAbsorptionLowerArms { get; set; }
    float IArmorSettings.damageAbsorptionHips { get; set; }
    float IArmorSettings.damageAbsorptionLegs { get; set; }
    float IArmorSettings.damageAbsorptionHands { get; set; }
    private float totalPhysicalDamageAbsorption;
    internal AttackingItem AttackingItem;
    internal virtual void Start()
    {
        SetDamageAbsorption();
        iPoiseLogic = poiseLogic;
        if (!TryGetComponent(out EnemyStats enemyStats))
            iPoiseLogic.UpdatePoiseBonus((int)(100f*totalPhysicalDamageAbsorption));
        poiseLogic.Initialize();
    }

    internal virtual void Update()
    {
        if (poiseLogic.IsSpent)
            poiseLogic.OnUpdate();
    }

    public virtual void TakeStaminaDamage(int damage)
    {

    }

    internal void AddPoiseDuringAttack()
    {
        if (AttackingItem != null)
            iPoiseLogic.AddPoise(AttackingItem.PoiseBreak);
    }

    internal void RemovePoiseDuringAttack()
    {
        if (AttackingItem != null)
            iPoiseLogic.SpendPoise(AttackingItem.PoiseBreak);
    }

    public virtual void TakeDamage(int damage,bool isInteracting = true)
    {
        SetDamageAbsorption();
        iPoiseLogic.UpdatePoiseBonus((int)(100f*totalPhysicalDamageAbsorption));
        damage = Mathf.RoundToInt(damage - (damage * totalPhysicalDamageAbsorption));
        CurrentHealth -= damage;
        iPoiseLogic.SpendPoise(damage);
        damageIsAnimated = poiseLogic.PoiseIsSpent();
        Debug.Log("Damage Absorption: " + damage);
    }

    private void SetDamageAbsorption()
    {
        var iArmorSettings = (IArmorSettings)this;

        totalPhysicalDamageAbsorption = 1 -
                                        (1 - iArmorSettings.damageAbsorptionTorso / 100) *
                                        (1 - iArmorSettings.damageAbsorptionHelmet / 100) *
                                        (1 - iArmorSettings.damageAbsorptionUpperArms / 100) *
                                        (1 - iArmorSettings.damageAbsorptionLowerArms / 100) *
                                        (1 - iArmorSettings.damageAbsorptionHips / 100) *
                                        (1 - iArmorSettings.damageAbsorptionLegs / 100) *
                                        (1 - iArmorSettings.damageAbsorptionHands / 100);
    }

    public virtual float GetSofteningBlowInPercent()
    {
        return 1f;
    }
}

interface IPoiseLogic
{
    internal void SpendPoise(int damagePoise);
    internal void UpdatePoiseBonus(int poiseBonus);
    internal void AddPoise(int poiseTotal);
}

[Serializable]
public class PoiseLogic : IPoiseLogic
{
    [field: Header("Poise Settings")] 
    [SerializeField,Min(0)] private int armorPoiseBonus;
    [SerializeField,Min(0)] private float armorPoiseRecoverTime;
    private int totalArmorPoise;
    private float lastRecoveryTime;
    public bool IsSpent => totalArmorPoise < 0;

    public void Initialize()
    {
        totalArmorPoise = armorPoiseBonus;
        lastRecoveryTime = armorPoiseRecoverTime;
    }

    public void OnUpdate()
    {
        if (totalArmorPoise < 0)
            lastRecoveryTime -= Time.deltaTime;
        Debug.Log("<color=red>"+totalArmorPoise.ToString()+"</color>");
        if (!(lastRecoveryTime < 0)) return;
        totalArmorPoise = armorPoiseBonus;
        lastRecoveryTime = armorPoiseRecoverTime;
    }

    public bool PoiseIsSpent()
    {
        Debug.Log("<color=green>"+totalArmorPoise.ToString()+"</color>");
        return totalArmorPoise < 0;
    }

    void IPoiseLogic.SpendPoise(int damagePoise)
    {
        if (damagePoise < 0) return;
        totalArmorPoise -= damagePoise;
    }

    void IPoiseLogic.UpdatePoiseBonus(int poiseBonus)
    {
        armorPoiseBonus = poiseBonus;
        Debug.Log("<color=green>"+poiseBonus+"</color>");
    }

    void IPoiseLogic.AddPoise(int poiseTotal)
    {
        if (poiseTotal < 0) return;
        totalArmorPoise += poiseTotal;
    }
}
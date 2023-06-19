using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEquipmentManager : MonoBehaviour
{
    [SerializeField]private EqupmentInventory equpmentInventory;

    [SerializeField]private ArmorItem torsoArmorUnarmed;
    [SerializeField] private ArmorItem helmetArmorUnarmed;
    [SerializeField] private ArmorItem upperArmsArmorUnarmed;
    [SerializeField] private ArmorItem lowerArmsArmorUnarmed;
    [SerializeField] private ArmorItem hipsArmorUnarmed;
    [SerializeField] private ArmorItem legsArmorUnarmed;
    [SerializeField] private ArmorItem handsArmorUnarmed;

    private void Awake()
    {
        equpmentInventory.Initialize(GetComponent<CharacterStats>());
        SetUnarmedArmor();
    }

    public void SetArmor(ArmorItem armorItem)
    {
        switch (armorItem.PartArmor) 
        {
            case PartOfTheArmor.UpperArms:
                equpmentInventory.SetAnotherUpperArms(armorItem);
                break;
            case PartOfTheArmor.LowerArms:
                equpmentInventory.SetAnotherLowerArms(armorItem);
                break;
            case PartOfTheArmor.Hips:
                equpmentInventory.SetAnotherHips(armorItem);
                break;
            case PartOfTheArmor.Torso:
                equpmentInventory.SetAnotherTorso(armorItem);
                break;
            case PartOfTheArmor.Head:
                equpmentInventory.SetAnotherHelmet(armorItem);
                break;
            case PartOfTheArmor.Legs:
                equpmentInventory.SetAnotherLegs(armorItem);
                break;
            case PartOfTheArmor.Hands:
                equpmentInventory.SetAnotherHands(armorItem);
                break;
        }
    }

    private void SetUnarmedArmor()
    {
        equpmentInventory.SetAnotherTorso(torsoArmorUnarmed);
        equpmentInventory.SetAnotherHelmet(helmetArmorUnarmed);
        equpmentInventory.SetAnotherHands(handsArmorUnarmed);
        equpmentInventory.SetAnotherLowerArms(lowerArmsArmorUnarmed);
        equpmentInventory.SetAnotherLegs(legsArmorUnarmed);
        equpmentInventory.SetAnotherHips(hipsArmorUnarmed);
        equpmentInventory.SetAnotherUpperArms(upperArmsArmorUnarmed);
    }
}

[Serializable]
internal sealed class EqupmentInventory
{
    [Header("Transforms slots")]
    [SerializeField]Transform helmetTransform;
    [SerializeField]Transform torsoTransform;
    [SerializeField]Transform upperArmsTransform;
    [SerializeField]Transform lowerArmsTransform;
    [SerializeField]Transform hipsTransform;
    [SerializeField]Transform legsTransform;
    [SerializeField]Transform handsTransform;

    IArmorsEquipmentSlot helmetsSlot;
    IArmorsEquipmentSlot torsesSlot;
    IArmorsEquipmentSlot upperArmsSlot;
    IArmorsEquipmentSlot lowerArmsSlot;
    IArmorsEquipmentSlot hipsSlot;
    IArmorsEquipmentSlot legsSlot;
    IArmorsEquipmentSlot handsSlot;

    [Header("Names current Armors")]
    string currentHelmet;
    string currentTorso;
    string currentUpperArms;
    string currentLowerArms;
    string currentHips;
    string currentLegs;
    string currentHands;

    private IArmorSettings iArmorSettings;

    public void Initialize(CharacterStats characterStats)
    {
        iArmorSettings = characterStats;
        GetSlots();
        InitializeSlots();
        //SetArmorsCurrent();
    }
    #region Initialize 
    private void GetSlots()
    {
        helmetsSlot = helmetTransform.GetComponent<EquipmentSlot>();
        torsesSlot = torsoTransform.GetComponent<EquipmentSlot>();
        hipsSlot = hipsTransform.GetComponent<EquipmentSlot>();
        upperArmsSlot = upperArmsTransform.GetComponent<EquipmentTwoArmors>();
        lowerArmsSlot = lowerArmsTransform.GetComponent<EquipmentTwoArmors>();
        legsSlot = legsTransform.GetComponent<EquipmentTwoArmors>();
        handsSlot = handsTransform.GetComponent<EquipmentTwoArmors>();
    }

    private void InitializeSlots()
    {
        helmetsSlot.Initialize();
        torsesSlot.Initialize();
        upperArmsSlot.Initialize();
        lowerArmsSlot.Initialize();
        hipsSlot.Initialize();
        legsSlot.Initialize();
        handsSlot.Initialize();
    }
    #endregion
    #region SetterSlots
    public void SetAnotherHands(ArmorItem name)
    {
        if (SetAnotherArmor(handsSlot, name.NameOfGameObject, ref currentHands))
            iArmorSettings.damageAbsorptionHands = name.DamageAbsorption;
    }

    public void SetAnotherLegs(ArmorItem name)
    {
        if (SetAnotherArmor(legsSlot, name.NameOfGameObject,ref currentLegs))
            iArmorSettings.damageAbsorptionLegs = name.DamageAbsorption;
    }

    public void SetAnotherHips(ArmorItem name)
    {
        if (SetAnotherArmor(hipsSlot, name.NameOfGameObject, ref currentHips))
            iArmorSettings.damageAbsorptionHips = name.DamageAbsorption;
    }

    public void SetAnotherLowerArms(ArmorItem name)
    {
        if (SetAnotherArmor(lowerArmsSlot, name.NameOfGameObject, ref currentLowerArms))
            iArmorSettings.damageAbsorptionLowerArms = name.DamageAbsorption;
    }

    public void SetAnotherUpperArms(ArmorItem name)
    {
        if (SetAnotherArmor(upperArmsSlot, name.NameOfGameObject, ref currentUpperArms))
            iArmorSettings.damageAbsorptionUpperArms = name.DamageAbsorption;
    }

    public void SetAnotherHelmet(ArmorItem name)
    {
        if (SetAnotherArmor(helmetsSlot,name.NameOfGameObject,ref currentHelmet))
            iArmorSettings.damageAbsorptionHelmet = name.DamageAbsorption;
    }

    public void SetAnotherTorso(ArmorItem name)
    {
        if (SetAnotherArmor(torsesSlot, name.NameOfGameObject, ref currentTorso))
            iArmorSettings.damageAbsorptionTorso = name.DamageAbsorption;
    }

    private bool SetAnotherArmor(IArmorsEquipmentSlot armorSlot,string name,ref string currentArmor)
    {
        if (!armorSlot.SetActive(name, true))
            return false;
        if (currentArmor != null)
            armorSlot.SetActive(currentArmor, false);

        currentArmor = name;
        return true;
    }
    #endregion
}

internal interface IArmorsEquipmentSlot
{
    internal protected void Initialize();
    internal protected bool SetActive(string name,bool isActive);
}

internal interface IArmorSettings
{
    float damageAbsorptionHelmet { get; internal set; }
    float damageAbsorptionTorso { get; internal set; }
    float damageAbsorptionUpperArms { get; internal set; }
    float damageAbsorptionLowerArms { get; internal set; }
    float damageAbsorptionHips { get; internal set; }
    float damageAbsorptionLegs { get; internal set; }
    float damageAbsorptionHands { get; internal set; }
}

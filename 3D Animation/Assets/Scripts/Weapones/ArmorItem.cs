using SG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/ArmorItem")]
public class ArmorItem : Item
{
    [SerializeField] private float damageAbsorption;
    public float DamageAbsorption => damageAbsorption;
    [SerializeField] private string nameOfGameObject;
    public string NameOfGameObject => nameOfGameObject;
    public PartOfTheArmor PartArmor { get; private set; }

    public PartOfTheArmor PartOfTheArmor()
    {
        if (PartArmor == global::PartOfTheArmor.Weapon)
            PartArmor = GetPart();
        return PartArmor;
    }

    private PartOfTheArmor GetPart()
    {
        if (nameOfGameObject.Contains("Hand"))
            return global::PartOfTheArmor.Hands;
        else if (nameOfGameObject.Contains("Head"))
            return global::PartOfTheArmor.Head;
        else if (nameOfGameObject.Contains("Hips"))
            return global::PartOfTheArmor.Hips;
        else if (nameOfGameObject.Contains("Leg"))
            return global::PartOfTheArmor.Legs;
        else if (nameOfGameObject.Contains("ArmLower"))
            return global::PartOfTheArmor.LowerArms;
        else if (nameOfGameObject.Contains("Torso"))
            return global::PartOfTheArmor.Torso;
        else if (nameOfGameObject.Contains("ArmUpper"))
            return global::PartOfTheArmor.UpperArms;

        throw new System.Exception();
    }

    public override bool IsAttackingWeapon()
    {
        return false;
    }

    public override ArmorItem GetArmorItem(out ArmorItem armorItem)
    {
        armorItem = this;
        return this;
    }
}

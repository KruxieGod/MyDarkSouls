using SG;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public enum Spells
{
    FaithSpell,
    PyroSpell,
    MagicSpell
}

public class SpellItem : AttackingItem
{
    [SerializeField]protected GameObject spellWarmUpFX;
    public GameObject SpellWarmUpFX => spellWarmUpFX;
    [SerializeField]protected GameObject spellCastFX;
    public GameObject SpellCastFX => spellCastFX;
    public string SpellAnimation;
    public int CostSpell;

    [Header("Spell Type")]
    public Spells Spell;

    [Header("Spell Description")]
    [TextArea]
    public string SpellDescription;

    public override bool IsSpell()
    {
        return true;
    }
}

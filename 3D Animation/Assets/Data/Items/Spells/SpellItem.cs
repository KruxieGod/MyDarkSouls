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

public class SpellItem : Item
{
    public GameObject SpellWarmUpFX;
    public GameObject SpellCastFX;
    public string SpellAnimation;
    public int CostSpell;

    [Header("Spell Type")]
    public Spells Spell;

    [Header("Spell Description")]
    [TextArea]
    public string SpellDescription;
}

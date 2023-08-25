using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterWeaponHolderSlotManager : MonoBehaviour
{
    protected IWeaponFX rightAttackingWeaponFX;
    public virtual void CloseRightHandDamageCollider()
    {
        
    }

    internal void StopParticlesWeapon()
    {
        if (rightAttackingWeaponFX == null) return;
        rightAttackingWeaponFX.SetInactive();
    }
    
    internal void PlayParticlesWeapon()
    {
        Debug.Log("EnemyPlayedParticles: " + this.gameObject.name);
        if (rightAttackingWeaponFX == null) return;
        rightAttackingWeaponFX.SetActive();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopParticlesFromWeapon : StateMachineBehaviour
{
    private CharacterWeaponHolderSlotManager characterWeaponHolderSlotManager;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        characterWeaponHolderSlotManager = animator.GetComponent<CharacterWeaponHolderSlotManager>();
        characterWeaponHolderSlotManager.StopParticlesWeapon();
        Debug.Log("STOPPED PARTICLES");
    }
}

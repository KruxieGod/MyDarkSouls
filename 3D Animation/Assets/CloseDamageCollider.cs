using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseDamageCollider : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<CharacterWeaponHolderSlotManager>().CloseRightHandDamageCollider();
    }
}

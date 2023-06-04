using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBoolFalling : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isInteracting",true);
        animator.SetBool("IsFalling", true);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsFalling", false);
    }
}

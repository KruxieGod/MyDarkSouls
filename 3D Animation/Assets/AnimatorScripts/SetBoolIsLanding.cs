using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBoolIsLanding : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsLanding", true);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsLanding", false);
    }
}

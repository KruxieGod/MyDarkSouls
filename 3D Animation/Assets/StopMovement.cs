using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopMovement : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("AnimationStarted");
        animator.SetFloat("Vertical",0);
        animator.SetFloat("Horizontal", 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBoolDodge : StateMachineBehaviour
{
    public string IsUsingRootMotionBool;
    public bool isUsingRootMotionStatus;

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(IsUsingRootMotionBool, isUsingRootMotionStatus);
    }
}

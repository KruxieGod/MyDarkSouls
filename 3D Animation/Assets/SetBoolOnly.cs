using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBoolOnly : StateMachineBehaviour
{
    public string Name;
    public bool Status;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(Name, Status);
    }
}

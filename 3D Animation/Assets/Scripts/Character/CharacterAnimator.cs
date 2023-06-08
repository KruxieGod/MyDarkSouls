using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterAnimator : MonoBehaviour
{
    public abstract void PlayTargetAnimation(string targetAnimation, bool useRootMotion = false, bool isInteracting = false, float time = 0);
}

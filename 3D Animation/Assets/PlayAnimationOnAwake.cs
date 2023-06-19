using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimationOnAwake : MonoBehaviour
{
    [SerializeField] private string nameAnimation;
    private void Awake()
    {
        GetComponent<Animator>().Play(nameAnimation);
    }
}

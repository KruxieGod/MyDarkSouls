using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG;

public class CharacterManager : MonoBehaviour
{
    public virtual bool IsParried { get; private set; }
    [SerializeField]internal Transform LockOnTransform;
    [SerializeField]private Transform backStabPoint;
    public Transform BackStabPoint => backStabPoint;
    [SerializeField] private Transform forwardStabPoint;
    public Transform ForwardStabPoint => forwardStabPoint;
}

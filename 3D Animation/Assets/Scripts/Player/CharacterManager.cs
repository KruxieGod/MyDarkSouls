using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG;

public class CharacterManager : MonoBehaviour
{
    public Transform LockOnTransform;
    [SerializeField]private Transform backStabPoint;
    public Transform BackStabPoint => backStabPoint;
}

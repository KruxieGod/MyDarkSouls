using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatTransformWeapon : MonoBehaviour
{
    private Transform startedTransform;
    public Quaternion TwoHandedRotation;
    public Vector3 TwoHandedPosition;
    void Start()
    {
        startedTransform  = transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

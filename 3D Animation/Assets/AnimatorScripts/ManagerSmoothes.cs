using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerSmoothes : MonoBehaviour
{
    public float f,z,r;
    private float _f, _z, _r;
    public Transform X;
    private SecondOrderDynamics secondOrder;
    private Vector3 velocityVector;
    public float speedVelocity = 1f;
    private void Start()
    {
        velocityVector = Vector3.zero;
        _f = f;
        _z= z;
        _r= r;
        secondOrder = new SecondOrderDynamics(f, z, r, X.position);
    }

    private void Update()
    {
        if (f!= _f || _z != z || _r != r)
        {
            _f = f;
            _z = z;
            _r = r;
            secondOrder = new SecondOrderDynamics(f,z,r,X.position);
        }
        transform.position = secondOrder.Update(Time.deltaTime, X.position);
    }
}

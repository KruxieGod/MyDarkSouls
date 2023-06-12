using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerSmoothes : MonoBehaviour
{
    public float f,z,r;
    private float _f, _z, _r;
    private Transform x = null;
    private SecondOrderDynamics secondOrder;
    private Vector3 velocityVector;
    public float speedVelocity = 1f;
    private void Start()
    {
        velocityVector = Vector3.zero;
        _f = f;
        _z= z;
        _r= r;
        secondOrder = new SecondOrderDynamics(f, z, r, Vector3.zero);
    }

    private void Update()
    {
        if (x == null)
        {
            x = GetComponent<SoulsPursuePlayer>()?.Player;
            return;
        }

        if (f!= _f || _z != z || _r != r)
        {
            _f = f;
            _z = z;
            _r = r;
            secondOrder = new SecondOrderDynamics(f,z,r,x.position);
        }
        transform.position = secondOrder.Update(Time.deltaTime, x.position);
    }
}

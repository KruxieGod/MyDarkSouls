using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondOrderDynamics
{
    private Vector3 xp; // previous input
    private Vector3 y, yd; // state variables
    private float k1, k2, k3; // dynamics constants

    public SecondOrderDynamics(float f, float z, float r, Vector3 x0)
    {
        k1 = z / (Mathf.PI*f);
        k2 = 1 / ((2 * Mathf.PI * f) * (2 * Mathf.PI * f));
        k3 = r * z / (2 * Mathf.PI * f);
        // initialize variables
        xp = x0;
        y = x0;
        yd = Vector3.zero;
    }

    public Vector3 Update(float T, Vector3 x, Vector3? xd = null)
    {

        if (xd == null)
        { // estimate velocity
            xd = (x - xp) / T;
            xp = x;
        }

        float k2_stable = Mathf.Max(k2, 1.1f * (T * T / 4 + T * k1 / 2)); // clamp k2 to guarantee stability
        y = y + T * yd; // integrate position by velocity
        yd = yd + T * (x + k3 * (Vector3)xd - y - k1 * yd) / k2_stable; // integrate velocity by acceleration
        return y;
    }
}
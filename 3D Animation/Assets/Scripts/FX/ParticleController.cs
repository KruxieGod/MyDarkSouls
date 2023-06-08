using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleController : MonoBehaviour
{
    private void OnParticleSystemStopped()
    {
        Debug.Log("GG");
        Destroy(gameObject);
    }
}

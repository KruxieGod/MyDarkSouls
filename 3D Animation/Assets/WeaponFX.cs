using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFX : MonoBehaviour,IWeaponFX
{
    [SerializeField] private Transform top;
    [SerializeField] private Transform down;
    [SerializeField]private ParticleSystem particleSystem;

    private void Awake()
    {
        particleSystem.Stop();
        float magnitude = Vector3.Distance(top.localPosition, down.localPosition);
        Vector3 position = particleSystem.transform.localPosition;
        particleSystem.transform.localPosition = new Vector3(position.x, top.localPosition.y - (magnitude/2f),position.z);
        var particleSystemShape = particleSystem.shape;
        particleSystemShape.scale = new Vector3(magnitude/2f,1f,1f);
    }

    void IWeaponFX.SetActive()
    {
        if (!particleSystem.isStopped) return;
        Debug.Log("PARTICLE SYSTEM: " + particleSystem.gameObject.name);
        particleSystem.Play();
    }

    void IWeaponFX.SetInactive()
    {
        Debug.Log("PARTICLESYSTEM IS STOPPED");
        particleSystem.Stop();
    }
}

public interface IWeaponFX
{
    internal void SetActive();
    internal void SetInactive();
}

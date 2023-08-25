using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class SoulsPursuePlayer : MonoBehaviour
{
    [SerializeField] private float offcetPlayerVelocity;
    [SerializeField] private float multiplier;
    [SerializeField] private float multiplierY;
    [SerializeField]private ParticleSystem _particleSystem;
    public Transform Player { get; private set; }
    private VelocityOverLifetimeModule vel;
    CharacterController characterController;

    private void Awake()
    {
        vel = _particleSystem.velocityOverLifetime;
        vel.enabled = true;
        vel.space = ParticleSystemSimulationSpace.Local;
        Player = FindAnyObjectByType<PlayerManager>().transform;
        characterController = Player.GetComponent<CharacterController>();
        Player = Player.GetComponent<PlayerManager>().LockOnTransform;
    }

    void Update()
    {
        Vector3 directionToPlayer = (Player.position - transform.position) + (characterController.velocity.magnitude > 1f ? characterController.velocity *offcetPlayerVelocity : Vector3.zero);
        vel.xMultiplier = directionToPlayer.x * multiplier;
        vel.yMultiplier = directionToPlayer.y * multiplierY;
        vel.zMultiplier = directionToPlayer.z * multiplier;
    }
}

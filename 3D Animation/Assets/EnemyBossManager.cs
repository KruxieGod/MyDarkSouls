using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBossManager : MonoBehaviour
{
    private EnemyWeaponSlotManager enemyWeaponSlotManager;
    [SerializeField] private string namePositionOnParticles;
    [SerializeField] private ParticleSystem particlesOnSecondPhase;
    private bool isStartedBossFight;
    [SerializeField]
    private IEnemyPhasesManager iEnemyPhasesManager;
    [SerializeField]
    private EnemyHealthBarBossManager enemyHealthBarBossManager;

    private void Awake()
    {
        enemyWeaponSlotManager = GetComponent<EnemyWeaponSlotManager>();
        iEnemyPhasesManager = GetComponent<IEnemyPhasesManager>();
        iEnemyPhasesManager.Initialize();
        enemyHealthBarBossManager.Initialize(iEnemyPhasesManager, this);
    }

    internal void SetParticlesOnSecondPhase()
    {
        var boxCollider = enemyWeaponSlotManager.RightHandSlot.CurrentWeaponModel.GetComponentInChildren<BoxCollider>();
        var particlesInScene = Instantiate(particlesOnSecondPhase, boxCollider.transform.Find(namePositionOnParticles));
    }
}

interface IEnemyPhasesManager
{
    void Initialize();
    void SetSecondPhaseBoss();
    void SetThirdPhaseBoss();
}
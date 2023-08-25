using System;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

[Serializable]
class EnemyHealthBarBossManager
{
    private EnemyManager enemyManager;
    private EnemyStats enemyStats;
    [SerializeField] private string nameBoss;
    private static IHealthBar healthBar;
    private IEnemyPhasesManager iEnemyPhasesManager;
    private MonoBehaviour currentObject;
    public void Initialize(IEnemyPhasesManager iEnemyPhasesManager,MonoBehaviour currentObject)
    {
        this.iEnemyPhasesManager = iEnemyPhasesManager;
        this.currentObject = currentObject;
        if (healthBar == null)
        {
            var hb = UnityEngine.Object.FindAnyObjectByType<EnemyBossHealthBar>();
            hb.gameObject.SetActive(false);
            healthBar = hb;
        }
        enemyStats = currentObject.GetComponent<EnemyStats>();
        enemyManager = currentObject.GetComponent<EnemyManager>();
        enemyStats.UpdateHealthBar = () => healthBar;
        GlobalEventManager.OnBossFightEventBegin.AddListener(SetHealthBarEvent);
        GlobalEventManager.OnBossFightEventEnd.AddListener(SetOffHealthBar);
    }

    private void SetOffHealthBar()
    {
        Debug.Log("ENEMYBOSSMANAGER OFF");
        if (currentObject == null)
        {
            ResetEventsSetAndOff();
        }
        enemyStats.SetOffHealthBar();
        GlobalEventManager.OnBossPhases.RemoveAllListeners();
        enemyManager.ResetAttacks();
    }

    private void ResetEventsSetAndOff()
    {
        GlobalEventManager.OnBossFightEventBegin.RemoveListener(SetHealthBarEvent);
        GlobalEventManager.OnBossFightEventEnd.RemoveListener(SetOffHealthBar);
    }

    private void SetHealthBarEvent()
    {
        if (currentObject == null)
        {
            ResetEventsSetAndOff();
            return;
        }
        enemyStats.InitializeEvents();
        enemyStats.SetName(nameBoss);
        GlobalEventManager.OnBossPhases.AddListener(iEnemyPhasesManager.SetSecondPhaseBoss);
    }
}

using UnityEngine;

class EnemyPhasesManager : MonoBehaviour, IEnemyPhasesManager
{
    [Header("Second Phase")]
    [SerializeField] private string nameAnimationInPhase;
    [SerializeField] private int hpToStartSecondPhase;
    [SerializeField] private EnemyAttackAction[] enemyAttackActionsSecondPhase;
    [Header("Third Phase")]
    [SerializeField] private int hpToStartThirdPhase;
    [SerializeField] private EnemyAttackAction[] enemyAttackActionsThirdPhase;
    private EnemyAnimatorManager enemyAnimatorManager;
    private EnemyStats enemyStats;
    private EnemyManager enemyManager;

    public void Initialize()
    {
        enemyManager = GetComponent<EnemyManager>();
        enemyStats = GetComponent<EnemyStats>();
        enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
    }

    public void SetSecondPhaseBoss()
    {
        if (hpToStartSecondPhase >= enemyStats.CurrentHealth)
        {
            Debug.Log("Second Phase");
            enemyAnimatorManager.PlayTargetAnimation(nameAnimationInPhase, true);
            enemyStats.IsInvulnerability = true;
            enemyManager.AddNewAttacks(enemyAttackActionsSecondPhase);
            GlobalEventManager.OnBossPhases.RemoveListener(SetSecondPhaseBoss);
            GlobalEventManager.OnBossPhases.AddListener(SetThirdPhaseBoss);
        }
    }

    public void SetThirdPhaseBoss()
    {
        if (hpToStartThirdPhase >= enemyStats.CurrentHealth)
        {
            Debug.Log("Third Phase");
            enemyManager.AddNewAttacks(enemyAttackActionsThirdPhase);
            GlobalEventManager.OnBossPhases.RemoveListener(SetThirdPhaseBoss);
        }
    }
}

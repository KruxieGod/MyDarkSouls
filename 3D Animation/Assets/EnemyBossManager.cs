using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossManager : MonoBehaviour
{
    [SerializeField] private string nameBoss;
    private void Awake()
    {
        var enemyStats = GetComponent<EnemyStats>();
        enemyStats.UpdateHealthBar = () => FindAnyObjectByType<EnemyBossHealthBar>();
        enemyStats.InitializeEvents();
        enemyStats.SetName(nameBoss);
    }
}

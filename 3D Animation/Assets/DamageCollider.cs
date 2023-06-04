using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG;
public class DamageCollider : MonoBehaviour
{
    Collider damageCollider;
    public int currentWeaponDamage = 25;
    private PlayerAttacker playerAttacker;

    private void Awake()
    {
        playerAttacker = FindAnyObjectByType<PlayerAttacker>();
        damageCollider = GetComponent<Collider>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger= true;
        damageCollider.enabled = false;
    }

    public void EnableDamageCollider()
    {
        damageCollider.enabled = true;
    }

    public void DisableDamageCollider()
    {
        damageCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other) // использование isinteracting для того , чтобы объект не мог двигаться
    {
        if (other.tag == "Player")
        {
            PlayerStats playerStats = other.GetComponent<PlayerStats>();
            if (playerStats != null && !playerStats.IsInvulnerability)
            {
                playerStats.TakeDamage(currentWeaponDamage,false);
            }
        }
        if (other.tag == "Enemy")
        {
            EnemyStats enemyStats = other.GetComponent<EnemyStats>();
            if (enemyStats != null)
            {
                enemyStats.TakeDamage(playerAttacker.IsHeavy? 2*currentWeaponDamage: currentWeaponDamage,false);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    public int damage = 25;
    private float time = 0f;
    private PlayerStats playerStats;

    private void OnTriggerEnter(Collider other)
    {
        time = 0f;
        if (other.TryGetComponent(out playerStats))
            playerStats.TakeDamage(damage, false);
    }

    private void OnTriggerStay(Collider other)
    {
        time += Time.deltaTime;
        playerStats = other.GetComponent<PlayerStats>();
        if (playerStats != null && time >0.5f)
        {
            time = 0f;
            playerStats.TakeDamage(damage,false);
        }
    }
}

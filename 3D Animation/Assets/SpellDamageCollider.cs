using SG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellDamageCollider : MonoBehaviour
{
    public GameObject ParticlesAfterDeath;
    public int Damage;
    public Item Item;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            var characterStats = other.GetComponent<CharacterStats>();
            if (Item.CharacterStats == characterStats)
                return;
            characterStats.TakeDamage(Damage,false);
        }

        Destroy(Instantiate(ParticlesAfterDeath, transform.position, Quaternion.identity),5f);
        Destroy(gameObject);
    }
}

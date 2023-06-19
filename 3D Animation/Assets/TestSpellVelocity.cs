using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestSpellVelocity : MonoBehaviour
{
    [SerializeField] private ProjectialeSpell instance;
    private float time = 4f;
    // Update is called once per frame
    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0)
        {
            time = 4f;
            var collider = Instantiate(instance.SpellWarmUpFX, transform.position, Quaternion.identity).GetComponent<SpellDamageCollider>();
            collider.Damage = instance.amountDamage;
            collider.ParticlesAfterDeath = instance.SpellCastFX;
            var rb = collider.GetComponent<Rigidbody>();
            rb.mass = instance.mass;
            rb.AddForce(Vector3.up * instance.gravityUp);
            rb.AddForce(transform.forward * instance.gravityForward);

            Debug.Log("PLEE");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG;
public class DamageCollider : MonoBehaviour
{
    private Weapon weapon;
    Collider damageCollider;

    private void Awake()
    {
        damageCollider = GetComponent<Collider>();
        damageCollider.gameObject.SetActive(true);
        damageCollider.isTrigger= true;
        damageCollider.enabled = false;
    }

    public void UploadWeapon(Weapon weapon,CharacterStats character)
    {
        this.weapon = weapon;
        weapon.CharacterStats = character;
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
        Debug.Log(other.tag);
        if ((!other.CompareTag("Player") && !other.CompareTag("Enemy")))
            return;
        var characterStats = other.GetComponent<CharacterStats>();
        if (characterStats == weapon.CharacterStats)
            return;
        if (characterStats.GetComponent<CharacterAnimator>().IsStabbing)
        {
            Debug.Log(weapon.CharacterStats);
            weapon.CharacterStats.GetComponent<CharacterAnimator>().PlayTargetAnimation("Parried", true);
            characterStats.TakeStaminaDamage(weapon.Damage);
            return;
        }
        if (!characterStats.IsInvulnerability)
        {
            characterStats.TakeDamage( (int)((weapon.CharacterStats.IsHeavyAttack ? (int)(weapon.HeavyAttackMultiplier * weapon.Damage) : weapon.Damage)*characterStats.GetSofteningBlowInPercent()), false);
        }
    }
}

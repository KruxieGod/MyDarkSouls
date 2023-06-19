using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/ProjectialeSpell")]
public class ProjectialeSpell : SpellItem
{
    [SerializeField]public float mass;
    [SerializeField] public float gravityUp;
    [SerializeField] public float gravityForward;
    [SerializeField] public int amountDamage;
    public override void Interact(bool isFinished = false)// isFinished - true , когда нужно завершить каст , isFinished - false, когда нужно начать каст
    {
        if (isFinished)
            SuccessfullySpell();
        else
            AttemptToCast();
    }

    private void AttemptToCast()
    {
        Debug.Log("Attempt to cast a spell");
        var animatorManager = CharacterStats.GetComponent<AnimatorManager>();
        if (animatorManager.animator.GetBool("isInteracting"))
            return;
        animatorManager.PlayTargetAnimation(SpellAnimation, true, true);
        animatorManager.animator.SetBool("IsAttacking", true);
    }

    private void SuccessfullySpell()
    {
        Debug.Log("Successfully Spell");
        var animatorManager = CharacterStats.GetComponent<AnimatorManager>();
        animatorManager.animator.SetBool("IsAttacking", false);
        var collider = Instantiate(SpellWarmUpFX, animatorManager.GetComponent<WeaponSlotManager>().leftHandSlot.transform.position, Quaternion.identity).GetComponent<SpellDamageCollider>();
        collider.Damage = amountDamage;
        collider.ParticlesAfterDeath = SpellCastFX;
        collider.Item = this;
        var rb = collider.GetComponent<Rigidbody>();
        rb.mass = mass;
        rb.AddForce(Vector3.up*gravityUp);
        rb.AddForce(CharacterStats.transform.forward * gravityForward);

    }
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Heal Spell")]
public class HealSpell : SpellItem
{
    [SerializeField] private int countUsed = 3;
    [HideInInspector] public int CountLast;
    public int AmountHealHealth;

    public override void Interact(bool isLight = false) // isLight - true , ����� ����� ��������� ���� , isLight - false, ����� ����� ������ ����
    {
        if (isLight)
            SuccessfullySpell();
        else
            AttemptToSpell();
    }

    private void AttemptToSpell()
    {
        Debug.Log("Attempt to cast a spell");
        var animatorManager = InputManager.Instance[PlayerId].GetComponent<AnimatorManager>();
        if (animatorManager.animator.GetBool("isInteracting"))
            return;
        Destroy(Instantiate(SpellWarmUpFX, InputManager.Instance[PlayerId].GetComponent<WeaponSlotManager>().leftHandSlot.transform),0.65f);
        animatorManager.PlayTargetAnimation(SpellAnimation,true,true);
        animatorManager.animator.SetBool("IsAttacking", true);
    }

    private void SuccessfullySpell()
    {
        CountLast--;
        Debug.Log("Successfully Spelled");
        Destroy(Instantiate(SpellCastFX, InputManager.Instance[PlayerId].GetComponent<WeaponSlotManager>().leftHandSlot.transform),0.5f);
        InputManager.Instance[PlayerId].GetComponent<PlayerStats>().AddHealth(AmountHealHealth);
        Debug.Log("countLost : " + CountLast);
        if (CountLast == 0)
            GameManager.RemoveFromAll(this);
    }

    private void OnEnable()
    {
        CountLast = countUsed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG;
using System.Linq;
using UnityEngine.InputSystem;

public class PlayerAttacker : MonoBehaviour
{
    [SerializeField] private float criticalDamageMultiplier;
    [SerializeField] private float distanceToBackStab;
    [SerializeField] private float angle;
    [SerializeField] private LayerMask layerDetection;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform criticalStartPoint;
    private PlayerStats playerStats;
    private PlayerInventory playerInventory;
    private Vector3 targetDirectionTo;
    private AnimatorManager animatorManager;
    private InputManager inputManager;
    private PlayerManager playerManager;
    private WeaponSlotManager weaponSlotManager;
    private PlayerLocomotion playerLocomotion;
    public int LastAttack;
    private string[] attacks;
    public bool IsHeavy;
    private void Awake()
    {
        playerStats = GetComponent<PlayerStats>();
        playerInventory = GetComponent<PlayerInventory>();
        playerManager = GetComponent<PlayerManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        weaponSlotManager = GetComponent<WeaponSlotManager>();
        inputManager = GetComponent<InputManager>();
        animatorManager = GetComponent<AnimatorManager>();
    }

    public void HandleWeaponCombo(Weapon weapon)
    {
        if (!inputManager.ComboFlag || playerStats.CurrentStamina <=0)
            return;
        animatorManager.animator.SetBool("CanDoCombo", false);
        animatorManager.PlayTargetAnimation(attacks[(++LastAttack)% attacks.Length], true, true);
        RotateToNerbyEnemy();
    }

    public void HandleLightAttack(Weapon weapon)
    {
        if (playerStats.CurrentStamina <= 0)
            return;
        weaponSlotManager.AttackingWeapon= weapon;
        IsHeavy = false;
        attacks = !inputManager.G || weapon.IsUnarmed ? weapon.LightsAttacks : weapon.TwoHandedAttacks;
        int index = Random.Range(0, attacks.Length);
        string randomAttack = attacks[index];
        LastAttack= index;
        RotateToNerbyEnemy();
        animatorManager.PlayTargetAnimation(randomAttack, true,true);
    }

    public void HandleHeavyAttack(Weapon weapon)//by RootMotion use rotation in playerLocomotion
    {
        if (playerStats.CurrentStamina <= 0)
            return;
        weaponSlotManager.AttackingWeapon = weapon;
        IsHeavy = true;
        attacks = !inputManager.G || weapon.IsUnarmed ? weapon.HeavyAttacks: weapon.TwoHandedAttacks;
        int index = Random.Range(0, attacks.Length);
        string randomAttack = attacks[index];
        LastAttack = index;
        RotateToNerbyEnemy();
        animatorManager.PlayTargetAnimation(randomAttack, true,true);
    }

    public void RotateToNerbyEnemy()
    {
        RotationToTarget(true);
    }

    private bool RotationToTarget(bool onTarget = false)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5f, layerDetection);
        if (colliders.Length > 0)
        {
            var character = colliders.OrderBy(x => Vector3.Distance(transform.position, x.transform.position))
                .FirstOrDefault(x =>
                {
                    return x.transform != transform && 
                    Vector3.Angle(transform.forward, x.transform.position - transform.position) <= angle &&
                    Physics.Raycast(criticalStartPoint.position,x.transform.position - criticalStartPoint.position,out var hit) &&
                    hit.collider.gameObject.layer != wallLayer;
                });
            Debug.Log(character);
            if (character == null)
                return false;
            var direction = character.transform.position - transform.position;
            targetDirectionTo = direction.normalized;
            if (onTarget)
                transform.rotation = Quaternion.LookRotation(targetDirectionTo);
            return true;
        }
        return false;
    }

    public void AttemptToBackStabAttack()
    {
        if (RotationToTarget() && 
            Physics.Raycast(criticalStartPoint.position, targetDirectionTo, out var hit, distanceToBackStab, layerDetection) &&
            hit.collider.TryGetComponent<CharacterManager>(out var characterManager))
        {
            Debug.Log("BackStabInteracting");
            Weapon weapon = (Weapon)playerInventory.RightWeapon;
            if (weapon.IsUnarmed)
                return;
            transform.rotation = Quaternion.LookRotation((characterManager.transform.position - transform.position).normalized);
            string enemyStabbed;
            string attackerStabbed;
            if (Vector3.Dot(-characterManager.transform.forward, targetDirectionTo) < 0)
            {
                characterManager.transform.rotation = transform.rotation;
                transform.position = characterManager.BackStabPoint.position;
                enemyStabbed = "BackStabbed";
                attackerStabbed = "BackStabAttack";
            }
            else if (characterManager.IsParried)
            {
                characterManager.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 180, 0));
                transform.position = characterManager.ForwardStabPoint.position;
                enemyStabbed = "ForwardStabbed";
                attackerStabbed = "ForwardStabAttack";
            }
            else
                return;
            characterManager.GetComponent<CharacterStats>().CriticalDamage = (int)(weapon.Damage*criticalDamageMultiplier);
            animatorManager.PlayTargetAnimation(attackerStabbed, true,true);
            characterManager.GetComponent<CharacterAnimator>().PlayTargetAnimation(enemyStabbed, true);
        }
    }

    public void AttemptToParryAttack()
    {
        if (!animatorManager.IsStabbing && playerInventory.LeftWeapon.IsShield() && !playerManager.isInteracting && playerStats.CurrentStamina > 0)
        {
            animatorManager.animator.Play("Left Arm Empty", animatorManager.animator.GetLayerIndex("Left Arm"));
            animatorManager.PlayTargetAnimation("Parry", true,true);
            animatorManager.SetStabbing();
        }
    }
}

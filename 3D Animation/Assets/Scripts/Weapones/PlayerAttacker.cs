using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG;
using System.Linq;

public class PlayerAttacker : MonoBehaviour
{
    [SerializeField] private float distanceToBackStab;
    [SerializeField] private float angle;
    [SerializeField] private LayerMask layerDetection;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private Transform criticalStartPoint;
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
        playerManager = GetComponent<PlayerManager>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        weaponSlotManager = GetComponent<WeaponSlotManager>();
        inputManager = GetComponent<InputManager>();
        animatorManager = GetComponent<AnimatorManager>();
    }

    public void HandleWeaponCombo(Weapon weapon)
    {
        if (!inputManager.ComboFlag)
            return;
        animatorManager.animator.SetBool("CanDoCombo", false);
        animatorManager.PlayTargetAnimation(attacks[(++LastAttack)% attacks.Length], true, true);
        RotationToTarget();
    }

    public void HandleLightAttack(Weapon weapon)
    {
        weaponSlotManager.AttackingWeapon= weapon;
        IsHeavy = false;
        attacks = !inputManager.G || weapon.IsUnarmed ? weapon.LightsAttacks : weapon.TwoHandedAttacks;
        int index = Random.Range(0, attacks.Length);
        string randomAttack = attacks[index];
        LastAttack= index;
        RotationToTarget();
        animatorManager.PlayTargetAnimation(randomAttack, true,true);
    }

    public void HandleHeavyAttack(Weapon weapon)//by RootMotion use rotation in playerLocomotion
    {
        weaponSlotManager.AttackingWeapon = weapon;
        IsHeavy = true;
        attacks = !inputManager.G || weapon.IsUnarmed ? weapon.HeavyAttacks: weapon.TwoHandedAttacks;
        int index = Random.Range(0, attacks.Length);
        string randomAttack = attacks[index];
        LastAttack = index;
        RotationToTarget();
        animatorManager.PlayTargetAnimation(randomAttack, true,true);
    }

    public bool RotationToTarget()
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
            var rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            playerLocomotion.TargetRotation = rotation;
            return true;
        }
        return false;
    }

    public void AttemptToBackStabAttack()
    {
        if (RotationToTarget() && 
            Physics.Raycast(criticalStartPoint.position, targetDirectionTo, out var hit, distanceToBackStab, layerDetection) &&
            hit.collider.TryGetComponent<CharacterManager>(out var characterManager) &&
            Vector3.Dot(-characterManager.transform.forward, targetDirectionTo) < 0)
        {
            Debug.Log("BackStabInteracting");
            characterManager.transform.rotation = playerLocomotion.TargetRotation;
            transform.position = characterManager.BackStabPoint.position;
            animatorManager.PlayTargetAnimation("BackStabAttack",true,true);
            characterManager.GetComponent<EnemyAnimatorManager>().PlayTargetAnimation("BackStabbed", true);
        }
    }
}

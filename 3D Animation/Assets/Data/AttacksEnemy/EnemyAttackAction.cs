using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SG;

[CreateAssetMenu(menuName ="A.I/Enemy Actions/Attack Action")]
public class EnemyAttackAction : EnemyAction
{
    public int AttackScore = 3;
    public float RecoveryTime = 2;

    public float MaximumAttackAngle = 35;
    public float MinimumAttackAngle = -35;

    public float MinimumAttackDistance = 0;
    public float MaximumAttackDistance = 2;
}

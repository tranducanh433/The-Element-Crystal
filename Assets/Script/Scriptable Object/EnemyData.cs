using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyData : ScriptableObject
{
    public Sprite whiteShadow;
    public Sprite fireEnemy;
    public RuntimeAnimatorController fireEnemyAnim;
    public Sprite waterEnemy;
    public RuntimeAnimatorController waterEnemyAnim;
    public Sprite plantEnemy;
    public RuntimeAnimatorController plantEnemyAnim;
}

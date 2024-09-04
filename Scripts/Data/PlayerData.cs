using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Data", menuName = "Data/Player Data")]
public class PlayerData : ScriptableObject
{
    public int level;//玩家等级
    public float speed;
    public int damage;
    public float invincibilityTime;//无敌时间
    public int volumeToNextLevel;//进入下一等级所需的能量值

    [Header("攻击范围")]
    public List<AttackArea> attackAreaList = new List<AttackArea>();

    public AttackArea GetAttackArea(AttackType attackType)
    {
        return attackAreaList.Find(i => i.attackType == attackType);
    }
}

[System.Serializable]
public class AttackArea
{
    public AttackType attackType;
    public Vector2 areaOffset;
    public Vector2 areaSize;
}



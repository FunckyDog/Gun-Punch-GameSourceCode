using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Data", menuName = "Data/Player Data")]
public class PlayerData : ScriptableObject
{
    public int level;//��ҵȼ�
    public float speed;
    public int damage;
    public float invincibilityTime;//�޵�ʱ��
    public int volumeToNextLevel;//������һ�ȼ����������ֵ

    [Header("������Χ")]
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



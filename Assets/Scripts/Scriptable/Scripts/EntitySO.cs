using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Status {
    public string name;
    // 이름
    public Sprite main;
    // 이미지
    public float attack;
    // 공격력
    public float armor;
    // 방어도
    public float health;
    // 체력
}

[CreateAssetMenu(fileName ="EntitySO", menuName ="Scriptable Object/EntitySO")]
public class EntitySO : ScriptableObject
{
    public Status status;
}   
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Status {
    public string name;
    // 이름
    public Sprite main;
    // 이미지
    public int attack;
    // 공격력
    public int armor;
    // 방어도
    public int health;
    // 체력
}

[CreateAssetMenu(fileName ="EntitySO", menuName ="Scriptable Object/EntitySO")]
public class EntitySO : ScriptableObject
{
    public Status status;
}   
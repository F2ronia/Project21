using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[System.Serializable]
public class Item {
    public int num;             // 카드 넘버
    public string name;         // 능력 이름
    public int cost;            // 능력 비용
    public int value;     // 능력 수치
    public Sprite main;         // 메인 이미지
    public Sprite background;   // 배경 이미지
    public Sprite upper;        // 상단 이미지
    [TextArea]
    public string text;         // 능력 설명
    public enum Type { Normal, Warrior }
    public Type type;           // 카드 속성
    public enum Target { My, Enemy, All}
    public Target target;       // 카드 사용 방식
    public enum RestoreType { Armor, Health, Mana }
    public RestoreType restoreType; // 회복 방식
}

[CreateAssetMenu(fileName="ItemSO", menuName="Scriptable Object/ItemSO")]
public class ItemSO : ScriptableObject {
    public Item item;
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[System.Serializable]
public class Item {
    public string name;         // 능력 이름
    public int cost;            // 능력 비용
    public float value;           // 능력 수치
    public Sprite main;         // 메인 이미지
    public Sprite background;   // 배경 이미지
    public string text;         // 능력 설명
    public int count;           // 카드 장수
    public enum Type { Normal, Sun, Moon, eclipse }
    public Type type;           // 카드 속성
}

[CreateAssetMenu(fileName="ItemSO", menuName="Scriptable Object/ItemSO")]
public class ItemSO : ScriptableObject {
    public Item item;
}
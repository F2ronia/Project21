using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Card : MonoBehaviour {
    [SerializeField]
    private TMP_Text nameTMP;           // 이름
    [SerializeField]
    private TMP_Text textTMP;           // 설명
    [SerializeField]
    private TMP_Text costTMP;           // 코스트
    [SerializeField]
    private TMP_Text valueTMP;          // 수치   
    [SerializeField]
    private SpriteRenderer main;        // 메인 이미지
    [SerializeField]
    private SpriteRenderer background;  // 배경 이미지

    public Item Item;
    bool isFront;

    public void Setup(Item _Item, bool isFront) {
        this.Item = _Item;
        this.isFront = isFront;

        if (this.isFront) {
            nameTMP.text = this.Item.name;
            textTMP.text = this.Item.text;   
            costTMP.text = this.Item.cost.ToString();

            main.sprite = this.Item.main;
        }
    }
}
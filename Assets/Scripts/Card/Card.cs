using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Card : MonoBehaviour {
    [SerializeField]
    private TMP_Text nameTMP;           // 이름
    //[SerializeField]
    //private TMP_Text textTMP;           // 설명
    [SerializeField]
    private TMP_Text costTMP;           // 코스트
    //[SerializeField]
    //private TMP_Text valueTMP;          // 수치   
    [SerializeField]
    private SpriteRenderer main;        // 메인 이미지
    [SerializeField]
    private SpriteRenderer background;  // 배경 이미지

    public Item item;
    public PRS originPRS;

    public void Setup(Item _item) {
        this.item = _item;

        nameTMP.text = this.item.name;
        //textTMP.text = this.item.text;   
        costTMP.text = this.item.cost.ToString();
        main.sprite = this.item.main;
        background.sprite = this.item.background;
    }

    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0) {
        if (useDotween) {
            transform.DOMove(prs.pos, dotweenTime);
            transform.DORotateQuaternion(prs.rot, dotweenTime);
            transform.DOScale(prs.scale, dotweenTime);
        } else {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }

    private void OnMouseOver() {
        CardManager.Instance.CardMouseOver(this);
    }

    private void OnMouseExit() {
        CardManager.Instance.CardMouseExit(this);
    }
    private void OnMouseDown() {
        CardManager.Instance.CardMouseDown();
    }
    private void OnMouseUp() {
        CardManager.Instance.CardMouseUp();
    }
}
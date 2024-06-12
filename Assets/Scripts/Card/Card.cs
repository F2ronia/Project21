using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using Unity.VisualScripting.Dependencies.NCalc;

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
    [SerializeField]
    private SpriteRenderer upper;       // 상단 이미지

    public Item item;
    public PRS originPRS;

    public void Setup(Item _item) {
        this.item = _item;

        nameTMP.text = this.item.name;
        //textTMP.text = this.item.text;   
        costTMP.text = this.item.cost.ToString();
        main.sprite = this.item.main;
        background.sprite = this.item.background;
        upper.sprite = this.item.upper;
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
#region ActiveOverloading
    private void ActiveCard(Entity entity, int damage) {
    // 단일 공격
        entity.OnDamage(damage);
    }
    private void ActiveCard(Item.RestoreType type, int restore) {
    // 단일 회복 or 방어도 
        if (type == Item.RestoreType.Armor)
            PlayerStatus.Instance.RestoreArmor(restore);
        else if (type == Item.RestoreType.Health)
            PlayerStatus.Instance.RestoreHealth(restore);
        else if (type == Item.RestoreType.Mana)
            PlayerStatus.Instance.RestoreMana(restore);
    }
    private void ActiveCard(int damage) {
    // 광역 데미지
        for (int i=EntityManager.Instance.allEntity.Count-1; i>=0; i--) {
            ActiveCard(EntityManager.Instance.allEntity[i], damage);
        }
    }
    
    public void CallActive(Entity entity, int num) {
        switch (num) {
            case 1:
            // 기본 공격
            case 6:
            // 어썰트 슬래시
            // 단일 6 데미지
                ActiveCard(entity, item.value);
                break;    
            case 2:
            // 기본 방어
            case 3:
            // 기본 회복
                ActiveCard(item.restoreType, item.value);
                break;
            case 4:
            // 반격 태세
            // 방어도 4 획득, 광역 2 데미지
                ActiveCard(item.restoreType, item.value);
                ActiveCard(item.value / 2);
                break;
            case 5:
            // 응수
            // 방어도 만큼 피해 주기
                ActiveCard(entity, PlayerStatus.Instance.armor);
                break;
            case 7:
            // 회전 베기
            // 광역 3 데미지
                ActiveCard(item.value);
                break;
            case 8:
            // 찌르기
            // 단일 3 데미지, 방어도 3 획득
                ActiveCard(entity, item.value);
                ActiveCard(item.restoreType, item.value);
                break;
            case 9:
            // 자세 고쳐잡기
            // 가진 방어도 *2 획득
                ActiveCard(item.restoreType, PlayerStatus.Instance.armor * 2);
                break;
            case 10:
            // 십자 베기
            // 광역 5 데미지, 단일 5 데미지
                ActiveCard(entity, item.value);
                ActiveCard(item.value);
                break;
            case 11:
            // 용맹
            // 단일 5 데미지, 마나 1 회복
                ActiveCard(entity, item.value);
                ActiveCard(item.restoreType, 1);
                break;
            case 12:
            // 바리케이드
            // 방어도 4 획득, 방어도 수치만큼 회복
                ActiveCard(item.restoreType, item.value);
                ActiveCard(Item.RestoreType.Health, PlayerStatus.Instance.armor);
                break;
            case 13:
            // 초월
            // 광역 4 데미지, 입힌 피해의 절반 회복
                ActiveCard(item.value);
                int value = EntityManager.Instance.allEntity.Count-1 * 2;
                ActiveCard(item.restoreType, value);
                break;
        }
    }
#endregion
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
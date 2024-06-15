using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using DG.Tweening;

public class Entity : MonoBehaviour
{
#region Status
    public string _name;
    // 객체 이름
    public int startHealth;
    // 시작 체력
    public int health { get; protected set; }
    // 현재 체력
    public int attack { get; protected set; }
    // 공격력
    public int armor { get; protected set; }
    // 방어도
    public bool isDead { get; protected set; }
    // 사망 여부
    public bool isActived { get; set; }
    // 행동 여부 체크
#endregion
    public SpriteRenderer main;
    public Status status;
    public Vector3 originPos;
    public event Action OnDeath;
    // 사망 이벤트
#region SetupFunctino
    public void Setup(Status _status) {
        this.status = _status;

        main.sprite = _status.main;
        _name = _status.name;
        startHealth = health = _status.health;
        attack = _status.attack;
        armor = _status.armor;

    }
    public void MoveTransform(Vector3 pos, bool useDotween, float dotweenTime = 0) {
        if (useDotween) {
            transform.DOMove(pos, dotweenTime);
        } else {
            transform.position = pos;
        }
    }
#endregion
#region BattleFunction
    protected virtual void OnEnable() {
    // 생성 시 리셋
        isDead = false;
        health = startHealth;
    }
    
    public virtual void OnDamage(int damage) {
    // 데미지 입는 기능
        Sequence sequence = DOTween.Sequence()
            .Append(transform.DOShakePosition(1.3f))
            .AppendCallback(() =>
            {
                // 효과 처리
                // 데미지 연산
                if (armor > 0) {
                // 방어도가 존재하는 경우
                    if (armor > damage) {
                    // 방어도가 데미지보다 높은 경우
                        armor -= damage;
                    } else {
                    // 방어도와 데미지가 같거나 낮은 경우
                        int applyValue = damage-armor;
                        health -= applyValue;
                        armor -= damage;
                    }
                } else {
                // 방어도가 존재하지 않는 경우
                    health -= damage;
                }

                if (health <= 0 && !isDead)
                    Die();
            });
    }

    public virtual void RestoreHealth(int restorePoint) {
    // 회복 기능
        if (isDead)
            return;

        health += restorePoint;
    }

    public virtual void RestoreArmor(int restorePoint) {
    // 방어도 기능
        if (isDead)
            return;

        armor += restorePoint;
    }

    public virtual void Die() {
    // 사망 시 기능
        if (OnDeath != null)
        {
            OnDeath();
            Debug.Log("이벤트 진입");
        }
        Debug.Log("트리거 체크");
        isDead = true;
    }
#endregion
}
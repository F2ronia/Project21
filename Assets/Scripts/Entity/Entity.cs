using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using DG.Tweening;

public class Entity : MonoBehaviour
{
    public string _name;
    // 객체 이름
    public float startHealth;
    // 시작 체력
    public float health { get; protected set; }
    // 현재 체력
    public float attack { get; protected set; }
    // 공격력
    public float armor { get; protected set; }
    // 방어도
    public bool isDead { get; protected set; }
    // 사망 여부
    public bool isMyTurn { get; protected set; }
    // 자기 턴 체크
    public Status status;
    public Vector3 originPos;
    public event Action OnDeath;
    // 사망 이벤트
#region SetupFunctino
    public void Setup(Status _status) {
        this.status = _status;

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
        health -= damage;

        if (health <= 0 && !isDead)
            Die();
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
        }

        isDead = true;
    }
#endregion
}
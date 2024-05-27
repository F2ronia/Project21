using System;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public int startHealth = 50;
    // 시작 체력
    public int health { get; protected set; }
    // 현재 체력
    public int armor { get; protected set; }
    // 현재 방어도
    public bool isDead { get; protected set; }
    // 사망 여부
    public bool isMyTurn { get; protected set; }
    // 자기 턴 체크
    public event Action OnDeath;
    // 사망 이벤트

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
}
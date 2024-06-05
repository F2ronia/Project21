using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Active {
    Attack,     // 공격
    Defence,    // 방어 
    Enforce,    // 강화 공격
    Special     // 특수 패턴
}

public class Enemy : Entity {
    private Active active;
    public Active Action {
        get {
            return active;
        }
    }

    void Start() {
        TurnManager.OnTurnStarted += OnTurnStarted;
        TurnManager.OnReadyAction += ReadyAction;
    }

    void OnDestroy() {
        TurnManager.OnTurnStarted -= OnTurnStarted;
    }

    private void OnTurnStarted(bool myTurn) {
        if (!myTurn)
            StartCoroutine(EnemyAI());
    }

    private IEnumerator EnemyAI() {
        Debug.Log("적 행동");
        EnemyAction();
        isActived = true;
        yield return Utils.D1;
        if (EntityManager.Instance.EnemyTurnEnd()) {
            EntityManager.Instance.ResetEnemyIsActive();
            TurnManager.Instance.EndTurn();
        }
    }

    private void ReadyAction(bool myTurn) {
        if (myTurn) {
            active = (Active)Random.Range(0, 4);
            // 공격 패턴 랜덤 설정 
            // 공격 or 방어만 테스트
            Debug.Log(active);
        }
    }

    private void EnemyAction() {
        switch (active) {
        // 특수 패턴은 나중에 추가
            case Active.Enforce:
            case Active.Special:
            case Active.Attack:
                PlayerStatus.Instance.OnDamage(status.attack);
                break;
            case Active.Defence:
                Debug.Log("방어도 획득 : " + status.armor);
                RestoreArmor(status.armor);
                Debug.Log("현재 방어도 : " + armor);
                break;
        }
    }

    private void OnMouseOver() {
        EntityManager.Instance.EntityMouseOver(this);
    }
    private void OnMouseExit() {
        EntityManager.Instance.EntityMouseExit(this);
    }
    private void OnMouseUp() {
        EntityManager.Instance.EntityMouseUp();
    }
}
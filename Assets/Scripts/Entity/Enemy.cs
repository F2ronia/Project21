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
    void Start() {
        TurnManager.OnTurnStarted += OnTurnStarted;
    }

    void OnDestroy() {
        TurnManager.OnTurnStarted -= OnTurnStarted;
    }

    private void OnTurnStarted(bool myTurn) {
        if (!myTurn)
            StartCoroutine(EnemyAI());
    }

    private IEnumerator EnemyAI() {

        Debug.Log("에너미 테스트");
        isActived = true;
        yield return Utils.D1;
        if (EntityManager.Instance.EnemyTurnEnd()) {
            EntityManager.Instance.ResetEnemyIsActive();
            TurnManager.Instance.EndTurn();
        }
    }
}
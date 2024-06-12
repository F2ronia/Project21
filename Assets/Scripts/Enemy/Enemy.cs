using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

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
        OnDeath += EnemyDie;
    }

    void OnDestroy() {
        TurnManager.OnTurnStarted -= OnTurnStarted;
        TurnManager.OnReadyAction -= ReadyAction;
        OnDeath -= EnemyDie;
    }

    private void OnTurnStarted(bool myTurn) {
        if (!myTurn)
            StartCoroutine(EnemyAI());
    }

    private IEnumerator EnemyAI() {
        Debug.Log("적 행동");
        yield return Utils.D05;
        EnemyAction();
        isActived = true;
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
                RestoreArmor(status.armor);
                break;
        }
    }

    private void EnemyDie() {
        Debug.Log("죽음 처리 확인");
        DestoryEnemy();
    }

    private void DestoryEnemy() {
    // 죽음 처리 + 애니메이션
        Sequence sequence = DOTween.Sequence()
            .Append(transform.DOShakePosition(1.3f))
            .Append(transform.DOScale(Utils.VZ, 0.3f)).SetEase(Ease.OutCirc)
            .OnComplete(() =>
            {
                for (int i=EntityManager.Instance.allEntity.Count-1; i>=0 ; i--) {
                // 연결된 UI 객체 찾기
                    var temp = EntityManager.Instance.allEntityUi[i].enemy;
                    var tempUI = EntityManager.Instance.allEntityUi[i];

                    if (temp == this) {
                        Destroy(tempUI.gameObject);
                        EntityManager.Instance.allEntityUi.Remove(tempUI);
                    }
                }
                Destroy(gameObject);
                EntityManager.Instance.allEntity.Remove(this);

                EntityManager.Instance.HideTargetPicker();
                // 타겟 지정된 상태 대비하여 숨김 처리
                Debug.Log("적 수 : " + EntityManager.Instance.allEntity.Count);
                if (EntityManager.Instance.allEntity.Count <= 0)
                    Debug.Log("적 전체 죽음");
            });
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
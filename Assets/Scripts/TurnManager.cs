using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class TurnManager : MonoBehaviour
{
#region Singleton
    public static TurnManager Instance {get; private set;}
    void Awake() => Instance = this;
#endregion
#region Variables
    [Header("Develop")]
    [Tooltip("시작 턴 모드 설정")]
    [SerializeField]
    ETurnMode eTurnMode;
    [Tooltip("시작 카드 개수 설정")]
    [SerializeField]
    int startCardCount;

    [Header("Properties")]
    public bool myTurn;
    public bool isLoading;
    private WaitForSeconds delay = new WaitForSeconds(0.5f);

    public static Action OnAddCard;
    public static event Action<bool> OnTurnStarted;

    private enum ETurnMode { Random, My, Ohter }
#endregion

    void BattleSetup() {
        switch (eTurnMode) {
            case ETurnMode.Random:
                myTurn = Random.Range(0, 2) == 0;
                break;
            case ETurnMode.My:
                myTurn = true;
                break;
            case ETurnMode.Ohter:
                myTurn = false;
                break;
        }
    }

    public IEnumerator StartBattle() {
        BattleSetup();
        isLoading = true;

        for (int i=0; i<startCardCount; i++) {
            yield return delay;
            OnAddCard?.Invoke();
        }

        StartCoroutine(StartTurn());
    }

    private IEnumerator StartTurn() {
        isLoading = true;
        GameManager.Instance.Notification("나의 턴");
        yield return delay;
        OnAddCard?.Invoke();
        isLoading = false;
    }

    public void EndTurn() {
        myTurn = !myTurn;
        StartCoroutine(StartTurn());
    }
}
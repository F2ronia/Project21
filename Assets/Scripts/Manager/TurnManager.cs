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
    public static Action<bool> OnReadyAction;
    public static Action<bool> OnAddCard;
    public static event Action<bool> OnTurnStarted;
    public static event Action<bool> OnTurnEnd;

    private enum ETurnMode { Random, My, Ohter }
#endregion

    void BattleSetup() {
        /*
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
        */
        myTurn = true;
    }

    public IEnumerator StartBattle() {
        BattleSetup();
        isLoading = true;
        yield return delay;
        StartCoroutine(StartTurn());
    }

    private IEnumerator StartTurn() {
        isLoading = true;
        yield return delay;
        for (int i=0; i<startCardCount; i++) {
            yield return delay;
            OnAddCard?.Invoke(myTurn);
        }
        isLoading = false;
        OnTurnStarted?.Invoke(myTurn);
        OnReadyAction.Invoke(myTurn);
    }

    public void EndTurn() {
        myTurn = !myTurn;
        OnTurnEnd?.Invoke(true);
        StartCoroutine(StartTurn());
    }
}
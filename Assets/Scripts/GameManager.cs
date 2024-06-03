using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
#region Singleton
    
    public static GameManager Instance { get; private set; }
    void Awake() {
        if (Instance != this && Instance != null) {
            Destroy(gameObject);
            return;
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
#endregion

    [SerializeField]
    NoticePannel noticePannel;

    void Start() {
        StartCoroutine(TurnManager.Instance.StartBattle());
    }

    void Update() {
        #if UNITY_EDITOR
            InputCheatKey();
        #endif
    }

    private void InputCheatKey() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            TurnManager.OnAddCard?.Invoke(true);
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            TurnManager.Instance.EndTurn();
        }
        if (Input.GetKeyDown(KeyCode.F)) {
            EnemySpawn();
        }
        if (Input.GetKeyDown(KeyCode.T)) {
            EntityManager.Instance.RemoveEmptyEntity();
        }
    }

    public void Notification(string msg) {
        noticePannel.Show(msg);
    }

    private void EnemySpawn() {
        EntityManager.Instance.SpawnEntity();
        EntityManager.Instance.InsertEmptyEntity(Utils.MousePos.x);
    }
}
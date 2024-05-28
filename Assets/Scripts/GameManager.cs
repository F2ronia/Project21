using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
#region Singleton
    public static GameManager Instance {get; private set;}
    void Awake() => Instance = this;
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
            TurnManager.OnAddCard?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            TurnManager.Instance.EndTurn();
        }
    }

    public void Notification(string msg) {
        noticePannel.Show(msg);
    }
}
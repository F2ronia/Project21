using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Active {
    Attack,     // 공격
    Defence,    // 방어도 
    Restore    // 회복
}

public class Enemy : Entity {
    [SerializeField]
    private int attack = 2;
    private Active action;

    void Start() {
        isMyTurn = true;
    }

    void Update() {
        if (Input.GetMouseButtonDown(0))
            isMyTurn = true;

        if (isMyTurn) {
            switch(Random.Range(0, 3)) {
                case 0:
                    action = Active.Attack;
                    break;
                case 1:
                    action = Active.Defence;
                    break;
                case 2:
                    action = Active.Restore;
                    break;
            }
            Debug.Log(action);
            isMyTurn = false;
        }
    }
}
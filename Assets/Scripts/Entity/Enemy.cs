using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum Active {
    Attack,     // 공격
    Defence,    // 방어 
    Enforce,    // 강화 공격
    Special     // 특수 패턴
}

public class Enemy : Entity {
    // Enemy 스크립터블 참조 예정
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
                    action = Active.Enforce;
                    break;
            }
            //Debug.Log(action);
            isMyTurn = false;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : Entity
{
#region Singleton
    public static PlayerStatus Instance {get; private set;}
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
    private void Start() {
        health = 20;
        Debug.Log("체력 : " + health);
    }

    public override void RestoreArmor(int restorePoint)
    {
        base.RestoreArmor(restorePoint);
        Debug.Log("현재 방어도 : " + armor);
    }

    public override void RestoreHealth(int restorePoint)
    {
        base.RestoreHealth(restorePoint);
        Debug.Log("현재 체력 : " + health);
    }

    public void RestoreMana(int restorePoint) {
        return;
    }
}

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
    private int playerMana;

    public int PlayerMana {
        get {
            return playerMana;
        } set {
            playerMana = value;
        }
    }
    private void Start() {
        startHealth = health = 20;
        Debug.Log("체력 : " + health);

        TurnManager.OnTurnStarted += RestoreMana;
    }

    private void OnDestroy() {
        TurnManager.OnTurnStarted -= RestoreMana;
    }

    public override void RestoreArmor(int restorePoint)
    {
        base.RestoreArmor(restorePoint);
        Debug.Log("현재 방어도 : " + armor);
    }

    public override void RestoreHealth(int restorePoint)
    {
        base.RestoreHealth(restorePoint);
        if (startHealth < health)
            health = startHealth;
        Debug.Log("현재 체력 : " + health);
    }

    public void RestoreMana(int restorePoint) {
        PlayerMana += restorePoint;
    }

    public void RestoreMana(bool myTurn) {
        if (myTurn) {
            PlayerMana = Utils.MaxMana;
            Debug.Log("현재 마나 : " + PlayerMana);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : Entity
{
#region Singleton
    public static PlayerStatus Instance {get; private set;}
    void Awake() => Instance = this;
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

        TurnManager.OnTurnStarted += RestoreMana;
    }

    private void OnDestroy() {
        TurnManager.OnTurnStarted -= RestoreMana;
    }

    public override void RestoreArmor(int restorePoint)
    {
        base.RestoreArmor(restorePoint);
    }

    public override void RestoreHealth(int restorePoint)
    {
        base.RestoreHealth(restorePoint);
        if (startHealth < health)
            health = startHealth;
    }

    public void RestoreMana(int restorePoint) {
        PlayerMana += restorePoint;
    }

    public void RestoreMana(bool myTurn) {
        if (myTurn) {
            PlayerMana = Utils.MaxMana;
        }
    }

    public override void Die()
    {
        Debug.Log("플레이어 사망");
        base.Die();    
    }
}

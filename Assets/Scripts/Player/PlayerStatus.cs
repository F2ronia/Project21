using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerStatus : Entity
{
#region Singleton
    public static PlayerStatus Instance {get; private set;}
    void Awake() => Instance = this;
#endregion
    private int playerMana;
    private bool isDanger = false;
    private AudioSource audioSource;

    public int PlayerMana {
        get {
            return playerMana;
        } set {
            playerMana = value;
        }
    }
    private void Start() {
        startHealth = health = 20;
        audioSource = GetComponent<AudioSource>();
        TurnManager.OnTurnStarted += RestoreMana;
    }
    private void Update() {
        if (health > 10)
            isDanger = false;
        else
            isDanger = true;

        GameManager.Instance.ChangeBattleBGM(isDanger);
    }

    private void OnDestroy() {
        TurnManager.OnTurnStarted -= RestoreMana;
    }

    public override void OnDamage(int damage)
    {
        base.OnDamage(damage);

        audioSource.Stop();
        audioSource.PlayOneShot(audioSource.clip);         
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
        GameManager.Instance.CallAnyScene("temp_main");
        //base.Die();    
    }
}

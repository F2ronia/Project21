using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using DG.Tweening;
using UnityEngine;

public enum SoundPlayer {
    Player_Armor,
    Player_Attack,
    Player_Heal,
    Player_Hit,
    Player_HitArmor,
    Player_Lose,
    Player_Win
}

public class PlayerStatus : Entity
{
#region Singleton
    public static PlayerStatus Instance {get; private set;}
    void Awake() => Instance = this;
#endregion
    private int playerMana;
    private bool isDanger = false;
    public AudioSource audio;
    [SerializeField]
    private AudioClip[] audioClips;

    public int PlayerMana {
        get {
            return playerMana;
        } set {
            playerMana = value;
        }
    }
    private void Start() {
        startHealth = health = 20;
        audio = GetComponent<AudioSource>();
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

    public void PlayerAttack() {
        audio.PlayOneShot(audioClips[(int)SoundPlayer.Player_Attack], Utils.SOUND5F);
    }
    public void PlayerWin() {
        audio.PlayOneShot(audioClips[(int)SoundPlayer.Player_Win], Utils.SOUNDMAX);
    }

    public override void OnDamage(int damage)
    {
        // 데미지 연산
        if (armor > 0) {
        // 방어도가 존재하는 경우
            if (armor > damage) {
            // 방어도가 데미지보다 높은 경우
                armor -= damage;
            } else {
            // 방어도와 데미지가 같거나 낮은 경우
                int applyValue = damage-armor;
                health -= applyValue;
                armor -= damage;
            }
            if (!audio.isPlaying)
                audio.PlayOneShot(audioClips[(int)SoundPlayer.Player_HitArmor], Utils.SOUNDMAX);
        } else {
        // 방어도가 존재하지 않는 경우
            health -= damage;
            if (!audio.isPlaying)
                audio.PlayOneShot(audioClips[(int)SoundPlayer.Player_Hit], Utils.SOUNDMAX);
        }

        if (health <= 0 && !isDead)
            Die();    
    }

    public override void RestoreArmor(int restorePoint)
    {
        base.RestoreArmor(restorePoint);
        audio.PlayOneShot(audioClips[(int)SoundPlayer.Player_Armor], Utils.SOUNDMAX);
    }

    public override void RestoreHealth(int restorePoint)
    {
        base.RestoreHealth(restorePoint);
        audio.PlayOneShot(audioClips[(int)SoundPlayer.Player_Heal], Utils.SOUNDMAX);
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
        audio.PlayOneShot(audioClips[(int)SoundPlayer.Player_Lose], Utils.SOUNDMAX);
        CardManager.Instance.RemoveAllMyCards();
        GameManager.Instance.CallAnyScene("temp_main");
        //base.Die();    
    }
}

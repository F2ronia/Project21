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
    [SerializeField]
    private Transform hitPoint;
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
        health = startHealth;
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

        // 파티클 추가 및 화면 흔들림 구현
        ParticleManager.Instance.SpawnParticle(Utils.HIT, hitPoint);
        Sequence sequence = DOTween.Sequence()
            .Append(Utils.MainCam.transform.DOShakePosition(.5f))
            .OnComplete(() =>
                Utils.MainCam.transform.position = Utils.MainCamLocalPos
            );

        // 데미지 텍스트 표기
        BattleUIManager.Instance.SetValueText(damage, Utils.ATTACK);

        if (health <= 0 && !isDead)
            Die();    
    }

    public override void RestoreArmor(int restorePoint)
    {
        base.RestoreArmor(restorePoint);
        audio.PlayOneShot(audioClips[(int)SoundPlayer.Player_Armor], Utils.SOUNDMAX);
        // 데미지 텍스트 표기
        BattleUIManager.Instance.SetValueText(restorePoint, Utils.ARMOR);
    }

    public override void RestoreHealth(int restorePoint)
    {
        int point = restorePoint;

        base.RestoreHealth(restorePoint);
        audio.PlayOneShot(audioClips[(int)SoundPlayer.Player_Heal], Utils.SOUNDMAX);
        if (startHealth < health) {
            health = startHealth;
            point = 0;
        } else if (health > 17) {
            point = startHealth-health;
        }
        // 데미지 텍스트 표기
        BattleUIManager.Instance.SetValueText(point, Utils.HEAL);

    }

    public void RestoreMana(int restorePoint) {
        PlayerMana += restorePoint;
        // 데미지 텍스트 표기
        BattleUIManager.Instance.SetValueText(restorePoint, Utils.MANA);
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
        BattleResultUI.Instance.CallLoseUI();
    }
}

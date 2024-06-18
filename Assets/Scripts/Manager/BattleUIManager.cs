using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BattleUIManager : MonoBehaviour
{
#region Singleton
    public static BattleUIManager Instance {get; private set;}
    void Awake() => Instance = this;
#endregion
    private bool isActive;
    public Image[] mp_img;
    public Slider hp_slider;
    public TMP_Text hp_text;
    public GameObject armor;
    public TMP_Text count_text;
    public GameObject value_text;

    void Update() {
        for (int i=0; i < 3; i++) {
            if (i+1 <= PlayerStatus.Instance.PlayerMana) { 
                isActive = true;
            } else {
                isActive = false;
            }   
            SetMpImg(mp_img[i], isActive);
        }

        hp_slider.value = (float)PlayerStatus.Instance.health/PlayerStatus.Instance.startHealth*100;
        hp_text.text = PlayerStatus.Instance.health.ToString();

        if (PlayerStatus.Instance.armor > 0) {
            SetArmorImg(true);
        } else {
            SetArmorImg(false);
        }

        count_text.text = CardManager.Instance.MyCards.ToString();
    }

    private void SetMpImg(Image img, bool isActive) {
        img.color = isActive ? Utils.ColorOrigin : Utils.ColorDisable;
    }

    private void SetArmorImg(bool isActive) {
        armor.SetActive(isActive);
        armor.GetComponentInChildren<TMP_Text>().text = PlayerStatus.Instance.armor.ToString();
    }

    public void SetValueText(int value, int type) {
        var text = value_text.GetComponent<TMP_Text>();
        text.text = value.ToString();

        switch (type) {
            case Utils.ATTACK:
                text.color = Color.red;
                break;
            case Utils.ARMOR:
                text.color = Color.gray;
                break;
            case Utils.HEAL:
                text.color = Color.green;
                break;
            case Utils.MANA:
                text.color = Color.blue;
                break;
        }

        Sequence sequence = DOTween.Sequence()
            .Append(value_text.transform.DOScale(new Vector3(2,2,2), 1f).SetEase(Ease.OutCirc))
            .Append(value_text.transform.DOScale(Utils.VZ, 0.5f).SetEase(Ease.OutCirc));
    }
}
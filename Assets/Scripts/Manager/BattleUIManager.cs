using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleUIManager : MonoBehaviour
{
    private bool isActive;
    public Image[] mp_img;
    public Slider hp_slider;
    public TMP_Text hp_text;
    public GameObject armor;

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
    }

    private void SetMpImg(Image img, bool isActive) {
        img.color = isActive ? Utils.ColorOrigin : Utils.ColorDisable;
    }

    private void SetArmorImg(bool isActive) {
        armor.SetActive(isActive);
        armor.GetComponentInChildren<TMP_Text>().text = PlayerStatus.Instance.armor.ToString();
    }
}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleResultUI : MonoBehaviour
{
#region Singleton
    public static BattleResultUI Instance {get; private set;}
    void Awake() => Instance = this;
#endregion
    public Image fade;
    public GameObject winUI;
    public GameObject loseUI;

    public void CallLoseUI() {
        fade.gameObject.SetActive(true);
        loseUI.SetActive(true);
        StartCoroutine(StartFadeOut(1f));
    }

    public void CallWinUI() {
        fade.gameObject.SetActive(true);
        winUI.SetActive(true);
        StartCoroutine(StartFadeOut(1f));
    }

    public void SelectText(string msg) {
        winUI.GetComponentInChildren<TMP_Text>().text = msg;
    }

    IEnumerator StartFadeOut(float _fadeTime)
    {
        Color alhpa = fade.color;
        alhpa.a = 0f;
        while (fade.color.a < 1)
        {
            alhpa.a += Time.deltaTime * _fadeTime;
            fade.color = alhpa;
            yield return null;
        }
    }
}
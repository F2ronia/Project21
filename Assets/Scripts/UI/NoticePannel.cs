using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class NoticePannel : MonoBehaviour
{
#region Singleton
    public static NoticePannel Instance {get; private set;}
    void Awake() {
        if (Instance != this && Instance != null) {
            return;
        } else {
            Instance = this;
        }
    }
#endregion
    private TMP_Text txt;

    public void Show(string msg) {
        txt.text = msg;
        Sequence sequence = DOTween.Sequence()
            .Append(transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.InOutQuad))
            .AppendInterval(0.9f)
            .Append(transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutQuad));
    }

    void Start() {
        txt = GetComponentInChildren<TMP_Text>();
        transform.localScale = Utils.VZ;
    }
}
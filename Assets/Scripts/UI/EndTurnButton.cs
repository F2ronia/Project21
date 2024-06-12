using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndTurnButton : MonoBehaviour
{
    [SerializeField]
    private Sprite active;
    [SerializeField]
    private Sprite inactive;
    [SerializeField]
    private TMP_Text btnText;

    [SerializeField]
    private Image image;

    void Start() {
        Setup(true);
        TurnManager.OnTurnStarted += Setup;
    }

    void OnDestroy() {
        TurnManager.OnTurnStarted -= Setup;
    }

    public void Setup(bool isActive) {
        image.color =  isActive ? new Color32(255,255,255,255) : new Color32(255,255,255, 100);
    }
}
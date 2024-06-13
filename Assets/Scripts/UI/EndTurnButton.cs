using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndTurnButton : MonoBehaviour
{
    [SerializeField]
    private Image image;
    private Button button;

    void Start() {
        Setup(false);
        button = GetComponent<Button>();
        TurnManager.OnTurnStarted += Setup;
    }

    void OnDestroy() {
        TurnManager.OnTurnStarted -= Setup;
    }

    public void Setup(bool isActive) {
        GetComponent<Button>().interactable = isActive;
        image.color =  isActive ? new Color32(255,255,255,255) : new Color32(255,255,255, 100);
    }
}
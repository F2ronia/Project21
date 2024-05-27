using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
#region Singleton
    public static UIManager Inst { get; private set; }
    void Awake() => Inst = this;
#endregion
    private TMP_Text stageText;

    void Start() {
        stageText = GetComponentInChildren<TMP_Text>();
    }

    public void SelectStage(string name) {
        stageText.text = name; 
    }
}
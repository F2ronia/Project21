using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EnemyUI : MonoBehaviour
{
    private Camera uiCam;
    private Canvas uiCan;
    private RectTransform rectParent;
    private RectTransform rectHp;

    [HideInInspector]
    public Vector3 offset = Utils.VZ;
    public Transform targetTr;
    public TMP_Text hpValue;
    public Slider slider;
    private int maxHp;
    private int initHp;
    public Enemy enemy;
    void Start() {
        uiCan = GetComponentInParent<Canvas>();
        uiCam = uiCan.worldCamera;

        rectParent = uiCan.GetComponent<RectTransform>();
        rectHp = this.gameObject.GetComponent<RectTransform>();

        maxHp = initHp = enemy.startHealth;
    }

    void Update() {
        slider.value = (float)initHp/maxHp*100;
        initHp = enemy.health;
        hpValue.text = initHp.ToString();
    }

    void LateUpdate() {
        var screenPos = Camera.main.WorldToScreenPoint(targetTr.position + offset);

        if (screenPos.z < 0.0f) {
            screenPos *= -1.0f;
        }

        var localPos = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectParent, screenPos,
            uiCam, out localPos);

        rectHp.localPosition = localPos;
    }
}
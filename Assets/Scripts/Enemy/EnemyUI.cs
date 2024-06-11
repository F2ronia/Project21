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
    public Image actionImg;
    public Sprite[] sprites;
    public GameObject armor;
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

        if (enemy.armor > 0) {
            SetArmorImg(true);
        } else {
            SetArmorImg(false);
        }
        ActionImageSet();
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

    private void ActionImageSet() {
        switch (enemy.Action) {
            case Active.Special:    // 특수 패턴
            case Active.Attack: // 일반 공격
                actionImg.sprite = sprites[0];
                break;
            case Active.Enforce:    // 강화 패턴
            case Active.Defence:    // 일반 방어
                actionImg.sprite = sprites[1];
                break;
        }   
    }
    private void SetArmorImg(bool isActive) {
        armor.SetActive(isActive);
        armor.GetComponentInChildren<TMP_Text>().text = enemy.armor.ToString();
    }
}
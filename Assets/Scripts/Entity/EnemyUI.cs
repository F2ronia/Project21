using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUI : MonoBehaviour
{
    private Camera uiCam;
    private Canvas uiCan;
    private RectTransform rectParent;
    private RectTransform rectHp;

    [HideInInspector]
    public Vector3 offset = Utils.VZ;
    [HideInInspector]
    public Transform targetTr;

    void Start() {
        uiCan = GetComponentInParent<Canvas>();
        uiCam = uiCan.worldCamera;

        rectParent = uiCan.GetComponent<RectTransform>();
        rectHp = this.gameObject.GetComponent<RectTransform>();
    }

    void Update() {

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
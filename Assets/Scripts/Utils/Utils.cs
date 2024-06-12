using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public class PRS {
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 scale;

    public PRS (Vector3 pos, Quaternion rot, Vector3 scale) {
        this.pos = pos;
        this.rot = rot;
        this.scale = scale;
    }
}

public class Utils
{
    public static Vector3 VZ => Vector3.zero;
    public static Quaternion QI => Quaternion.identity;
    public static Vector3 SO => Vector3.one;
    public static WaitForSeconds D1 = new WaitForSeconds(1);
    public static WaitForSeconds D05 = new WaitForSeconds(.5f);
    public static int MaxMana = 3;
    public static Color32 ColorOrigin = new Color32(255, 255, 255, 255);
    public static Color32 ColorDisable = new Color32(100, 100, 100, 255);

    public static Vector3 MousePos
    {
        get {
            Vector3 result = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            result.z = -10;
            return result;
        }
    }

    public static Transform MyCardLeft {
        get {
            Transform tr = GameObject.Find("CardLeft").transform;
            return tr;
        } set {

        }
    }

    public static Transform MyCardRight {
        get {
            Transform tr = GameObject.Find("CardRight").transform;
            return tr;
        }
    }
}
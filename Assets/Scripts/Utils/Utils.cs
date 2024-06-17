using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;

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
    public static Vector2 SelectLayout = new Vector2(500f, 500f);
    public static Vector2 ListLayout = new Vector2(200f, 200f);
    public static Vector3 MainCamLocalPos = new Vector3(0, 0, -100);
    public static float SOUND1F = 0.1f;
    public static float SOUND2F = 0.2f;
    public static float SOUND3F = 0.3f;
    public static float SOUND4F = 0.4f;
    public static float SOUND5F = 0.5f;
    public static float SOUND7F = 0.7f;
    public static float SOUNDMAX = 1f;
    public const int SINGLE = 0;
    public const int MULTI = 1;
    public const int HIT = 2;

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
        }
    }

    public static Transform MyCardRight {
        get {
            Transform tr = GameObject.Find("CardRight").transform;
            return tr;
        }
    }

    public static GameObject MainCam {
        get {
            GameObject cm = GameObject.Find("Main Camera");
            return cm;
        }
    }
}
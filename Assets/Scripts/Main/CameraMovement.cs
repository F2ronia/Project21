using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraMovement : MonoBehaviour
{
    public Transform imageF;
    public Transform imageS;
    Transform transform;
    private int camSet = 0;

    private Transform OriTransform;
    private void Start()
    {
        transform = GetComponent<Transform>();
        OriTransform = transform;
    }
    void FixedUpdate()
    {
        switch (camSet)
        {
            case 1:
                transform.LookAt(imageF);
                break;
            case 2:
                transform.LookAt(imageS);
                break;
        }
    }

    public void CutSceneSet(int _setCode, Vector3 _moveVec)
    {
        transform = OriTransform;
        camSet = _setCode;
        transform.DOMove(_moveVec, 10f);
    }
}

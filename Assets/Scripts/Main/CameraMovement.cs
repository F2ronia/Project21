using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class CameraMovement : MonoBehaviour
{
    public Transform image;
    public Transform imageF;
    public Transform imageS;
    Transform transform;
    private int camSet = 0;

    private Transform OriTransform;
    private void Start()
    {
        transform = GetComponent<Transform>();
        OriTransform = transform;
        transform.position = new Vector3 (transform.position.x - 1f, transform.position.y, transform.position.z);
        transform.DOMove(new Vector3(1f, transform.position.y, transform.position.z), 10f).SetLoops(-1, LoopType.Yoyo);
    }
    void FixedUpdate()
    {
        switch (camSet)
        {
            case 0:
                transform.LookAt(image);
                break;
            case 1:
                transform.LookAt(imageF);
                break;
            case 2:
                transform.LookAt(imageS);
                break;
        }
    }

    public void CutSceneSet(int _setCode, Vector3 _moveVec, float _moveTime)
    {
        transform = OriTransform;
        camSet = _setCode;
        transform.DOMove(_moveVec, _moveTime);
    }
}

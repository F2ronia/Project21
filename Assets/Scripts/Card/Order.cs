using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour
{
    [SerializeField]
    Renderer[] backRednerers;
    [SerializeField]
    Renderer[] middleRednerers;
    [SerializeField]
    string sortingLayerName;
    int originOrder;

    public void SetOriginOrder(int originOrder) {
        this.originOrder = originOrder;
        SetOrder(originOrder);
    }

    public void SetMostFrontOrder(bool isMostFront)
    {
        SetOrder(isMostFront ? 100 : originOrder);
    }

    public void SetOrder(int order) {
        int mulOrder = order * 10;

        foreach (var renderer in backRednerers) {
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = mulOrder;
        }

        foreach (var renderer in middleRednerers) {
            renderer.sortingLayerName = sortingLayerName;
            renderer.sortingOrder = ++mulOrder;
        }
    }
}
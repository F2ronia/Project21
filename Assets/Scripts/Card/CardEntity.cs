using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class CardEntity : MonoBehaviour
{
    public Vector3 originPos;
    private Item item;

    public void Setup(Item item)
    {
        this.item = item;
    }
}
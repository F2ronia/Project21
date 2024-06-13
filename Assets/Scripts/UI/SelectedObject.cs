using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedObject : MonoBehaviour
{
    private Card card;

    void Start() {
        card = GetComponent<Card>();
    }

    public void ReturnSelected() {
        CardManager.Instance.GetSelectCard(card.item);
    }

    public void RemoveSelected() {
        CardManager.Instance.GetRemoveSelect(card.item);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooling : MonoBehaviour
{
#region Singleton
    public static ObjectPooling Instance { get; private set; }
    void Awake() {
        if (Instance != this && Instance != null) {
            Destroy(gameObject);
            return;
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
#endregion
    [SerializeField]
    private GameObject poolingObject;
    [SerializeField]
    private Transform cardSpawnPoint;
    private Queue<Card> poolingObjectQueue = new Queue<Card>();

    void Start() {
        Initialize(8);
    }

    private Card CreateNewObejct() {
        var cardObject = Instantiate(poolingObject, cardSpawnPoint.position, Utils.QI);
        cardObject.SetActive(false);
        var card = cardObject.GetComponent<Card>();
        return card;
    }

    private void Initialize(int count) {
        for (int i=0; i<count; i++) {
            poolingObjectQueue.Enqueue(CreateNewObejct());
        }
    }

    public static Card GetObject() {
        if (Instance.poolingObjectQueue.Count > 0) {
            var obj = Instance.poolingObjectQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        } else {
            var newObj = Instance.CreateNewObejct();
            newObj.transform.SetParent(null);
            newObj.gameObject.SetActive(true);
            return newObj;
        }
    }

    public static void ReturnObject(Card card) {
        card.gameObject.SetActive(false);
        card.transform.SetParent(Instance.transform);
        card.transform.localPosition = Utils.VZ;
        card.transform.rotation = Utils.QI;
        card.transform.localScale = Utils.SO;
        Instance.poolingObjectQueue.Enqueue(card);
    }

    public static void ReturnAllObject(List<Card> cards) {
        for (int i=cards.Count-1; i>=0; i--) {
            ReturnObject(cards[i]);
        }
    }
}

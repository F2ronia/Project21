using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour {
#region Singleton
    public static CardManager Instance {get; private set;}
    void Awake() => Instance = this;
#endregion
#region Variables
    [SerializeField]
	private GameObject cardPrefab;
    public ItemSO[] itemSO;
    private List<Item> listBuffer;
    private List<Card> myCards;
    [SerializeField]
    private Transform cardSpawnPoint;
    [SerializeField]
    private Transform myCardLeft;
    [SerializeField]
    private Transform myCardRight;
#endregion
#region UnityEventFunction
    void Start() {
        SetupListBuffer();
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            AddCard();
        }
    }
#endregion
#region Functions
    private void SetupListBuffer() {
        listBuffer = new List<Item>(100);
        for (int i=0; i<itemSO.Length; i++) {
            Item item = itemSO[i].items[0];
            for (int j=0; j<item.count; j++) {
                listBuffer.Add(item);
            }
        }
        for (int i=0; i<listBuffer.Count; i++) {
            int rand = UnityEngine.Random.Range(i, listBuffer.Count);
            Item temp = listBuffer[i];
            listBuffer[i] = listBuffer[rand];
            listBuffer[rand] = temp;
        }
    }
    public Item PopList() {
        if (listBuffer.Count == 0) {
	        Debug.Log("카드 전부 뽑음");
            SetupListBuffer();
	    }
        
        Item temp = listBuffer[0];
        listBuffer.RemoveAt(0);
        return temp;
    }
    
	private void AddCard() {
		var cardObject = Instantiate(cardPrefab, cardSpawnPoint.position, Utils.QI);
		var card = cardObject.GetComponent<Card>();
		card.Setup(PopList());
        myCards.Add(card);

        SetOriginOrder();
        CardAlignment();
	}

    private void SetOriginOrder() {
        int count = myCards.Count;

        for (int i=0; i<count; i++) {
            var target = myCards[i];
            target?.GetComponent<Order>().SetOriginOrder(i);
        }
    }

    private void CardAlignment() {
        int count = myCards.Count;

        List<PRS> originCardPRS = new List<PRS>();
        originCardPRS = RoundAlignment(myCardLeft, myCardRight, count, 0.5f, Vector3.one * 1.9f);

        for (int i=0; i<count; i++) {
            var target = myCards[i];
            
            target.originPRS = originCardPRS[i];
            target.MoveTransform(target.originPRS, true, 0.7f);
        }
    }

    private List<PRS> RoundAlignment(Transform left, Transform right, int objCount, float height, Vector3 scale) {
        float[] objLerps = new float[objCount];
        List<PRS> results = new List<PRS>(objCount);

        switch (objCount) {
            case 1:
                objLerps = new float[] { 0.5f };
                break;
            case 2:
                objLerps = new float[] { 0.27f, 0.73f };
                break;
            case 3:
                objLerps = new float[] { 0.1f, 0.5f, 0.9f };
                break;
            default:
                float interval = 1f / (objCount - 1);
                for (int i=0; i<objCount; i++)
                    objLerps[i] = interval * i;
                break;
        }

        for (int i=0; i <objCount; i++) {
            var targetPos = Vector3.Lerp(left.position, right.position, objLerps[i]);
            var targetRot = Utils.QI;

            if (objCount >= 4) {
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                curve = height >= 0 ? curve : -curve;
                targetPos.y += curve;
                targetRot = Quaternion.Slerp(left.rotation, right.rotation, objLerps[i]);
            }

            results.Add(new PRS(targetPos, targetRot, scale));
        }


        return results;
    }
#endregion
}
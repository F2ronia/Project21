using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardManager : MonoBehaviour {
#region Singleton
    public static CardManager Instance {get; private set;}
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
#region Variables
    [SerializeField]
	private GameObject cardPrefab;
    public ItemSO[] itemSO;
    private List<Item> listBuffer;
    private List<Item> myCardList;
    // 카드 덱
    private List<Card> myCards;
    [SerializeField]
    private Transform cardSpawnPoint;
    [SerializeField]
    private Transform myCardLeft;
    [SerializeField]
    private Transform myCardRight;
    [SerializeField]
    private ECardState eCardState;
    private Card selectCard;
    private bool isDraggable;
    private bool onCardArea;
    private enum ECardState { Nothing, CanMouserOver, CanMouseDrag }
#endregion
#region UnityEventFunction
    void Start() {
        SetupListBuffer();
        TurnManager.OnAddCard += AddCard;
        TurnManager.OnTurnStarted += OnTurnStarted;
    }
    void Update() {
        if (isDraggable)
            CardDrag();

        DetectCardArea();
        SetECardState();
    }
    void OnDestroy() {
        TurnManager.OnAddCard -= AddCard;
        TurnManager.OnTurnStarted -= OnTurnStarted;
    }
#endregion
#region Functions
    private void OnTurnStarted(bool myTurn) {
        // 플레이어 마나회복
        // 카드 뽑기
        if (myTurn)
            Debug.Log("턴 돌아옴");
    }

    private void SetupListBuffer() {
        listBuffer = new List<Item>();
        for (int i=0; i<itemSO.Length; i++) {
            Item item = itemSO[i].item;
            for (int j=0; j<itemSO[i].item.count; j++) {
                listBuffer.Add(item);
            }
            if (itemSO[i].item.type == Item.Type.Normal) {
                myCardList.Add(item);
                //Debug.Log("카드 추가 : " + item);
            }
        }
        for (int i=0; i<myCardList.Count; i++) {
            int rand = UnityEngine.Random.Range(i, myCardList.Count);
            Item temp = myCardList[i];
            myCardList[i] = myCardList[rand];
            myCardList[rand] = temp;
        }
    }
    public Item PopList() {
        /*
        if (listBuffer.Count == 0) {
            Debug.Log("카드 전부 뽑음");
            SetupListBuffer();
        }
        
        Item temp = listBuffer[0];
        listBuffer.RemoveAt(0);
        return temp;

    */
        if (myCardList.Count == 0) {
            Debug.Log("카드 전부 뽑음");
            SetupListBuffer();
        } 

        Item temp = myCardList[0];
        myCardList.RemoveAt(0);
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

    #region Card
    public void CardMouseOver(Card card) {
        if (eCardState == ECardState.Nothing)
            return;

        selectCard = card;
        EnLargeCard(true, card);
    }
    public void CardMouseExit(Card card) {
        EnLargeCard(false, card);
    }
    public void CardMouseDown() {
        if (eCardState != ECardState.CanMouseDrag)
            return;

        isDraggable = true;
    }
    public void CardMouseUp() {
        isDraggable = false;

        if (eCardState != ECardState.CanMouseDrag)
            return;
    }
    private void CardDrag()
    {
        if (!onCardArea) {
            selectCard.MoveTransform(new PRS(Utils.MousePos, Utils.QI, selectCard.originPRS.scale), false);
        }
    }
    private void DetectCardArea() {
        RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
        int layer = LayerMask.NameToLayer("CardArea");
        onCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
    }
    private void SetECardState()
    {
        if (TurnManager.Instance.isLoading) {
            eCardState = ECardState.Nothing;
        } else if (!TurnManager.Instance.myTurn) {
            eCardState = ECardState.CanMouserOver;
        } else if (TurnManager.Instance.myTurn) {
            eCardState = ECardState.CanMouseDrag;
        }
    }
    private void EnLargeCard(bool isEnLarge, Card card) {

        if (isEnLarge) {
            Vector3 enlargePos = new Vector3(card.originPRS.pos.x, -2f, -10f);
            
            card.MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 2.65f), false);
        } else {
            card.MoveTransform(card.originPRS, false);
        }

        card.GetComponent<Order>().SetMostFrontOrder(isEnLarge);
    }
    #endregion
#endregion
}
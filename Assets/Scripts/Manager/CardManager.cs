using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;

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
#region SerializeField
    [SerializeField]
    private Transform myCardLeft;
    [SerializeField]
    private Transform myCardRight;
    [SerializeField]
    private ECardState eCardState;
#endregion
#region Public/Private
    public ItemSO[] itemSO;
    private List<Item> listBuffer;
    // 플레이용 카드 버퍼
    private List<Item> allCardList;
    // 전체 카드 리스트
    private List<Item> myCardList;
    // 카드 덱
    private List<Card> myCards;
    // 현재 플레이어가 소지한 카드
    private Card selectCard;
    private bool isDraggable;
    private bool onCardArea;
    private enum ECardState { Nothing, CanMouseOver, CanMouseDrag}
    private enum NormalCard { Attack, Armor, Heal }
#endregion
#region UnityEventFunction
    void Start() {
        SetupListBuffer();
        TurnManager.OnAddCard += AddCard;
        TurnManager.OnTurnStarted += OnTurnStarted;
        TurnManager.OnTurnEnd += OnTurnEnd;
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
        TurnManager.OnTurnEnd -= OnTurnEnd;
    }
#endregion
#region Functions
    private void OnTurnStarted(bool myTurn) {
        // 카드 뽑기
        if (myTurn) {
            Debug.Log("턴 돌아옴");
        }
    }
    private void OnTurnEnd(bool myTurn) {
        if (myTurn) {
            for (int i=myCards.Count-1; i>=0; i--) {
                //Destroy(myCards[i].gameObject, .5f);
                DestoryCard(myCards[i]);
                myCards.RemoveAt(i);
            }
        }
    }

    private void DestoryCard(Card card) {
        ObjectPooling.ReturnObject(card);
    }

    private void SetupListBuffer() {
    // 전체 카드 데이터 불러오기
        if (allCardList == null)
            allCardList = new List<Item>();
        if (myCardList == null)
            myCardList = new List<Item>();
        if (myCards == null)
            myCards = new List<Card>();

        for (int i=0; i<itemSO.Length; i++) {
            Item item = itemSO[i].item;        
            allCardList.Add(item);

            SetupMyCardList(item);
            // 초기 카드 세팅
        }
    }
    private void SetupMyCardList(Item item) {
    // 초기 카드 세팅
        if (item.type == Item.Type.Normal) {
        // 기본 카드 추가
        // 공격 5 : 방어 3 : 회복 2
            switch(item.num) {
                case 1: // Attack
                    for (int i=0; i<5; i++)
                        myCardList.Add(item);
                    break;
                case 2: // Armor
                    for (int i=0; i<3; i++)
                        myCardList.Add(item);
                    break;
                case 3: // Heal
                    for (int i=0; i<2; i++)
                        myCardList.Add(item);
                    break;
            }
        }

        SetupMyCardList();
        // 내 카드 데이터 세팅
    }
    private void SetupMyCardList() {
    // 내 카드 데이터 불러오기
        listBuffer = new List<Item>();
        
        for (int i=myCardList.Count-1; i>=0; i--) {
        // 플레이용 버퍼에 카드 데이터 저장
            Item item = myCardList[i];        
            listBuffer.Add(item);
        }

        for (int i=0; i<listBuffer.Count; i++) {
        // 셔플
            int rand = UnityEngine.Random.Range(i, listBuffer.Count);
            Item temp = listBuffer[i];
            listBuffer[i] = listBuffer[rand];
            listBuffer[rand] = temp;
        }
    }
    public Item PopList() {
        if (listBuffer.Count == 0) {
            SetupMyCardList();
        } 

        Item temp = listBuffer[0];
        listBuffer.RemoveAt(0);
        return temp;
    }

	private void AddCard(bool myTurn) {
        if (myTurn) {
            var card = ObjectPooling.GetObject();
            card.Setup(PopList());
            myCards.Add(card);

            SetOriginOrder();
            CardAlignment();
        }
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

        if (!onCardArea) {
            TryUseCard(selectCard);
        }
    }
    private void CardDrag()
    {
        if (!onCardArea) {
            selectCard.MoveTransform(new PRS(Utils.MousePos, Utils.QI, selectCard.originPRS.scale), false);
        }
    }

    public bool TryUseCard(Card card) {
        if (PlayerStatus.Instance.PlayerMana < card.item.cost) {
        // 사용하려는 카드의 비용이 현재 마나보다 많은 경우
            Debug.Log("마나가 부족합니다");
        } else {
            switch(card.item.target) {
                case Item.Target.My:
                case Item.Target.All:
                    card.CallActive(null, card.item.num);
                    break;
                case Item.Target.Enemy:
                    EntityManager.Instance.SelectTargetEntity(card);
                    break;
            }
            PlayerStatus.Instance.PlayerMana -= card.item.cost;
            DestoryCard(card);
            myCards.Remove(card);
            SetOriginOrder();
            CardAlignment();
        }
        return false;
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
            eCardState = ECardState.CanMouseOver;
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
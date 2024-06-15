using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.PerformanceData;
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
#region SerializeField
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
    private List<Item> selectList;
    // 카드 추가 이벤트용 리스트
    private Card selectCard;
    private bool isDraggable;
    private bool onCardArea;
    private const int NORMAL_CARD = 3;
    private enum ECardState { Nothing, CanMouseOver, CanMouseDrag}
    private enum NormalCard { Attack, Armor, Heal }
#endregion
#region Sound/Effect
    private AudioSource audioSource;
#endregion
#region UnityEventFunction
    void Start() {
        SetupListBuffer();
        audioSource = GetComponent<AudioSource>();
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
                DestoryCard(myCards[i]);
                //myCards.RemoveAt(i);
            }
            myCards.Clear();
        }
    }

    private void DestoryCard(Card card) {
        ObjectPooling.ReturnObject(card);
    }
    
    public void RemoveAllMyCards() {
        ObjectPooling.ReturnAllObject(myCards);
        myCards.Clear();
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
    /*
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
    */
        myCardList.Add(item);
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
            //audioSource.Stop();
            //audioSource.Play();
            
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
        Debug.Log(count);

        List<PRS> originCardPRS = new List<PRS>();
        originCardPRS = RoundAlignment(Utils.MyCardLeft, Utils.MyCardRight, count, 0.5f, Vector3.one * 1.9f);

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

    // 전투 승리 시 이벤트
    public IEnumerator BattleReward() {
        EntityManager.Instance.allEntity.Clear();
        RemoveAllMyCards();
        AddCardEvent();
        yield return Utils.D1;
    }

    public void AddCardEvent() {
    // 카드 추가 
        var temp  = GameObject.Find("BattleUI");
        temp.SetActive(false);
        int random;

        if (selectList == null)
            selectList = new List<Item>();
        // 특수 카드 리스트 중 3개 생성
        for (int i=0; i<3; i++) {
            random = UnityEngine.Random.Range(NORMAL_CARD, allCardList.Count);
            selectList.Add(allCardList[random]);
            SelectUI.Instance.AddSelect(selectList[i]);
            //추후 중복 방지 알고리즘 추가
        }
        selectList.Clear();
        // 3개 중 1장 선택
        // SelectedObject -> ReturnSelected()
        // 선택한 카드 덱에 추가
        // GetSelectCard()
    }

    public void GetSelectCard(Item item) {
        myCardList.Add(item);
        SelectUI.Instance.RemoveAll();

        RemoveCardEvent();
    }

    private void RemoveCardEvent() {
    // 카드 제거
        // 현재 보유한 카드 리스트 보여줌
        SelectUI.Instance.ShowList(myCardList);
        // 특정 카드 선택
        // SelectedObject -> RemoveSelected()
        // 선택한 카드 덱에서 제거
        // GetRemoveSelect()
    }

    public void GetRemoveSelect(Item item) {
        myCardList.Remove(item);
        SelectUI.Instance.RemoveAll();
        GameManager.Instance.CallAnyScene("temp_stage");
    }
    #endregion
#endregion
}
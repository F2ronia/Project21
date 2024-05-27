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
	public GameObject cardPrefab;
    public ItemSO ItemSO;
    List<Item> listBuffer;
#endregion
#region UnityEventFunction
    void Start() {
        SetupListBuffer();
    }
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            AddCard(true);
        }
    }
#endregion
#region Functions
    private void SetupListBuffer() {
        listBuffer = new List<Item>();
        for (int i=0; i<ItemSO.abilities.Length; i++) {
            Item abty = ItemSO.abilities[i];
            listBuffer.Add(abty);
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
    
	private void AddCard(bool isMine) {
		var cardObject = Instantiate(cardPrefab, Vector3.zero, Quaternion.identity);
		var card = cardObject.GetComponent<Card>();
		card.Setup(PopList(), isMine);
		//listBuffer.Add(card);
	}
#endregion
}
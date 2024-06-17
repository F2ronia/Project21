using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectUI : MonoBehaviour
{
#region Singleton
    public static SelectUI Instance {get; private set;}
    void Awake() {
        if (Instance != this && Instance != null) {
            return;
        } else {
            Instance = this;
        }
    }
#endregion
    [SerializeField]
    private GridLayoutGroup gridLayoutGroup;

    [SerializeField]
    private GameObject selectPrefab;
    [SerializeField]
    private GameObject listPrefab;

    public void AddSelect(Item item) {
        gridLayoutGroup.cellSize = Utils.SelectLayout;

        var temp = Instantiate(selectPrefab, Utils.VZ, Utils.QI);
        temp.transform.parent = this.transform;
        temp.transform.localScale = Utils.SO;

        temp.GetComponent<Card>().item = item;
        temp.GetComponent<Image>().sprite = item.main;
        temp.GetComponentInChildren<TMP_Text>().text = item.name;
    }

    public void RemoveAll() { 
        int count = this.transform.childCount;

        for (int i=0; i<count; i++) {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void ShowList(List<Item> items) {
        gridLayoutGroup.cellSize = Utils.ListLayout;

        for (int i=0; i<items.Count; i++) {
            var temp = Instantiate(listPrefab, Utils.VZ, Utils.QI);
            temp.transform.parent = this.transform;
            temp.transform.localScale = Utils.SO;

            temp.GetComponent<Card>().item = items[i];
            temp.GetComponent<Image>().sprite = items[i].main;
            temp.GetComponentInChildren<TMP_Text>().text = items[i].name;
        }
    }
}
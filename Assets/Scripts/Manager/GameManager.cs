using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
#region Singleton
    public static GameManager Instance { get; private set; }
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
    NoticePannel noticePannel;
    [SerializeField]
    private EntitySO[] entitySOs;
    // 모든 적 정보 
    private List<Status> entityList;
    // 적 생성용 리스트

    private void Start() {
        SceneManager.sceneLoaded += LoadSceneEvent;
    }

    void Update() {
        #if UNITY_EDITOR
            InputCheatKey();
        #endif
    }

    private void LoadSceneEvent(Scene scene, LoadSceneMode mode) {
        if (scene.name == "temp_battle") {
            CallBattle();
        }
    }

    private void InputCheatKey() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            TurnManager.OnAddCard?.Invoke(true);
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            TurnManager.Instance.EndTurn();
        }
    }

    public void Notification(string msg) {
        Debug.Log("테스트");
        //noticePannel.Show(msg);
    }

    private void EnemySpawn(Status status) {
        EntityManager.Instance.SpawnEntity(status);
        //EntityManager.Instance.InsertEmptyEntity(Utils.MousePos.x);
    }

    public void LoadTriggerEnemy() {
        Debug.Log("데이터 불러오기 ");
        if (entityList == null)
            entityList = new List<Status>();

        for (int i=0; i<3; i++) {
            Status status = entitySOs[i].status;
            entityList.Add(status);
        }  
    }

    public void CallBattle() { 
        SetupEnemyList();

        StartCoroutine(TurnManager.Instance.StartBattle());
    }

    private void SetupEnemyList() {
        if (EntityManager.Instance.allEntity != null) {
            EntityManager.Instance.allEntity = null;
            EntityManager.Instance.allEntityUi = null;     
        }

        EntityManager.Instance.allEntity = new List<Entity>();
        EntityManager.Instance.allEntityUi = new List<EnemyUI>();

        for (int i=0; i < entityList.Count; i++) {
            EnemySpawn(entityList[i]);
        }  

        entityList = null;
    }

    public void CallAnyScene (string scene) {
        SceneManager.LoadScene(scene);
    }
}
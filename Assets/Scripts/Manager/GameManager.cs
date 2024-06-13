using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BGM {
    Main,
    Stage,
    Battle_Normal,
    Battle_Danger,
    Battle_Elite
}

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
    [Header("Develop")]
    [Tooltip("모드 설정")]
    [SerializeField]
    private bool EditMode;
    [SerializeField]
    NoticePannel noticePannel;
    [SerializeField]
    private EntitySO[] entitySOs;
    // 모든 적 정보 
    private List<Status> entityList;
    // 적 생성용 리스트
    [SerializeField]
    private AudioClip[] audioClips; 
    private AudioSource audioSource;

    private void Start() {
        if (EditMode) {
        LoadTriggerEnemy();
        CallBattle();
        }

        audioSource = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += LoadSceneEvent;
    }

    void Update() {
        #if UNITY_EDITOR
            InputCheatKey();
        #endif
    }

    private void LoadSceneEvent(Scene scene, LoadSceneMode mode) {
        if (scene.name == "temp_main") {
            audioSource.clip = audioClips[(int)BGM.Main];
        }
        if (scene.name == "temp_stage") {
            audioSource.clip = audioClips[(int)BGM.Stage];
        }
        if (scene.name == "temp_battle") {
            audioSource.clip = audioClips[(int)BGM.Battle_Normal];
            CallBattle();
        }
        audioSource.Stop();
        audioSource.Play();
    }

    private void InputCheatKey() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            TurnManager.OnAddCard?.Invoke(true);
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            CardManager.Instance.AddCardEvent();
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

    public void ChangeBattleBGM(bool isDanger) {
    // 전투 중 일정 체력 이하 시 BGM 변경
        var temp = isDanger ? audioClips[(int)BGM.Battle_Danger] : audioClips[(int)BGM.Battle_Normal];

        if (temp == audioSource.clip) {
            return;
        } else { 
            audioSource.clip = temp;
            audioSource.Stop();
            audioSource.Play();
        }
    }
}
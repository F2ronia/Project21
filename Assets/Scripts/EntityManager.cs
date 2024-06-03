using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

// 적 생성 및 관리 스크립트
public class EntityManager : MonoBehaviour
{
#region Singleton
    public static EntityManager Instance {get; private set;}
    void Awake() => Instance = this;
#endregion
#region Serialized
    [SerializeField]
    private GameObject entityPrefab;
    // 적 프리팹
    [SerializeField]
    private EntitySO[] entitySOs;
    private List<Status> entityList;
    [SerializeField]
    private List<Entity> entities;
    [SerializeField]
    private Entity EmptyEntity;
    [SerializeField]
    private Transform entitySpawnPoint; 
    [SerializeField]
    private GameObject TargetPicker;
    [SerializeField]
    private GameObject entityUI;
    // 타겟
#endregion
#region Entity
    private const int MAX_ENTITY_COUNT = 3;
    public bool IsFullEntities => entities.Count >= MAX_ENTITY_COUNT && !ExistEmptyEntity;
    private bool ExistEmptyEntity => entities.Exists(x => x == EmptyEntity);
    private int EmptyEntityIndex => entities.FindIndex(x => x == EmptyEntity);
    private bool ExistTargetPickEntity => targetPickEntity != null;
    private bool CanMouseInput => TurnManager.Instance.myTurn && !TurnManager.Instance.isLoading;

    public List<Entity> allEntity;
    public Entity targetPickEntity;
    // 적 행동 실행 여부 체크
#endregion
    void Start() {
        SetupEnemyList();
    }

    void Update() {
        ShowTargetPicker(ExistTargetPickEntity);
    }

    private void ShowTargetPicker(bool isShow) {
        TargetPicker.SetActive(isShow);
        if (ExistTargetPickEntity) {
            TargetPicker.transform.position = targetPickEntity.transform.position;
        }
    }

    private void SetupEnemyList() {
        entityList = new List<Status>();
        allEntity = new List<Entity>();

        for (int i=0; i<entitySOs.Length; i++) {
            Status status = entitySOs[i].status;

            entityList.Add(status);
            //Debug.Log("추가 확인 + " + entityList[i].name);
        }
    }

    public bool EnemyTurnEnd() {
        int value = 0;

        for (int i=allEntity.Count-1; i>=0; i--) {
            if (allEntity[i].isActived) {
                value++;
            }
        }
        
        if (allEntity.Count == value) {
            return true;
        } else {
            return false;
        }
    }
    
    public void ResetEnemyIsActive() {
        for (int i=allEntity.Count-1; i>=0; i--) {
            allEntity[i].isActived = false;
        }
    }

    private Status PopList() {
        Status temp = entityList[0];
        entityList.RemoveAt(0);
        return temp;
    }

    private void EntityAlignment() {
        float targetY = 4.15f;
        var targetEntities = entities;

        for (int i=0; i<targetEntities.Count; i++) {
            float targetX = (targetEntities.Count - 1) * -3.4f + i * 6.8f;

            var targetEntity = targetEntities[i];
            targetEntity.originPos = new Vector3(targetX, targetY, 0);
            targetEntity.MoveTransform(targetEntity.originPos, true, 0.5f);
            targetEntity.GetComponent<Order>()?.SetOriginOrder(i);
        }
    }

    public void InsertEmptyEntity(float xPos) {
        if (IsFullEntities)
            return;
        
        if (!ExistEmptyEntity)
            entities.Add(EmptyEntity);

        Vector3 emptyEntityPos = EmptyEntity.transform.position;
        emptyEntityPos.x = xPos;
        EmptyEntity.transform.position = emptyEntityPos;

        int _emptyEntityIndex = EmptyEntityIndex;
        entities.Sort((entity1, entity2) => entity1.transform.position.x.CompareTo(entity2.transform.position.x));
        if (EmptyEntityIndex != _emptyEntityIndex)
            EntityAlignment();
    }

    public void RemoveEmptyEntity() {
        if (!ExistEmptyEntity)
            return;

        entities.RemoveAt(EmptyEntityIndex);
        EntityAlignment();
    }

    public bool SpawnEntity() {
        if (IsFullEntities || !ExistEmptyEntity || entityList.Count == 0)
            return false;

        var enemyObj = Instantiate(entityPrefab, entitySpawnPoint.position, Utils.QI);
        var enemy = enemyObj.GetComponent<Enemy>();

        var uiCanvas = GameObject.Find("BattleUI").GetComponent<Canvas>();
        var enemyUI = Instantiate<GameObject>(entityUI, uiCanvas.transform);

        var slider = enemyUI.GetComponent<EnemyUI>();
        slider.targetTr = enemyObj.transform;
        slider.offset = new Vector3(0, -8f, 0);
        slider.enemy = enemy;

        entities[EmptyEntityIndex] = enemy;
        enemy.Setup(PopList());
        allEntity.Add(enemy);
        EntityAlignment();

        return true;
    }

    public void EntityMouseDrag() {
        bool existTarget = false;

        foreach (var hit in Physics2D.RaycastAll(Utils.MousePos, Vector3.forward)) {
            Entity entity = hit.collider?.GetComponent<Entity>();
            if (entity != null && !entity.isDead) {
                targetPickEntity = entity;
                existTarget = true;
                break;
            }
        }
        if (!existTarget)
            targetPickEntity = null;
    }
}
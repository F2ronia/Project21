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
    // 모든 적 정보 
    [SerializeField]
    private Entity EmptyEntity;
    [SerializeField]
    private Transform entitySpawnPoint; 
    [SerializeField]
    private GameObject TargetPicker;
    [SerializeField]
    private GameObject entityUI;
    private EntityState entityState;
    private enum EntityState { Nothing, CanMouseOver, Attack }
    // 타겟
#endregion
#region Entity
    private const int MAX_ENTITY_COUNT = 3;
    public bool IsFullEntities => allEntity.Count >= MAX_ENTITY_COUNT;
    private bool IsSelected = false;
    public List<Entity> allEntity;
    // 현재 스테이지 내 생존 적 리스트
    public List<EnemyUI> allEntityUi;
    // 현재 스테이지 내 생존 적 UI
    public Entity targetPickEntity;
    // 적 행동 실행 여부 체크
#endregion
    void Update() {
        SetEntityState();
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

    private void EntityAlignment() {
        float leftOrder = -3.4f;
        float padding = 6.8f;
        float interval = 2f;
        float targetY = 4.15f;
        var targetEntities = allEntity;

        for (int i=0; i<targetEntities.Count; i++) {
            float targetX = (targetEntities.Count - 1) * leftOrder + i * padding + i * interval;

            var targetEntity = targetEntities[i];
            targetEntity.originPos = new Vector3(targetX, targetY, 0);
            targetEntity.MoveTransform(targetEntity.originPos, true, 0.5f);
            targetEntity.GetComponent<Order>()?.SetOriginOrder(i);
        }
    }

    public bool SpawnEntity(Status status) {
        if (IsFullEntities) {
            return false;
        }

        var enemyObj = Instantiate(entityPrefab, entitySpawnPoint.position, Utils.QI);
        var enemy = enemyObj.GetComponent<Enemy>();

        var uiCanvas = GameObject.Find("BattleUI").GetComponent<Canvas>();
        var enemyUI = Instantiate<GameObject>(entityUI, uiCanvas.transform);

        var slider = enemyUI.GetComponent<EnemyUI>();
        slider.targetTr = enemyObj.transform;
        slider.offset = new Vector3(0, -8f, 0);
        slider.enemy = enemy;

        enemy.Setup(status);
        allEntity.Add(enemy);
        allEntityUi.Add(slider);
        EntityAlignment();

        return true;
    }
#region 마우스 상호작용
    private void ShowTargetPicker(bool isShow, Entity entity) {
        Vector3 entityPos = entity.gameObject.transform.position;

        TargetPicker.SetActive(isShow);
        TargetPicker.transform.position = entityPos;
    }

    public void HideTargetPicker() {
        TargetPicker.SetActive(false);
    }
    public void EntityMouseOver(Entity entity) {
        if (entityState == EntityState.Nothing || entityState == EntityState.Attack)
            return;
        
        ShowTargetPicker(true, entity);
    }
    public void EntityMouseExit(Entity entity) {
        ShowTargetPicker(false, entity);
    }
    public void EntityMouseUp() {
        EntityCheck();
    }
    private void EntityCheck() {
        bool existTarget = false;    

        foreach (var hit in Physics2D.RaycastAll(Utils.MousePos, Vector3.forward)) {
            Entity entity = hit.collider?.GetComponent<Entity>();
            if (entity != null && !entity.isDead) {
                targetPickEntity = entity;
                existTarget = true;
                IsSelected = true;
                break;
            }
        }
        if (!existTarget) 
            targetPickEntity = null;
    } 

    public void SelectTargetEntity(Card card) {
        StartCoroutine(DelayUntilSelect(card));
    }

    private IEnumerator DelayUntilSelect(Card card) {
        entityState = EntityState.CanMouseOver;
        yield return new WaitUntil(() => IsSelected == true);
        if (targetPickEntity != null) {
            card.CallActive(targetPickEntity, card.item.num);
            entityState = EntityState.Nothing;
            IsSelected = false;
        }
    }

    private void SetEntityState() {
        if (TurnManager.Instance.isLoading) {
            entityState = EntityState.Nothing;
        } else if (!TurnManager.Instance.myTurn) {
            // 플레이어 턴이 아닐 경우 
            entityState = EntityState.Attack;
        }
    }
#endregion
}
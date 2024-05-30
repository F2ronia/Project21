using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

// 적 생성 및 관리 스크립트
public class EntityManager : MonoBehaviour
{
#region Singleton
    public static EntityManager Instance {get; private set;}
    void Awake() => Instance = this;
#endregion

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
    // 적 정보 
    private const int MAX_ENTITY_COUNT = 3;
    public bool IsFullEntities => entities.Count >= MAX_ENTITY_COUNT && !ExistEmptyEntity;
    private bool ExistEmptyEntity => entities.Exists(x => x == EmptyEntity);
    private int EmptyEntityIndex => entities.FindIndex(x => x == EmptyEntity);

    void Start() {
        SetupEnemyList();
    }

    private void SetupEnemyList() {
        entityList = new List<Status>();

        for (int i=0; i<entitySOs.Length; i++) {
            Status status = entitySOs[i].status;

            entityList.Add(status);
            Debug.Log("추가 확인 + " + entityList[i].name);
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

        entities[EmptyEntityIndex] = enemy;
        enemy.Setup(PopList());
        EntityAlignment();

        return true;
    }
}
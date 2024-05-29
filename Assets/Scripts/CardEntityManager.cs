using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEntityManager : MonoBehaviour
{
#region Singleton
    public static CardEntityManager Instance {get; private set;}
    void Awake() => Instance = this;
#endregion

    [SerializeField]
    private CardEntity cardEntity;   
}
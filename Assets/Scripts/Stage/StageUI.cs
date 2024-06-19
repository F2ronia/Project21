using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageUI : MonoBehaviour
{
    public void GoTitle()
    {
        gameObject.transform.GetChild(1).gameObject.SetActive(false);
        GameManager.Instance.GameOver();
    }
}

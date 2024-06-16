using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
#region Singleton
    public static ParticleManager Instance {get; private set;}
    void Awake() => Instance = this;
#endregion
    public GameObject[] particles;

    public void SpawnParticle(int type, Transform spawnPos) {
        var obj = Instantiate(particles[type]);
        obj.transform.position = spawnPos.localPosition;
        Destroy(obj, 2f);
    }
}

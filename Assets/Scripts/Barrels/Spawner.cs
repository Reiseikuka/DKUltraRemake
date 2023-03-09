using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;

    public float minTime;
    public float maxTime = 4f;
    //how often will it spawn

    private void Start()
    {
        Spawn();
    }
    private void Spawn()
    {
        Instantiate(prefab, transform.position, Quaternion.identity);
        //copy of  the existent object
        Invoke(nameof(Spawn), Random.Range(minTime, maxTime));

    }
}

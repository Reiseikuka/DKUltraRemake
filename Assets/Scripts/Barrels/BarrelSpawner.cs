using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelSpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private Transform barrelSpawnLocation;
    private GameObject currentBarrel;

    private Animator animator;
    private int throwBarrelHash;

    private float spawnTime;
    private float minSpawnTime;
    [SerializeField] private float maxSpawnTime = 4f;
    //how often will it spawn

    private void Awake()
    {
        animator = GetComponent<Animator>();
        throwBarrelHash = Animator.StringToHash("ThrowBarrel");
        AnimationClip[] animations = animator.runtimeAnimatorController.animationClips;

        foreach (AnimationClip animation in animations)
        {
            if (animation.name == "DK_ThrowBarrel")
                minSpawnTime = animation.length;

        }
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    private void Update()
    {
        spawnTime -= Time.deltaTime;
        if(spawnTime <= 0)
        {
            spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
            animator.SetTrigger(throwBarrelHash);
            Spawn();
        }
    }
    private void Spawn()
    {
        currentBarrel = Instantiate(prefab, barrelSpawnLocation.position, Quaternion.identity);
        currentBarrel.transform.parent = barrelSpawnLocation;
    }

    public void ReleaseBarrel()
    {
        currentBarrel.transform.parent = null;
        currentBarrel.GetComponent<Rigidbody2D>().simulated = true;
    }

}

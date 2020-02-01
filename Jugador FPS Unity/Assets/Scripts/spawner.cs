using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawner : MonoBehaviour
{
    public GameObject prefabEnem;
    public Vector3 whereSpawn;
    public PlayerHealth player;

    // Update is called once per frame
    void Start()
    {
        InvokeRepeating("Spawner", 3, 3);
        //player = FindObjectOfType<PlayerHealth>();
    }

    void Spawner()
    {
        Vector3 spawner = new Vector3(Random.Range(-whereSpawn.x, whereSpawn.x), 1, Random.Range(-whereSpawn.z, whereSpawn.z));
        GameObject spawnGo = Instantiate(prefabEnem, spawner, gameObject.transform.rotation);
    }
}

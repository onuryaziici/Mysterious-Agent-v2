using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnControl : MonoBehaviour
{
    public GameObject spawnPlus;

    float posX, posZ;
    int countPlus;
    GameObject plus;

    Vector3 posSpawn;
    void Update()
    {
        SpawnPlus();
    }
    void SpawnPlus()
    {
        posX = Random.Range(-8, 8);
        posZ = Random.Range(-18, 18);
        posSpawn = new Vector3(posX, 0.6f, posZ);

        countPlus = GameObject.FindGameObjectsWithTag("Plus").Length;

        if (countPlus == 0)
        {
            var spawn = Instantiate(spawnPlus, posSpawn, Quaternion.identity);
            plus = spawn.gameObject;
        }
        else
        {
            plus.transform.Rotate(0, Time.deltaTime * 90, 0);
        }
    }
}

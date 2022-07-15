using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    GameObject player;
    Vector3 offset = new Vector3(0, 15, -10);
    private void Awake()
    {
        player = GameObject.Find("Player");
    }
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, Time.deltaTime * 5);
    }
}

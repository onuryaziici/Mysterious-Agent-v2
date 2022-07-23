using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public List<GameObject> meshArrows;
    public GameObject trapArrow;
    public GameObject trapButton;
    GameObject player;
    private void Awake()
    {
        player = GameObject.Find("Player");
    }
    void Start()
    {
        
    }
    void Update()
    {
        if (meshArrows[0] != null && meshArrows[1] != null && meshArrows[2] != null)
        {
            if (player.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled == true && PlayerPrefs.GetInt("Enemies") != 0)
            {
                meshArrows[0].gameObject.SetActive(true);
                meshArrows[1].gameObject.SetActive(true);
                meshArrows[2].gameObject.SetActive(true);
            }
            else if (player.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled == false)
            {
                meshArrows[0].gameObject.SetActive(false);
                meshArrows[1].gameObject.SetActive(false);
                meshArrows[2].gameObject.SetActive(false);
            }
            else if (PlayerPrefs.GetInt("Enemies") == 0)
            {
                meshArrows[0].gameObject.SetActive(false);
                meshArrows[1].gameObject.SetActive(false);
                meshArrows[2].gameObject.SetActive(false);
            }
        }
        if (PlayerPrefs.GetInt("Enemies") == 1)
        {
            Destroy(meshArrows[0]);
            Destroy(meshArrows[1]);
            Destroy(meshArrows[2]);
        }

        if (trapButton.transform.localScale.y >= 2.01f && PlayerPrefs.GetInt("Enemies") == 1)
        {
            trapArrow.SetActive(true);
        }
        else if(trapButton.transform.localScale.y < 2.01f )
        {
            Destroy(trapArrow);
        }
        else if (PlayerPrefs.GetInt("Enemies") == 0)
        {
            Destroy(trapArrow);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelUp : MonoBehaviour
{
    public static LevelUp instance;
    public List<Image> levelUp;
    int index = 0;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            return;
        }
    }
    void Start()
    {

    }
    void Update()
    {
        Debug.Log(index);
        //LevelImage();
    }
    public void LevelInc()
    {
        if (LevelManager.control.score % 5 != 1)
        {
            levelUp[index].GetComponent<Image>().color = new Color(255, 255, 255, 255);
            index++;
        }
    }
    public void LevelImage()
    {
        if (LevelManager.control.score % 5 == 0)
        {
            index = 0;
            for (int i = index; i < levelUp.Count; i++)
            {
                levelUp[i].GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            }
        }
        if (LevelManager.control.score % 5 == 1)
        {
            for (int i = index; i < levelUp.Count; i++)
            {
                levelUp[i].GetComponent<Image>().color = new Color32(255, 255, 255, 128);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public static Player instance = null;
    public float speedAmount=1f;
    public int maxHealth = 100;
    public int currentHealth;
    Animator anim;
    public bool isDie = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        LevelUp.instance.LevelImage();
    }



    public void TakeDamage(int damage)
    {
  
       Die();
        
    }
    void Die()
    {
        isDie = true;
        anim.SetBool("isDie", isDie);
        this.gameObject.GetComponent<JoystickControl>().enabled = false;
        StartCoroutine(Restart());
        LevelManager.control.SaveDie();
    }
    IEnumerator Restart()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

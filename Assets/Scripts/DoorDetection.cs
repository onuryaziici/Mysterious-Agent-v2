using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DoorDetection : MonoBehaviour
{
    [SerializeField] Animator anim = null;
    [SerializeField] bool openTrigger = false;
    [SerializeField] bool closeTrigger = false;
    [SerializeField] Animator finishAnim;
    [SerializeField] int openEnemyCount, closeEnemyCount;
    GameObject player;
    private void Awake()
    {
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && PlayerPrefs.GetInt("Enemies")==openEnemyCount)
        {
            GameObject.Find("door 1").GetComponent<MeshCollider>().enabled=false;
            if (openTrigger)
            {
                anim.Play("DoorOpen", 0, 0.0f);
                 gameObject.SetActive(false);
                if (AgentCount.instance.agentCount == 0 && player.transform.position.z < 25)
                {
                    AgentCount.instance.agentCount = AgentCount.instance.newAgentCount;
                    AgentCount.instance.agentCountText.text = "" + AgentCount.instance.agentCount;
                }
            }

           else if (closeTrigger && PlayerPrefs.GetInt("Enemies") == closeEnemyCount)
            {
                StartCoroutine(WaitForNextScene());
                finishAnim.SetTrigger("Finish");
                LevelManager.control.SaveStart();
            }
        }
    }
    IEnumerator WaitForNextScene()
    {
        LevelManager.control.ScoreInc();
        yield return new WaitForSeconds(1f);
    }


}

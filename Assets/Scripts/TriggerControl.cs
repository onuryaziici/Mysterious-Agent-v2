using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TriggerControl : MonoBehaviour
{
    public static TriggerControl instance = null;
    GameObject triggerObject;
    public Image reloadImage, lifeBar, meshReturnBar;
    public Text incNumber;
    public ParticleSystem cloud;
    public ParticleSystem lifeBarPlus;
    public float recoveryTime;
    Animator anim;
    bool meshChange = false;
    public bool isCompleted = false;
    public bool safe = false;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    private void Start()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
        anim = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hide"))
        {
            meshChange = true;
            triggerObject = other.gameObject;
        }
        if (other.gameObject.CompareTag("Attack") && !gameObject.GetComponent<BoxCollider>().Equals(null))
        {
            safe = false;
            if (!gameObject.GetComponent<BoxCollider>().Equals(null))
            {
                cloud.Play();
            }

            gameObject.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = true;
            gameObject.GetComponent<Renderer>().enabled = false;

            if (gameObject.GetComponent<CapsuleCollider>().Equals(null))
            {
                gameObject.AddComponent<CapsuleCollider>();

                gameObject.GetComponent<CapsuleCollider>().height = 2;
                gameObject.GetComponent<CapsuleCollider>().radius = 0.5f;
                gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0, 1, 0);
            }

            Destroy(gameObject.GetComponent<BoxCollider>());
            Destroy(gameObject.GetComponent<MeshFilter>());
            meshReturnBar.gameObject.SetActive(false);
            Attack();
            StartCoroutine(other.gameObject.GetComponentInParent<AIController>().TakeDamage());
            meshChange = false;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Hide"))
        {
            Debug.Log("MeshChange");
            meshChange = false;
            triggerObject = other.gameObject;
            reloadImage.fillAmount = 0;
        }
    }
    private void Update()
    {

        Reload();
        MeshChange();
        MeshPlayer();
        MeshReturn();
    }
    void Reload()
    {
        reloadImage.transform.parent.rotation = Quaternion.Euler(0, gameObject.transform.rotation.y, 0);

        if (meshChange && gameObject.GetComponent<BoxCollider>().Equals(null))
        {
            reloadImage.fillAmount += Time.deltaTime / 3;
        }
        if (reloadImage.fillAmount == 1)
        {
            isCompleted = true;
            cloud.Play();
        }
        else
        {
            isCompleted = false;
        }
    }
    void MeshChange()
    {
        if (isCompleted && meshChange)
        {
            safe=true;
            gameObject.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = false;
            gameObject.AddComponent<MeshFilter>();
            gameObject.GetComponent<Renderer>().enabled = true;

            gameObject.GetComponent<MeshFilter>().mesh = triggerObject.GetComponentInParent<MeshFilter>().mesh;
            gameObject.GetComponent<Renderer>().material = triggerObject.GetComponentInParent<Renderer>().material;

            if (gameObject.GetComponent<BoxCollider>().Equals(null))
            {
                gameObject.AddComponent<BoxCollider>();
            }
            Destroy(gameObject.GetComponent<CapsuleCollider>());

            reloadImage.fillAmount = 0;
            meshReturnBar.fillAmount = 1;
            meshReturnBar.gameObject.SetActive(true);         
        }
    }
    void MeshReturn()
    {
        if (meshReturnBar.fillAmount == 0 && meshReturnBar.gameObject.activeInHierarchy)
        {
            meshChange = false;
            safe = false;
            if (!gameObject.GetComponent<BoxCollider>().Equals(null))
            {
                cloud.Play();
            }

            gameObject.transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().enabled = true;
            gameObject.GetComponent<Renderer>().enabled = false;

            if (gameObject.GetComponent<CapsuleCollider>().Equals(null))
            {
                gameObject.AddComponent<CapsuleCollider>();

                gameObject.GetComponent<CapsuleCollider>().height = 2;
                gameObject.GetComponent<CapsuleCollider>().radius = 0.5f;
                gameObject.GetComponent<CapsuleCollider>().center = new Vector3(0, 1, 0);
            }

            Destroy(gameObject.GetComponent<BoxCollider>());
            Destroy(gameObject.GetComponent<MeshFilter>());
            meshReturnBar.gameObject.SetActive(false);
        }
    }
    void MeshPlayer()
    {
        if (!gameObject.GetComponent<BoxCollider>().Equals(null))
        {
            meshReturnBar.fillAmount -= Time.deltaTime / recoveryTime;
        }
    }
    void Attack()
    {
        anim.SetTrigger("Attack");
    }
}

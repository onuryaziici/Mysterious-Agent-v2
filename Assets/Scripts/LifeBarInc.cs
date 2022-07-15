using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBarInc : MonoBehaviour
{
    bool BarInc = false;
    Renderer plusRend;
    private void Awake()
    {
        plusRend = GetComponent<Renderer>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            BarInc = true;
            TriggerControl.instance.lifeBar.fillAmount += 0.1f;
            TriggerControl.instance.lifeBarPlus.Play();
            Destroy(gameObject, 0.1f);
        }
        if (collision.gameObject.CompareTag("Untagged"))
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            plusRend.enabled = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hide"))
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        LifeInc();
    }
    void LifeInc()
    {
        if (BarInc)
        {
            TriggerControl.instance.incNumber.gameObject.SetActive(true);
        }
        else
        {
            TriggerControl.instance.incNumber.gameObject.SetActive(false);
        }
    }
}

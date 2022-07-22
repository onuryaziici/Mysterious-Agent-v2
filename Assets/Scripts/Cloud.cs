using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    
    [SerializeField] List<Transform> cloudTransform = new List<Transform>();
    [SerializeField] float speed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, speed * Time.deltaTime, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Spawn"))
        {
            
            int randomPoint = Random.Range(0, cloudTransform.Count);
            Instantiate(this.gameObject, cloudTransform[randomPoint].position, cloudTransform[randomPoint].rotation);
            
        }
        if (other.gameObject.CompareTag("Bound"))
        {
            Destroy(this.gameObject);
        }
    }
}

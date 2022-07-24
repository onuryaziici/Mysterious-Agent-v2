using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionOfTrapObject : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            this.gameObject.tag = "Obstacle";
            this.gameObject.layer = LayerMask.NameToLayer("Obstacle");
        }
        
    }
}

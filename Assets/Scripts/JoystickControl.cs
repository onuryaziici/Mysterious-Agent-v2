using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoystickControl : MonoBehaviour
{
    public static JoystickControl instance = null;
    public DynamicJoystick dynamic;
    public GameObject background;

    float moveSpeed = 5;
    float turnSpeed = 5;
    public Rigidbody rb;

    public Animator playerAnim;
    public bool canMove=true;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (instance == null)
        {
            instance = this;
        }
    }
    void FixedUpdate()
    {
        if (playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Punch"))
        {
            canMove=false;
        }
        else
        {
            canMove=true;
        }
        if (Input.GetButton("Fire1") && canMove)
        {
            Joystick();
        }
        else
        {
             rb.velocity = new Vector3(0, 0, 0);
        }
    }
    void Joystick()
    {
        float horizontal = dynamic.Horizontal;
        float vertical = dynamic.Vertical;
        Vector3 addesPos = new Vector3(x: horizontal * Time.deltaTime * moveSpeed, y: 0, z: vertical * Time.deltaTime * moveSpeed);
        transform.position += addesPos;

        Vector3 direction = Vector3.forward * vertical + Vector3.right * horizontal;
        transform.rotation = Quaternion.Slerp(a: transform.rotation, b: Quaternion.LookRotation(direction), t: turnSpeed * Time.deltaTime);
    }
}

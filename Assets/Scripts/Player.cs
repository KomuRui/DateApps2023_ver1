using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody rb;
    private bool isJump = false;

    [SerializeField]
    private float speed = 1.0f;

    [SerializeField]
    private float jumpPower = 300;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(-Input.GetAxis("L_Stick_H2") * speed * Time.deltaTime, 0,0);
    }

    void OnCollisionEnter(Collision collision)  
    {
        if (collision.transform.tag == "Stage")
            isJump = false;
    }
}

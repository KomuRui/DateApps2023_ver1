using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CarrtBall : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        rb = this.gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.AddForce(new Vector3(0, 0.01f, 0));

        //äCÇÊÇËâ∫Ç…åæÇ¡ÇΩÇÁÉäÉXÉ|Å[Éì
        if (transform.position.y <= -1.5)
        {
            transform.position = startPos;
            rb.velocity = Vector3.zero;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Goal")
        {
            GameManager.nowMiniGameManager.SetMiniGameFinish();
        }
    }
}
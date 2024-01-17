using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vortex : MonoBehaviour
{
    [SerializeField] private bool isSpown = false;
    [SerializeField] private float despawnTime = 3f;
    [SerializeField] private float speed = 4000f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isSpown) return;

        //数秒に非アクティブになる
        Invoke("Despawn", despawnTime);
        isSpown = true;
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag == "ThreePlayer")
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            rb.AddForce((transform.position - other.transform.position).normalized * speed * Time.deltaTime);
        }
    }

    //デスポーン
    public void Despawn()
    {
        this.gameObject.SetActive(false);
        isSpown = false;
    }
}

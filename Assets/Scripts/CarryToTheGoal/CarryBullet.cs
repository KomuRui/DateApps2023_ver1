using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CarryBullet : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.0f; //ˆÚ“®‘¬“x

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // ˆÚ“®
        transform.position += Vector3.down * moveSpeed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "CarryStage" || other.gameObject.tag == "Player")
            Destroy(this.gameObject);
    }
}

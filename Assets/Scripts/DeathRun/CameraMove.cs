using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField]private float speed = 0.002f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z > 20) return;

        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speed * Time.deltaTime); 
    }
}

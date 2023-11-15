using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var c = Camera.main.transform.position;
        var p = transform.position;
        c.x = p.x;
        transform.LookAt(2 * p - c);
    }
}

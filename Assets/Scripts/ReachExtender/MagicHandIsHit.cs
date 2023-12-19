using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicHandIsHit : MonoBehaviour
{

    public bool isHit = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isHit = true;
        }
    }

    void OnTriggerEixt(Collider other)
    {
        if(other.tag == "Player")
        {
            isHit = false;
        }
       
    }
}

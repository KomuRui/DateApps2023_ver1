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
        if (other.tag == "Arm") return;

        if (other.tag == "Stage")
        {
            isHit = true;
        }
        else
        {
            isHit = false;
        }
    }

}

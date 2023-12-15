using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetCollider : MonoBehaviour
{
    public Net net;
    public List<GameObject> fishObj = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnTriggerEnter(Collider collision)
    {
        if (collision.transform.tag == "Fishes") 
            fishObj.Add(collision.gameObject);
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.transform.tag == "Fishes") 
            fishObj.Remove(collision.gameObject);
    }
}

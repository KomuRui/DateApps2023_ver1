using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
        if ((collision.transform.tag == "Fishes" || collision.transform.tag == "GoldFishes") && collision.GetComponent<NavMeshAgent>().enabled)
            fishObj.Add(collision.gameObject);
    }

    void OnTriggerExit(Collider collision)
    {
        if ((collision.transform.tag == "Fishes" || collision.transform.tag == "GoldFishes") && collision.GetComponent<NavMeshAgent>().enabled) 
            fishObj.Remove(collision.gameObject);
    }
}

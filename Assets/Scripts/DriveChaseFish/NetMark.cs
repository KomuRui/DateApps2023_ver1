using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetMark : MonoBehaviour
{
    public Net net; //網

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision other)
    {
        //プールに当たって自分の対応しているプールなら
        if (other.transform.tag == "Pool" && transform.parent.GetComponent<PlayerNum>().playerNum == other.gameObject.GetComponent<Pool>().playerNum)
        {
            net.NetReturn();
            net.FishGoPool(other.gameObject.GetComponent<Pool>().fallPoint, other.gameObject.GetComponent<Pool>().goalPoint);
        }
    }
}

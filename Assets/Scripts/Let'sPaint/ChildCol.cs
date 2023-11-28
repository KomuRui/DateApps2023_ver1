using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCol : MonoBehaviour
{
    public LetsPaintPlayer p;
    public GameObject hitObj;      //当たってるオブジェクト
    public bool isMuteki = false;  //無敵か

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.transform.tag == "Player" && isMuteki)
        {
            hitObj = collision.gameObject;
        }
    }

    void OnTriggerExit(Collider collision)
    {
        if (isMuteki && collision.transform.tag == "Player" && hitObj == collision.gameObject)
        {
            p.ReturnAlpha();
        }

    }
}

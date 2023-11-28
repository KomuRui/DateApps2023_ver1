using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildCol : MonoBehaviour
{
    public LetsPaintPlayer p;
    public GameObject hitObj;      //�������Ă�I�u�W�F�N�g
    public bool isMuteki = false;  //���G��

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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DolphinTop : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //あたった瞬間
    void OnTriggerEnter(Collider collision) 
    {
        if (collision.gameObject.tag == "WaterFall")
        {
            // 水しぶき
            ((FishGameManager)GameManager.nowMiniGameManager).WaterEffect(transform.position);
            Debug.Log("バシャーン");
        }
    }

    
    //離れた瞬間
    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "WaterFall")
        {
            //水しぶき
            ((FishGameManager)GameManager.nowMiniGameManager).WaterUpEffect(transform.position);
            Debug.Log("バシャーン");
        }
    }
}

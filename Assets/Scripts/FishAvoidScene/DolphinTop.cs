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


    //���������u��
    void OnTriggerEnter(Collider collision) 
    {
        if (collision.gameObject.tag == "WaterFall")
        {
            // �����Ԃ�
            ((FishGameManager)GameManager.nowMiniGameManager).WaterEffect(transform.position);
            Debug.Log("�o�V���[��");
        }
    }

    
    //���ꂽ�u��
    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "WaterFall")
        {
            //�����Ԃ�
            ((FishGameManager)GameManager.nowMiniGameManager).WaterUpEffect(transform.position);
            Debug.Log("�o�V���[��");
        }
    }
}

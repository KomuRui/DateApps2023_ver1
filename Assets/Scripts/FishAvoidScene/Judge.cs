using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judge : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player3;
    //FishCountDown playerscript; //�ĂԃX�N���v�g�ɂ����Ȃ���
    //GameObject obj = GameObject.Find("Player"); //Player���Ă����I�u�W�F�N�g��T��
    //playerscript = obj.GetComponent<FishCountDown>(); //�t���Ă���X�N���v�g���擾

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //��������Ȃ�
        if(Player1 == null && Player2 == null && Player3 == null)
        {

        }
    }
}

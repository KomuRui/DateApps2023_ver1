using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judge : MonoBehaviour
{
    public GameObject Player1;
    public GameObject Player2;
    public GameObject Player3;
    //FishCountDown playerscript; //呼ぶスクリプトにあだなつける
    //GameObject obj = GameObject.Find("Player"); //Playerっていうオブジェクトを探す
    //playerscript = obj.GetComponent<FishCountDown>(); //付いているスクリプトを取得

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //だれもいない
        if(Player1 == null && Player2 == null && Player3 == null)
        {

        }
    }
}

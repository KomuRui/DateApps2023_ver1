using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishDisplay : MonoBehaviour
{
    [SerializeField] private List<GameObject> playerList;

    public GameObject endText;
    public GameObject[] ranking;
    public List<int> temRanking;

    //FishCountDown fishCountDown;
    GameObject obj;
    
    // Start is called before the first frame update
    void Start()
    {
        obj = GameObject.Find("GameManager");
        //fishCountDown = obj.GetComponent<FishCountDown>();
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーが全滅しているか
        bool isAllDead = true;
        foreach (var player in playerList)
        {
            //生きているプレイヤーがいたら全滅していない
            if (player != null)
            {
                isAllDead = false;
                break;
            }
        }

        //だれもいないか制限時間が過ぎていたら
        if (isAllDead)
        {
            Instantiate(endText, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }
}
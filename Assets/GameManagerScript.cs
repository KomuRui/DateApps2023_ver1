using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public TextMeshProUGUI notification;
    [SerializeField] private List<ConsecutivePlayer> playerList = new List<ConsecutivePlayer>(); //プレイヤーのリスト

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < playerList.Count; i++)
        {
            //もしゴールしていたら
            if(playerList[i].goolFlag == true)
            {
                notification.text = i+ 2 + "P Goal";
            }
        }

        int deadNum = 0;
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].isDead == true)
            {
                deadNum++;
            }
        }

        //もし全員死んでいたら
        if(deadNum == playerList.Count)
        {
            notification.text = "ALL DEAD";
        }
    }
}

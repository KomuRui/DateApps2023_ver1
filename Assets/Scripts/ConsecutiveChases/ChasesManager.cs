using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class ChasesManager : MiniGameManager
{
    //ゴールした順番、（番号が若い順で早い）
    private List<byte> goalPlayer;

    [SerializeField] List<ConsecutivePlayer> threePlayerList;

    //Startと同じ
    public override void SceneStart()
    {

    }

    //Updateと同じ
    public override void MiniGameUpdate()
    {
        ///////////a版のみ////////////////

        bool flag = true;
        foreach (var item in threePlayerList)
        {
            if (!item.isGoal && !item.isDead)
            {
                flag = false;
            }
        }

        if (flag)
        {
            GameManager.nowMiniGameManager.SetMiniGameFinish();
        }
        //////////////////////////////////
    }

    //ミニゲームが終わったら呼ばれる
    public override void MiniGameFinish()
    {

        
        //ランキングをつける
        //Ranking();
    }

    //プレイヤーにランクをつける
    public void Ranking()
    {
        ///////////順位を決める////////////////
        bool onePlayerWin = false;

        //<Player番号、距離>
        var threeRankMiss = new Dictionary<byte, float>();
        List<byte> threeRankResult = new List<byte>();

        foreach (GameObject player in threePlayerObj)
        {
            //全員倒していたら
            if (goalPlayer == null)
            {
                //1Pプレイヤーを勝ちにする
                onePlayerWin = true;
            }

            //ゴールしていなかったら
            if(!player.GetComponent<ConsecutivePlayer>().isGoal)
            {
                threeRankMiss.Add(player.GetComponent<PlayerNum>().playerNum, player.GetComponent<ConsecutivePlayer>().transform.position.z);
            }
        }

        //ソートする
        var sortedDictionary = threeRankMiss.OrderByDescending(pair => pair.Value);

        ///////////ランキングをつける//////////

        //ゴールした人達に順位をつける
        for (int i = 0; i < threeRankResult.Count; i++)
        {
            threeRankResult.Add(goalPlayer[i]);
        }

        //ゴールしてない人に順位をつける
        foreach (var item in sortedDictionary)
        {
            threeRankResult.Add(item.Key);
        }

        byte oneWin = 1;
        //1Pが勝っていたら
        if (onePlayerWin)
        {
            //1Pの順位を確定
            ScoreManager.AddScore(onePlayerObj.GetComponent<PlayerNum>().playerNum, 1);
            oneWin += 1;
        }
        else
        {
            //1Pの順位を確定
            ScoreManager.AddScore(onePlayerObj.GetComponent<PlayerNum>().playerNum, 4);
        }

        
        //3P側の順位を確定
        for (byte i = 0; i < threeRankResult.Count; i++) 
        {
            ScoreManager.AddScore(threeRankResult[i], (byte)(i + oneWin));
        }
    }

    //プレイヤーがゴールした時に呼ぶ関数
    public void PlayerGoal(byte playerNum)
    {
        goalPlayer.Add(playerNum);
    }
}

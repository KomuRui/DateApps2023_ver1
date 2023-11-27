using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class ChasesManager : MiniGameManager
{
    //ゴールした順番、（番号が若い順で早い）
    private List<byte> goalPlayer;

    //Startと同じ
    public override void SceneStart()
    {

    }

    //Updateと同じ
    public override void MiniGameUpdate()
    {
    }

    //ミニゲームが終わったら呼ばれる
    public override void MiniGameFinish()
    {
        //ランキングをつける
        //ScoreManager.AddScore();
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

        ////死んだ人に順位をつける
        //byte nowRank = (onePlayerWin ? (byte)1 : (byte)0);
        //byte sameRank = 1;
        //byte lookNum = 1;
        //float beforeValue = -1;
        //foreach (var item in threeRankResult)
        //{
        //    if (beforeValue != item.Value)
        //    {
        //        nowRank += sameRank;
        //        sameRank = 1;
        //    }
        //    else
        //        sameRank++;

            //    beforeValue = item.Value;
            //    ScoreManager.AddScore(threePlayerObj[item.Key - 1].GetComponent<PlayerNum>().playerNum, nowRank);
            //    lookNum++;
            //}


            //    int threePlayer = playerPercent[1] + playerPercent[2] + playerPercent[3];
            //    int onePlayer = playerPercent[0];

            //    bool isWinOnePLayer = false;

            //    //1人側が勝ったのなら
            //    if (threePlayer <= onePlayer)
            //    {
            //        ScoreManager.AddScore(onePlayerObj.GetComponent<PlayerNum>().playerNum, 1);
            //        isWinOnePLayer = true;
            //    }
            //    else
            //        ScoreManager.AddScore(onePlayerObj.GetComponent<PlayerNum>().playerNum, 4);

            //    //3人側の得点をソートで並び変える
            //    var dict = new Dictionary<int, int>();
            //    for (int i = 1; i < playerPercent.Length; i++)
            //        dict.Add(i, playerPercent[i]);

            //    var sortedDictionary = dict.OrderByDescending(pair => pair.Value);

            //    //順位を確認
            //    byte nowRank = (isWinOnePLayer ? (byte)1 : (byte)0);
            //    byte sameRank = 1;
            //    byte lookNum = 1;
            //    float beforeValue = -1;
            //    foreach (var item in sortedDictionary)
            //    {
            //        if (beforeValue != item.Value)
            //        {
            //            nowRank += sameRank;
            //            sameRank = 1;
            //        }
            //        else
            //            sameRank++;

            //        beforeValue = item.Value;
            //        ScoreManager.AddScore(threePlayerObj[item.Key - 1].GetComponent<PlayerNum>().playerNum, nowRank);
            //        lookNum++;
            //    }
            //}
    }

    //プレイヤーがゴールした時に呼ぶ関数
    public void PlayerGoal(byte playerNum)
    {
        goalPlayer.Add(playerNum);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//using static UnityEditor.Progress;

public class NotFallHoleGameManager : MiniGameManager
{
    //床の開いてる数
    public int openCount;
    public AllParentRotate rotate;
    public List<FallRotateFloor> Floor;

    public override void SceneStart()
    {
        openCount = 0;
        rotate.playerNum = onePlayerObj.GetComponent<PlayerNum>().playerNum;

        for (int i = 0; i < Floor.Count; i++)
            Floor[i].SetPlayerNum(onePlayerObj.GetComponent<PlayerNum>().playerNum);

    }

    public override void MiniGameUpdate()
    {
    }


    //ゲーム終了時に呼ばれる
    public override void MiniGameFinish()
    {
        
        //１人側が勝ったかどうか
        bool isWinOnePLayer = false;

        //プレイヤーがすべて死んでいるのなら
        if (isPlayerAllDead)
        {
            ScoreManager.AddScore(onePlayerObj.GetComponent<PlayerNum>().playerNum, 1);
            isWinOnePLayer = true;
        }
        else
            ScoreManager.AddScore(onePlayerObj.GetComponent<PlayerNum>().playerNum, 4);

        //順位を確認
        byte nowRank = (isWinOnePLayer ? (byte)2 : (byte)1);
        byte sameRank = 0;

        //生き残っている人に順位をつける
        foreach (var player in threePlayer)
        {
            //生きているのなら
            if (!player.Value)
            {
                ScoreManager.AddScore(player.Key, nowRank);
                sameRank++;
            }
        }

        //3人側の得点をソートで並び変える
        var sortedDictionary = lifeTime.OrderByDescending(pair => pair.Value);
        float beforeValue = -1;
        foreach (var item in sortedDictionary)
        {
            //生きているのならこの先処理しない
            if (!threePlayer[item.Key]) continue;

            //前回の値と違うのならば
            if (beforeValue != item.Value)
            {
                nowRank += sameRank;
                sameRank = 1;
            }
            else
                sameRank++;

            beforeValue = item.Value;
            ScoreManager.AddScore(item.Key, nowRank);
        }
    }

    ////////////////////////////////ゲーム中に必要なもの///////////////////////////////

    public int GetCount() { return openCount; }

    public bool AddCount() 
    {
        if (openCount < 6)
        {
            openCount++;
            return true;
        }
        else
            return false;
    }

    public void MinusCount() { openCount--; openCount = Math.Max(0, openCount); }
}

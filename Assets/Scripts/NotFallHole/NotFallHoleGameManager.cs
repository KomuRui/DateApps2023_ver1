using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NotFallHoleGameManager : MiniGameManager
{
    //���̊J���Ă鐔
    public int openCount;

    public override void SceneStart()
    {
        openCount = 0;
    }

    public override void MiniGameUpdate()
    {
    }


    //�Q�[���I�����ɌĂ΂��
    public override void MiniGameFinish()
    {
        //int threePlayer = playerPercent[1] + playerPercent[2] + playerPercent[3];
        //int onePlayer = playerPercent[0];

        //bool isWinOnePLayer = false;

        ////1�l�����������̂Ȃ�
        //if (threePlayer <= onePlayer)
        //{
        //    ScoreManager.AddScore(onePlayerObj.GetComponent<PlayerNum>().playerNum, 1);
        //    isWinOnePLayer = true;
        //}
        //else
        //    ScoreManager.AddScore(onePlayerObj.GetComponent<PlayerNum>().playerNum, 4);

        ////3�l���̓��_���\�[�g�ŕ��ѕς���
        //var dict = new Dictionary<int, int>();
        //for (int i = 1; i < playerPercent.Length; i++)
        //    dict.Add(i, playerPercent[i]);

        //var sortedDictionary = dict.OrderByDescending(pair => pair.Value);

        ////���ʂ��m�F
        //byte nowRank = (isWinOnePLayer ? (byte)1 : (byte)0);
        //byte sameRank = 1;
        //byte lookNum = 1;
        //float beforeValue = -1;
        //foreach (var item in sortedDictionary)
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
    }

    ////////////////////////////////�Q�[�����ɕK�v�Ȃ���///////////////////////////////

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

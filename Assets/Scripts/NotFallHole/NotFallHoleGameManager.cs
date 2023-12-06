using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//using static UnityEditor.Progress;

public class NotFallHoleGameManager : MiniGameManager
{
    //���̊J���Ă鐔
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


    //�Q�[���I�����ɌĂ΂��
    public override void MiniGameFinish()
    {
        
        //�P�l�������������ǂ���
        bool isWinOnePLayer = false;

        //�v���C���[�����ׂĎ���ł���̂Ȃ�
        if (isPlayerAllDead)
        {
            ScoreManager.AddScore(onePlayerObj.GetComponent<PlayerNum>().playerNum, 1);
            isWinOnePLayer = true;
        }
        else
            ScoreManager.AddScore(onePlayerObj.GetComponent<PlayerNum>().playerNum, 4);

        //���ʂ��m�F
        byte nowRank = (isWinOnePLayer ? (byte)2 : (byte)1);
        byte sameRank = 0;

        //�����c���Ă���l�ɏ��ʂ�����
        foreach (var player in threePlayer)
        {
            //�����Ă���̂Ȃ�
            if (!player.Value)
            {
                ScoreManager.AddScore(player.Key, nowRank);
                sameRank++;
            }
        }

        //3�l���̓��_���\�[�g�ŕ��ѕς���
        var sortedDictionary = lifeTime.OrderByDescending(pair => pair.Value);
        float beforeValue = -1;
        foreach (var item in sortedDictionary)
        {
            //�����Ă���̂Ȃ炱�̐揈�����Ȃ�
            if (!threePlayer[item.Key]) continue;

            //�O��̒l�ƈႤ�̂Ȃ��
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

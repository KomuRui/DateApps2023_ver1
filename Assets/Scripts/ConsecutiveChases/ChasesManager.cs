using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public class ChasesManager : MiniGameManager
{
    //�S�[���������ԁA�i�ԍ����Ⴂ���ő����j
    private List<byte> goalPlayer;

    //Start�Ɠ���
    public override void SceneStart()
    {

    }

    //Update�Ɠ���
    public override void MiniGameUpdate()
    {
    }

    //�~�j�Q�[�����I�������Ă΂��
    public override void MiniGameFinish()
    {
        //�����L���O������
        //ScoreManager.AddScore();
    }

    //�v���C���[�Ƀ����N������
    public void Ranking()
    {
        ///////////���ʂ����߂�////////////////
        bool onePlayerWin = false;

        //<Player�ԍ��A����>
        var threeRankMiss = new Dictionary<byte, float>();
        List<byte> threeRankResult = new List<byte>();

        foreach (GameObject player in threePlayerObj)
        {
            //�S���|���Ă�����
            if (goalPlayer == null)
            {
                //1P�v���C���[�������ɂ���
                onePlayerWin = true;
            }

            //�S�[�����Ă��Ȃ�������
            if(!player.GetComponent<ConsecutivePlayer>().isGoal)
            {
                threeRankMiss.Add(player.GetComponent<PlayerNum>().playerNum, player.GetComponent<ConsecutivePlayer>().transform.position.z);
            }
        }

        //�\�[�g����
        var sortedDictionary = threeRankMiss.OrderByDescending(pair => pair.Value);

        ///////////�����L���O������//////////

        //�S�[�������l�B�ɏ��ʂ�����
        for (int i = 0; i < threeRankResult.Count; i++)
        {
            threeRankResult.Add(goalPlayer[i]);
        }

        //�S�[�����ĂȂ��l�ɏ��ʂ�����
        foreach (var item in sortedDictionary)
        {
            threeRankResult.Add(item.Key);
        }

        byte oneWin = 1;
        //1P�������Ă�����
        if (onePlayerWin)
        {
            //1P�̏��ʂ��m��
            ScoreManager.AddScore(onePlayerObj.GetComponent<PlayerNum>().playerNum, 1);
            oneWin += 1;
        }
        else
        {
            //1P�̏��ʂ��m��
            ScoreManager.AddScore(onePlayerObj.GetComponent<PlayerNum>().playerNum, 4);
        }

        
        //3P���̏��ʂ��m��
        for (byte i = 0; i < threeRankResult.Count; i++) 
        {
            ScoreManager.AddScore(threeRankResult[i], (byte)(i + oneWin));
        }

        ////���񂾐l�ɏ��ʂ�����
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

            //    //1�l�����������̂Ȃ�
            //    if (threePlayer <= onePlayer)
            //    {
            //        ScoreManager.AddScore(onePlayerObj.GetComponent<PlayerNum>().playerNum, 1);
            //        isWinOnePLayer = true;
            //    }
            //    else
            //        ScoreManager.AddScore(onePlayerObj.GetComponent<PlayerNum>().playerNum, 4);

            //    //3�l���̓��_���\�[�g�ŕ��ѕς���
            //    var dict = new Dictionary<int, int>();
            //    for (int i = 1; i < playerPercent.Length; i++)
            //        dict.Add(i, playerPercent[i]);

            //    var sortedDictionary = dict.OrderByDescending(pair => pair.Value);

            //    //���ʂ��m�F
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

    //�v���C���[���S�[���������ɌĂԊ֐�
    public void PlayerGoal(byte playerNum)
    {
        goalPlayer.Add(playerNum);
    }
}

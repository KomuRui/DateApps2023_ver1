using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class ChasesManager : MiniGameManager
{
    //�S�[���������ԁA�i�ԍ����Ⴂ���ő����j
    private List<byte> goalPlayer;

    [SerializeField] List<ConsecutivePlayer> threePlayerList;

    //Start�Ɠ���
    public override void SceneStart()
    {

    }

    //Update�Ɠ���
    public override void MiniGameUpdate()
    {
        ///////////a�ł̂�////////////////

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

    //�~�j�Q�[�����I�������Ă΂��
    public override void MiniGameFinish()
    {

        
        //�����L���O������
        //Ranking();
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
    }

    //�v���C���[���S�[���������ɌĂԊ֐�
    public void PlayerGoal(byte playerNum)
    {
        goalPlayer.Add(playerNum);
    }
}

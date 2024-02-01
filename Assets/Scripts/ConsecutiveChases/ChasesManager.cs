using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
//using static UnityEditor.Progress;

public class ChasesManager : MiniGameManager
{
    //�S�[���������ԁA�i�ԍ����Ⴂ���ő����j
    public List<byte> goalPlayer;

    [SerializeField] private GameObject canvas;

    public List<Image> onePlayerNextCommandImageList = new List<Image>();
    public List<Image> threePlayerNextCommandImageList = new List<Image>();
    [SerializeField] private GameObject camera;
    [SerializeField] private OneSide_UI oneUI;



    //Start�Ɠ���
    public override void SceneStart()
    {
        canvas.SetActive(true);

        onePlayerObj.GetComponent<ChasesPlayer>().nextCommandImageList = onePlayerNextCommandImageList;

        for(int i = 0; i < threePlayerNextCommandImageList.Count; i++) 
        {
            threePlayerNextCommandImageList[i].GetComponent<ThreeSide_UI>().player = threePlayerObj[i];
            threePlayerNextCommandImageList[i].GetComponent<ThreeSide_UI>().SetPosition();
        }

        for(int i = 0; i < threePlayerObj.Count; i++) 
        {
            threePlayerObj[i].GetComponent<ConsecutivePlayer>().nextCommandImage = threePlayerNextCommandImageList;
            threePlayerObj[i].GetComponent<ConsecutivePlayer>().threePlayerNum = i;
        }

        //foreach (var player in threePlayerObj)
        //{
        //    player.GetComponent<ConsecutivePlayer>().nextCommandImage = threePlayerNextCommandImageList;
        //}

        camera.GetComponent<CameraController>().playerList.Add(onePlayerObj);
        foreach (var player in threePlayerObj)
        {
            camera.GetComponent<CameraController>().playerList.Add(player);
        }

        oneUI.player = onePlayerObj;
    }

    //�Q�[���J�n���ɌĂ΂��
    public override void MiniGameStart()
    {
    }

    //Update�Ɠ���
    public override void MiniGameUpdate()
    {
        ///////////a�ł̂�////////////////
        bool flag = true;
        foreach (var item in threePlayerObj)
        {
            if (item != null &&!item.GetComponent<ConsecutivePlayer>().isGoal && !item.GetComponent<ConsecutivePlayer>().isDead)
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
        Ranking();
    }

    //�v���C���[�Ƀ����N������
    public void Ranking()
    {
        ///////////���ʂ����߂�////////////////
        bool onePlayerWin = false;

        //<Player�ԍ��A����>
        var threeRankMiss = new Dictionary<byte, float>();
        List<Dictionary<byte, float>> threeRankResult = new List<Dictionary<byte, float>>();

        foreach (GameObject player in threePlayerObj)
        {
            //�S���|���Ă�����
            if (goalPlayer.Count() == 0)
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

        byte oneWin = 0;
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

        byte rank = oneWin;
        //�S�[�������l�B�ɏ��ʂ�����
        for (int i = 0; i < goalPlayer.Count(); i++)
        {
            rank++;
            ScoreManager.AddScore(goalPlayer[i], rank);
        }

        //�S�[�����ĂȂ��l�ɏ��ʂ�����

        float tmp = -9999;
        byte tai = 0;
        foreach (var item in sortedDictionary)
        {
            rank++;
            if (tmp != (float)item.Value)
            {
                ScoreManager.AddScore(item.Key, (byte)(rank - tai));
                tmp = (float)item.Value;
                tai = 0;
            }
            else
            {
                tai++;
                ScoreManager.AddScore(item.Key, (byte)(rank - tai));
            }
        }
    }

    //�v���C���[���S�[���������ɌĂԊ֐�
    public void PlayerGoal(byte playerNum)
    {
        goalPlayer.Add(playerNum);
    }
}

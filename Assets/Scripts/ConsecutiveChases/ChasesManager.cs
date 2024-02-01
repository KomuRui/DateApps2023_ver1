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
    //ゴールした順番、（番号が若い順で早い）
    public List<byte> goalPlayer;

    [SerializeField] private GameObject canvas;

    public List<Image> onePlayerNextCommandImageList = new List<Image>();
    public List<Image> threePlayerNextCommandImageList = new List<Image>();
    [SerializeField] private GameObject camera;
    [SerializeField] private OneSide_UI oneUI;



    //Startと同じ
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

    //ゲーム開始時に呼ばれる
    public override void MiniGameStart()
    {
    }

    //Updateと同じ
    public override void MiniGameUpdate()
    {
        ///////////a版のみ////////////////
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

    //ミニゲームが終わったら呼ばれる
    public override void MiniGameFinish()
    {
        //ランキングをつける
        Ranking();
    }

    //プレイヤーにランクをつける
    public void Ranking()
    {
        ///////////順位を決める////////////////
        bool onePlayerWin = false;

        //<Player番号、距離>
        var threeRankMiss = new Dictionary<byte, float>();
        List<Dictionary<byte, float>> threeRankResult = new List<Dictionary<byte, float>>();

        foreach (GameObject player in threePlayerObj)
        {
            //全員倒していたら
            if (goalPlayer.Count() == 0)
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

        byte oneWin = 0;
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

        byte rank = oneWin;
        //ゴールした人達に順位をつける
        for (int i = 0; i < goalPlayer.Count(); i++)
        {
            rank++;
            ScoreManager.AddScore(goalPlayer[i], rank);
        }

        //ゴールしてない人に順位をつける

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

    //プレイヤーがゴールした時に呼ぶ関数
    public void PlayerGoal(byte playerNum)
    {
        goalPlayer.Add(playerNum);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DeathRunGameManager : MiniGameManager
{
    [SerializeField] private Vector3 goal2DPos;
    [SerializeField] private Vector3 start2DPos;
    [SerializeField] private Vector3 goal3DPos;
    [SerializeField] private Vector3 start3DPos;
    [SerializeField] private GameObject startToGoalCanvas;
    [NonSerialized] public List<byte> goalPlayer = new List<byte>();

    private float startToGoalDis3D;
    private float startToGoalDis2D;

    //シーン開始
    public override void SceneStart() 
    {
        //各距離を求める
        startToGoalDis3D = goal3DPos.z - start3DPos.z;
        startToGoalDis2D = goal2DPos.y - start2DPos.y;
        
        //チュートリアルが終了しているのなら
        if(TutorialManager.isTutorialFinish)
            startToGoalCanvas.SetActive(true);
    }

    //更新
    public override void MiniGameUpdate() 
    {
        //開始していないか終わっているのなら
        if (!GameManager.nowMiniGameManager.IsStart() || GameManager.nowMiniGameManager.IsFinish()) return;

        //画像の位置を変える
        for (int i = 0; i < threePlayerImage.Count; i++)
        {
            if (threePlayerObj[i] == null) continue;

            //画像の位置を求める
            float dis3D = threePlayerObj[i].transform.position.z - start3DPos.z;
            float ratio = dis3D / startToGoalDis3D;
            threePlayerImage[i].transform.localPosition = new Vector3(threePlayerImage[i].transform.localPosition.x,
                                                                      start2DPos.y + (startToGoalDis2D * ratio),
                                                                      threePlayerImage[i].transform.localPosition.z);
        }

    }

    //ゲーム終了時に呼ばれる
    public override void MiniGameFinish()
    {

        //１人側が勝ったかどうか
        bool isWinOnePLayer = false;

        //ゴールしたプレイヤーがいないのなら
        if (goalPlayer.Count <= 0)
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
        foreach (var player in goalPlayer)
        {
            ScoreManager.AddScore(player, nowRank);
            nowRank++;
        }

        Dictionary<byte, float> deadDis = new Dictionary<byte, float>();
        int lookNum = 0;
        foreach (var item in threePlayer)
        {
            deadDis[item.Key] = threePlayerImage[lookNum].transform.localPosition.y - start2DPos.y;
            lookNum++;
        }

        //3人側の得点をソートで並び変える
        var sortedDictionary = deadDis.OrderByDescending(pair => pair.Value);
        float beforeValue = -1;
        foreach (var item in sortedDictionary)
        {
            //ゴールしているのならこの先処理しない
            if (goalPlayer.Contains(item.Key)) continue;

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

}

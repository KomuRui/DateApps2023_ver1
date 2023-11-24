using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class LetsPaintGameManager : MiniGameManager
{
    [SerializeField] private PaintTarget target;
    [SerializeField] private TextMeshProUGUI[] playerPercentText; //パーセント

    private int[] playerPercent;

    //更新
    public override void MiniGameUpdate() 
    {
        //各プレイヤーのパーセント計算
        playerPercentCalc();
    }

    //各プレイヤーのパーセント計算
    private void playerPercentCalc()
    {
        playerPercent = target.GetPercent(target);
        for (int i = 0; i < playerPercentText.Length; i++)
            playerPercentText[i].SetText(playerPercent[i].ToString() + "%");
    }

    //ゲーム終了時に呼ばれる
    public override void MiniGameFinish()
    {
        int threePlayer = playerPercent[1] + playerPercent[2] + playerPercent[3];
        int onePlayer = playerPercent[0];

        bool isWinOnePLayer = false;

        //1人側が勝ったのなら
        if (threePlayer <= onePlayer)
        {
            ScoreManager.AddScore(onePlayerObj.GetComponent<PlayerNum>().playerNum, 1);
            isWinOnePLayer = true;
        }
        else
            ScoreManager.AddScore(onePlayerObj.GetComponent<PlayerNum>().playerNum, 4);

        //3人側の得点をソートで並び変える
        var dict = new Dictionary<int, int>();
        for (int i = 1; i < playerPercent.Length; i++)
            dict.Add(i, playerPercent[i]);

        var sortedDictionary = dict.OrderByDescending(pair => pair.Value);

        //順位を確認
        byte nowRank = (isWinOnePLayer ? (byte)1 : (byte)0);
        byte sameRank = 1;
        byte lookNum = 1;
        float beforeValue = -1;
        foreach (var item in sortedDictionary)
        {
            if (beforeValue != item.Value)
            {
                nowRank += sameRank;
                sameRank = 1;
            }
            else 
                sameRank++;

            beforeValue = item.Value;
            ScoreManager.AddScore(threePlayerObj[item.Key - 1].GetComponent<PlayerNum>().playerNum, nowRank);
            lookNum++;
        }
    }
}

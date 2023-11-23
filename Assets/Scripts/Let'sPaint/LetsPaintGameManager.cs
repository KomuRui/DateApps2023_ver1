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


    // Update is called once per frame
    void Update()
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
        int threePlayer = playerPercent[0] + playerPercent[1] + playerPercent[2];
        int onePlayer = playerPercent[3];

        bool isWinOnePLayer = false;

        //1人側が勝ったのなら
        if (threePlayer <= onePlayer)
        {
            ScoreManager.AddScore(onePlayerParent.GetComponent<PlayerNum>().playerNum, 1);
            isWinOnePLayer = true;
        }
        else
            ScoreManager.AddScore(onePlayerParent.GetComponent<PlayerNum>().playerNum, 4);

        //3人側の得点をソートで並び変える
        var dict = new Dictionary<int, int>();
        for (int i = 0; i < playerPercent.Length; i++)
            dict.Add(i, playerPercent[i]);

        IOrderedEnumerable<KeyValuePair<int, int>> sorted = dict.OrderByDescending(pair => pair.Key);

        //順位を確認
        byte nowRank = (isWinOnePLayer ? (byte)1 : (byte)0);
        byte sameRank = 1;
        float beforeValue = -1;
        for (int i = 0; i < dict.Count; i++)
        {
            if (beforeValue != dict[i])
            {
                nowRank += sameRank;
                sameRank = 1;
            }
            else 
                sameRank++;

            beforeValue = dict[i];
            ScoreManager.AddScore(onePlayerParent.GetComponent<PlayerNum>().playerNum, nowRank);
        }
    }
}

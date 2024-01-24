using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ReachExtenderManager : MiniGameManager
{
    [SerializeField] VortexManager vortexManager;

    //Startと同じ
    public override void SceneStart()
    {
    }

    //ゲーム開始時に呼ばれる
    public override void MiniGameStart()
    {
        vortexManager.StartVortexCoroutine();
    }

    //Updateと同じ
    public override void MiniGameUpdate()
    {
    }

    //ミニゲームが終わったら呼ばれる
    public override void MiniGameFinish()
    {
        vortexManager.SetIsAppearanceVotex(false);

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
}

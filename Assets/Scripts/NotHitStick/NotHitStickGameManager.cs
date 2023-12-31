using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NotHitStickGameManager : MiniGameManager
{
    public int kill = 0;
    public Rotate1 rotate1;
    public Rotate2 rotate2;
    public GameObject[] floor;
    public GameObject hitEffectParent;
    public GameObject tyakutiEffectParent;

    public override void SceneStart()
    {
        rotate1.playerNum = onePlayerObj.GetComponent<PlayerNum>().playerNum;
        rotate2.playerNum = onePlayerObj.GetComponent<PlayerNum>().playerNum;

        int i = 2;
        foreach (var player in threePlayerObj)
        {
            player.GetComponent<NotHitStickPlayer>().stage = floor;
            player.GetComponent<NotHitStickPlayer>().nowStageNum = i;
            i--;
        }
    }

    public override void MiniGameUpdate()
    {
    }

    //ゲーム終了時に呼ばれる
    public override void MiniGameFinish()
    {

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

    //着地エフェクトを表示
    public void tyakutiEffect(Vector3 pos)
    {
        GameObject ef = null;

        for (int i = 0; i < tyakutiEffectParent.transform.childCount; i++)
        {
            if (!tyakutiEffectParent.transform.GetChild(i).gameObject.activeSelf)
            {
                ef = tyakutiEffectParent.transform.GetChild(i).gameObject;
                break;
            }
        }

        if (ef != null)
        {
            ef.transform.position = pos;
            ef.SetActive(true);
            ef.GetComponent<ParticleSystem>().Play();
        }
    }

    //プレイヤー踏んだ時のエフェクトを表示
    public void hitEffect(Vector3 pos)
    {
        GameObject ef = null;

        for (int i = 0; i < hitEffectParent.transform.childCount; i++)
        {
            if (!hitEffectParent.transform.GetChild(i).gameObject.activeSelf)
            {
                ef = hitEffectParent.transform.GetChild(i).gameObject;
                break;
            }
        }

        if (ef != null)
        {
            ef.transform.position = pos;
            ef.SetActive(true);
            ef.GetComponent<ParticleSystem>().Play();
        }
    }

}

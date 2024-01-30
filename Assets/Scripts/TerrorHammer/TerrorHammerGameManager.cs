using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrorHammerGameManager : MiniGameManager
{
    [NonSerialized] public List<byte> goalPlayer = new List<byte>();
    public GameObject hitEffectParent;

    /// Start is called before the first frame update
    public override void SceneStart()
    {

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

        //生き残っている人に順位をつける
        foreach (var player in goalPlayer)
        {
            ScoreManager.AddScore(player, nowRank);
            nowRank++;
        }

        //3人側の得点をソートで並び変える
        foreach (var item in threePlayer)
        {
            //ゴールしているのならこの先処理しない
            if (goalPlayer.Contains(item.Key)) continue;

            //スコア加算
            ScoreManager.AddScore(item.Key, nowRank);
        }
    }

    //ハンマーのヒットエフェクトを表示
    public void HammerHitEffect(Vector3 pos)
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
       
        if(ef != null)
        {
            ef.transform.position = pos;
            ef.SetActive(true);
            ef.GetComponent<ParticleSystem>().Play();
        }
    }
}

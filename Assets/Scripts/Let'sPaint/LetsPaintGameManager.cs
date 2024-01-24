using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class LetsPaintGameManager : MiniGameManager
{
    [SerializeField] private PaintTarget target;
    [SerializeField] private GameObject floor;
    public GameObject splashEffectParent;
    public GameObject hitEffectParent;
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
        //テクスチャ取得
        Material m = floor.GetComponent<Renderer>().material;
        Texture t = m.GetTexture("BaseMap");

        //ピクセルごとの色と総ピクセル数を取得
        //Color[] pixels = t;
        //int totalPixels = pixels.Length;

        // playerPercent = target.GetPercent(target);
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

    //海に落ちた時のエフェクトを表示
    public void SplashEffect(Vector3 pos)
    {
        GameObject ef = null;

        for (int i = 0; i < splashEffectParent.transform.childCount; i++)
        {
            if (!splashEffectParent.transform.GetChild(i).gameObject.activeSelf)
            {
                ef = splashEffectParent.transform.GetChild(i).gameObject;
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

    //当たった時のエフェクトを表示
    public void HitEffect(Vector3 pos)
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

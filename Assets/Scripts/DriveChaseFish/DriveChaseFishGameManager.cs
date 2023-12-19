using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class DriveChaseFishGameManager : MiniGameManager
{
    public Dictionary<int, int> fiscScore = new Dictionary<int, int>();
    public FishManager fishManager;
    public List<Pool> pool = new List<Pool>();
    public List<MeshRenderer> poolImageOnePlayerMesh = new List<MeshRenderer>();
    public List<MeshRenderer> poolImageThreePlayerMesh = new List<MeshRenderer>();


    //シーン開始
    public override void SceneStart() {

        //プレイヤーの番号と各プールの番号が同じになるように適用
        pool[0].playerNum = onePlayerObj.GetComponent<PlayerNum>().playerNum;
        for(int i = 0; i < threePlayerObj.Count; i++)
        {
            pool[i + 1].playerNum = threePlayerObj[i].GetComponent<PlayerNum>().playerNum;
        }

        //各プールのプレイヤー画像を適用
        for(int i = 0; i < poolImageOnePlayerMesh.Count; i++)
        {
            poolImageOnePlayerMesh[i].material.mainTexture = Resources.Load<Texture>(PlayerManager.GetPlayerVisualImage((byte)pool[0].playerNum));
            poolImageThreePlayerMesh[i].material.mainTexture = Resources.Load<Texture>(PlayerManager.GetPlayerVisualImage((byte)pool[i + 1].playerNum));
        }
    }

    //ミニゲームスタート
    public override void MiniGameStart()
    {
        //各プレイヤーの得点初期化
        for (int i = 0; i < PlayerManager.PLAYER_MAX; i++)
            fiscScore[i + 1] = 0;
    }

    //更新
    public override void MiniGameUpdate()
    {
    }

    //ゲーム終了時に呼ばれる
    public override void MiniGameFinish()
    {
        int threePlayer = fiscScore[pool[1].playerNum] + fiscScore[pool[2].playerNum] + fiscScore[pool[3].playerNum];
        int onePlayer = fiscScore[pool[0].playerNum];

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
        for (int i = 1; i < pool.Count; i++)
            dict.Add(pool[i].playerNum, fiscScore[pool[i].playerNum]);

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
            ScoreManager.AddScore((byte)item.Key, nowRank);
            lookNum++;
        }
    }

    //得点加算
    public void FishScorePlus(int playerNum,int fishSum) { fiscScore[playerNum] += fishSum; }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//プレイヤランクー情報
public class PlayerRank
{
    public byte rank; //順位
    public int score; //ポイント数
}

public static class ScoreManager
{
    //プレイヤースコア(key : プレイヤー番号)
    private static Dictionary<byte, PlayerRank> score;

    //前回のプレイヤースコア
    private static Dictionary<byte, PlayerRank> beforeScore;

    //スコア表(順位によって何ポイント取得するか)
    private static Dictionary<byte, byte> scoreTable;

    //初期化
    public static void Initializ()
    {
        score = new Dictionary<byte, PlayerRank>();
        beforeScore = new Dictionary<byte, PlayerRank>();
        scoreTable = new Dictionary<byte, byte>();

        for (byte i = 1; i < PlayerManager.PLAYER_MAX + 1; i++)
        {
            PlayerRank rank = new PlayerRank();
            rank.rank = 1;
            rank.score = 0;
            score[i] = rank;

            PlayerRank beforeRank = new PlayerRank();
            beforeRank.rank = 1;
            beforeRank.score = 0;
            beforeScore[i] = beforeRank;
        }

        //score[1].score = 1;
        //score[2].score = 1;
        //score[3].score = 1;
        //score[4].score = 1;

        //スコア表初期化
        scoreTable[1] = 4;
        scoreTable[2] = 3;
        scoreTable[3] = 2;
        scoreTable[4] = 1;
    }

    //順位を再計算する
    public static void ReCalcRank()
    {
        //3人側の得点を降順ソートで並び変える
        var dict = new Dictionary<byte, int>();
        for (int i = 0; i < PlayerManager.PLAYER_MAX; i++)
            dict.Add(score.Keys.ElementAt(i), score.Values.ElementAt(i).score);

        var sortedDictionary = dict.OrderByDescending(pair => pair.Value);

        //順位を確認
        byte nowRank = 0;
        byte sameRank = 1;
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
            score[item.Key].rank = nowRank;
        }
    }


    //スコア取得
    public static int GetScore(byte numPlayer) { return score[numPlayer].score; }
    public static int GetBeforeScore(byte numPlayer) { return beforeScore[numPlayer].score; }

    //順位取得
    public static int GetRank(byte numPlayer) { return score[numPlayer].rank; }
   
    //任意順位のプレイヤー番号取得
    public static List<byte> GetNominatePlayerRank(byte rank) 
    {
        List<byte> a = new List<byte>();

        foreach (var player in score)
            if (player.Value.rank == rank) a.Add(player.Key);

        return a;
    }

    //順位によって獲得するポイントを取得
    public static int GetRankScore(byte numPlayer,byte rank) 
    {
        int point = scoreTable[rank];

        //1人側が1位の場合は2点加算する
        if (PlayerManager.GetOnePlayer() == numPlayer && rank == 1) point += 2;

        return point; 
    }

    //スコア加算
    public static void AddScore(byte numPlayer,byte rank) 
    {
        //前回のスコアを設定
        beforeScore[numPlayer].score = score[numPlayer].score;

        //今回のスコアを更新
        score[numPlayer].score += scoreTable[rank];

        //1人側が1位の場合は2点加算する
        if (PlayerManager.GetOnePlayer() == numPlayer && rank == 1) score[numPlayer].score += 2;

        //順位を設定
        GameManager.nowMiniGameManager.nowMiniGameRank[numPlayer] = rank;
    }

}

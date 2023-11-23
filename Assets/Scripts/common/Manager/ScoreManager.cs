using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreManager
{
    //プレイヤースコア(key : プレイヤー番号)
    private static Dictionary<byte, int> score;

    //スコア表(順位によって何ポイント取得するか)
    private static Dictionary<byte, byte> scoreTable;

    //初期化
    public static void Initializ()
    {
        score = new Dictionary<byte, int>();
        scoreTable = new Dictionary<byte, byte>();

        for (byte i = 1; i < PlayerManager.PLAYER_MAX + 1; i++)
            score[i] = 0;

        //スコア表初期化
        scoreTable[1] = 4;
        scoreTable[2] = 3;
        scoreTable[3] = 2;
        scoreTable[4] = 1;
    }

    //スコア取得
    public static int GetScore(byte numPlayer) { return score[numPlayer]; }

    //スコア加算
    public static void AddScore(byte numPlayer,byte rank) 
    {
        score[numPlayer] += scoreTable[rank];

        //1人側が1位の場合は2点加算する
        if (PlayerManager.GetOnePlayer() == numPlayer && rank == 1) score[numPlayer] += 2;

        //順位を設定
        GameManager.nowMiniGameManager.nowMiniGameRank[numPlayer] = rank;
    }

}

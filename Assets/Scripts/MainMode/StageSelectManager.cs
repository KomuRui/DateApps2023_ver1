using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class StageSelectManager
{

    private static Dictionary<GameObject, bool> stage = new Dictionary<GameObject, bool>(); //GameObject : 各ステージの画像 bool : ステージが遊ばれたかどうか
    private static int nowRound = 1; //現在のラウンド
    private const int MAX_ROUND = 4; //最大ラウンド数
    
    //初期化
    public static void Initializ()
    {
        nowRound = 1;
    }

    //現在のラウンド数を取得
    public static int GetNowRound() { return nowRound; }

    //次のラウンドへ
    public static void NextRound() 
    {
        //もう最大ラウンド数以上になっているのならばこの先処理しない
        if (nowRound >= MAX_ROUND) return;

        nowRound++; 
    }

}

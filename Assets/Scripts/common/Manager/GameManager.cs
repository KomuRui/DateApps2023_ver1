using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    public static MiniGameManager nowMiniGameManager; //現在のミニゲーム管理
    public static bool isTitleStart = false; //タイトルから始めてるかどうか
    public static bool isSubMode = false;

    //初期化
    public static void Initializ()
    {
        isTitleStart = true;
        PlayerManager.Initializ();
        ScoreManager.Initializ();
        StageSelectManager.Initializ();
    }
}

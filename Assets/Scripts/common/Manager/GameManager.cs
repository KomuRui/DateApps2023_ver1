using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{
    public static MiniGameManager nowMiniGameManager; //現在のミニゲーム管理
  
    //初期化
    public static void Initializ()
    {
        PlayerManager.Initializ();
        ScoreManager.Initializ();
        ScoreManager.Initializ();
    }
}

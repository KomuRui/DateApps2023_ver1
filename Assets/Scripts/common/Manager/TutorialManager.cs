using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public static class TutorialManager 
{

    private static Dictionary<int, bool> playerReadyOK = new Dictionary<int, bool>();
    public static float tutorialTime = 45;
    public static bool isTutorialFinish = false;

    ///test用
    public static bool isInitializOK = false;

    //初期化
    public static void Initializ()
    {
        //初期化されていたらこの先処理しない
        if (isInitializOK) return;

        //プレイヤー番号分初期化
        for (int i = 0; i < PlayerManager.PLAYER_MAX; i++)
            playerReadyOK[i + 1] = false;

        isInitializOK = true;
    }

    //更新
    public static void Update()
    {
        //押したかどうかチェックする
        if (Input.GetButtonDown("Startbutton1"))
            SetPlayerReadyOK(1);
        if (Input.GetButtonDown("Startbutton2"))
            SetPlayerReadyOK(2);
        if (Input.GetButtonDown("Startbutton3"))
            SetPlayerReadyOK(3);
        if (Input.GetButtonDown("Startbutton4"))
            SetPlayerReadyOK(4);


        //時間計測
        tutorialTime -= Time.deltaTime;
        tutorialTime = Mathf.Max(tutorialTime, 0);
        if (tutorialTime <= 1)
        {
            GameManager.nowMiniGameManager.TutorialFinish();
        }
    }


    //プレイヤーを準備OKに設定
    public static void SetPlayerReadyOK(int playerNum) { playerReadyOK[playerNum] = true; }

    //準備OKしているプレイヤーを取得
    public static int GetReadyOKSum()
    {
       int sum = 0;

        for (int i = 0; i < PlayerManager.PLAYER_MAX; i++)
            if (playerReadyOK[i + 1]) sum++;

        return sum;
    }

}

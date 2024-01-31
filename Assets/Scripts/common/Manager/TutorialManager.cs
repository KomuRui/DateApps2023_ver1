using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public static class TutorialManager 
{

    public static Dictionary<byte, bool> playerReadyOK = new Dictionary<byte, bool>();
    public static bool isTutorialFinish = false;

    ///test用
    public static bool isInitializOK = false;

    //初期化
    public static void Initializ()
    {
        //初期化されていたらこの先処理しない
        if (isInitializOK) return;

        //プレイヤー番号分初期化
        for (byte i = 0; i < PlayerManager.PLAYER_MAX; i++)
            playerReadyOK[(byte)(i + 1)] = false;

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

        //準備OKしているのなら画像を表示
        MiniGameManager mana = GameManager.nowMiniGameManager;
        int num = 0;
        byte[] three = { 0, 0, 0 };
        byte one = mana.onePlayer;
        foreach (var threePlayer in mana.threePlayer)
        {
            three[num] = threePlayer.Key;
            num++;
        }
        for (byte i = 0; i < PlayerManager.PLAYER_MAX; i++)
        {
            if (one == (byte)(i + 1) && playerReadyOK[(byte)(i + 1)]) mana.okImage[i].SetActive(true);
            if (three[0] == (byte)(i + 1) && playerReadyOK[(byte)(i + 1)]) mana.okImage[i].SetActive(true);
            if (three[1] == (byte)(i + 1) && playerReadyOK[(byte)(i + 1)]) mana.okImage[i].SetActive(true);
            if (three[2] == (byte)(i + 1) && playerReadyOK[(byte)(i + 1)]) mana.okImage[i].SetActive(true);
        }
    }


    //プレイヤーを準備OKに設定
    public static void SetPlayerReadyOK(byte playerNum) { playerReadyOK[playerNum] = true; }

    //準備OKしているプレイヤーを取得
    public static int GetReadyOKSum()
    {
       int sum = 0;

        for (byte i = 0; i < PlayerManager.PLAYER_MAX; i++)
            if (playerReadyOK[(byte)(i + 1)]) sum++;

        return sum;
    }

}

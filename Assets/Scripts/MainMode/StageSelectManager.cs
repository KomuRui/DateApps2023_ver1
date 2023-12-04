using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class StageSelectManager
{

    private static Dictionary<GameObject, bool> stage = new Dictionary<GameObject, bool>(); //GameObject : 各ステージの画像 bool : ステージが遊ばれたかどうか
    private static int nowRound = 1;                                   　     //現在のラウンド
    private const int MAX_ROUND = 4;                                   　     //最大ラウンド数
    public static List<string> notPlayminiGameSceneName = new List<string>(); //プレイしていないミニゲームの名前
    public static List<string> playMiniGameSceneName = new List<string>();    //すでにプレイしているミニゲームの名前
    public static string nowMiniGameName;
    public static bool isMainModeFinish;

    //初期化
    public static void Initializ()
    {
        nowMiniGameName = "";
        isMainModeFinish = false;
        nowRound = 1;
        notPlayminiGameSceneName.Add("AvoidFish");
        notPlayminiGameSceneName.Add("CarryToTheGoal");
        notPlayminiGameSceneName.Add("ConsecutiveChases");
        notPlayminiGameSceneName.Add("FlagUp");
        notPlayminiGameSceneName.Add("Let'sPaint");
        notPlayminiGameSceneName.Add("NotFallHole");
        notPlayminiGameSceneName.Add("NotHitStick");
        notPlayminiGameSceneName.Add("Surfboard");
    }

    //ミニゲームシーンに変更
    public static void ChangeMiniGameScene()
    {
        nowMiniGameName = notPlayminiGameSceneName[Random.Range(0, notPlayminiGameSceneName.Count)];
        notPlayminiGameSceneName.Remove(nowMiniGameName);
        SceneManager.LoadScene(nowMiniGameName);
    }

    //現在のラウンド数を取得
    public static int GetNowRound() { return nowRound; }

    //次のラウンドへ
    public static void NextRound()
    {
        //もう最大ラウンド数以上になっているのならばこの先処理しない
        if (nowRound >= MAX_ROUND)
        {
            isMainModeFinish = true;
            return;
        }

        nowRound++; 
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤー情報
public class PlayerInfo
{
    GameObject visual; //プレイヤーの見た目
}

public static class PlayerManager
{

    //プレイヤー情報(key : プレイヤー番号)
    private static Dictionary<byte, PlayerInfo> player;

    //初期化
    public static void Initializ()
    {
        Dictionary<byte, PlayerInfo> player = new Dictionary<byte, PlayerInfo>();
    }

    //プレイヤー情報を設定
    public static void SetPlayerInfo(byte num, PlayerInfo playerInfo)
    {
        //情報があればこの先処理しない
        if (player[num] != null) return;
        
        //セット
        player[num] = playerInfo;
    }

    //プレイヤー情報を取得
    public static PlayerInfo GetPlayerInfo(byte num) { return player[num]; }
}
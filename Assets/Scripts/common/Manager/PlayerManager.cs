using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤー情報
public class PlayerInfo
{
    public GameObject visual;  //プレイヤーの見た目
    public bool isThreePlayer; //3人側プレイヤーかどうか
}

public static class PlayerManager
{

    //プレイヤー情報(key : プレイヤー番号)
    private static Dictionary<byte, PlayerInfo> player;

    //プレイヤー人数
    public const int PLAYER_MAX = 4;

    //初期化
    public static void Initializ()
    {
        player = new Dictionary<byte, PlayerInfo>();

        for (byte i = 1; i < PLAYER_MAX + 1; i++)
        {
            PlayerInfo info = new PlayerInfo();
            info.visual = null;
            info.isThreePlayer = false;
            player[i] = info;
        }
    }

    //プレイヤーの見た目を設定
    public static void SetPlayerVisual(byte num, GameObject playerVisual) { player[num].visual = playerVisual; }

    //1人側プレイヤーの設定
    public static void SetOnePlayer(byte num) 
    {
        for (byte i = 1; i < PLAYER_MAX + 1; i++)
            player[i].isThreePlayer = true;

        player[num].isThreePlayer = false;
    }

    //プレイヤー情報を取得
    public static PlayerInfo GetPlayerInfo(byte num) { return player[num]; }

    //プレイヤーの見た目を取得
    public static GameObject GetPlayerVisual(byte num) { return player[num].visual; }

    //3人側プレイヤー番号取得
    public static List<byte> GetThreePlayer() 
    {
        List<byte> p = new List<byte>();
     
        for (byte i = 1; i < PLAYER_MAX + 1; i++)
            if (player[i].isThreePlayer) p.Add(i);

        return p;
    }

    //1人側プレイヤー番号取得
    public static byte GetOnePlayer()
    {
        for (byte i = 1; i < PLAYER_MAX + 1; i++)
            if (!player[i].isThreePlayer) return i;

        return 0;
    }
}
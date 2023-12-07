using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤー情報
public class PlayerInfo
{
    public string visualPath;       //プレイヤーの見た目
    public string vitualImagePath;  //プレイヤーの見た目画像パス
    public bool isThreePlayer;      //3人側プレイヤーかどうか
}

public static class PlayerManager
{

    //プレイヤー情報(key : プレイヤー番号)
    private static Dictionary<byte, PlayerInfo> player;

    //プレイヤー人数
    public const int PLAYER_MAX = 4;

    //1人側プレイヤーの番号
    private static byte onePlayerNum = 1;

    //初期化
    public static void Initializ()
    {
        player = new Dictionary<byte, PlayerInfo>();
        onePlayerNum = 1;

        for (byte i = 1; i < PLAYER_MAX + 1; i++)
        {
            PlayerInfo info = new PlayerInfo();
            info.visualPath = "";
            info.isThreePlayer = false;
            info.vitualImagePath = "";
            player[i] = info;
        }

        player[1].isThreePlayer = false;
        player[2].isThreePlayer = true;
        player[3].isThreePlayer = true;
        player[4].isThreePlayer = true;

        ///////////////////見た目と画像を手動で設定するので汚くなります(α版のみ)//////////////////////////
        player[1].visualPath = "Slime_01";
        player[2].visualPath = "Slime_01_King";
        player[3].visualPath = "Slime_01_MeltalHelmet";
        player[4].visualPath = "Slime_01_Viking";

        player[1].vitualImagePath = "image_010";
        player[2].vitualImagePath = "image_002";
        player[3].vitualImagePath = "image_003";
        player[4].vitualImagePath = "image_004";


    }
    
    //次の1人側を設定
    public static void NextOnePlayer() {

        player[onePlayerNum].isThreePlayer = true;
        onePlayerNum++;
        if (onePlayerNum > 4) return;

        player[onePlayerNum].isThreePlayer = false;
    }

    //プレイヤーの見た目を設定
    public static void SetPlayerVisual(byte num, string playerVisual) { player[num].visualPath = playerVisual; }

    //プレイヤーの見た目画像を設定
    public static void SetPlayerVisualImage(byte num, string playerVisual) { player[num].vitualImagePath = playerVisual; }

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
    public static string GetPlayerVisual(byte num) { return player[num].visualPath; }

    //プレイヤーの見た目画像を取得
    public static string GetPlayerVisualImage(byte num) { return player[num].vitualImagePath; }

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

    //プレイヤー番号取得
    public static byte GetPlayerNum(string vitualPath)
    {
        for (byte i = 1; i < PLAYER_MAX + 1; i++)
            if (player[i].visualPath == vitualPath) return i;

        return 0;
    }
}
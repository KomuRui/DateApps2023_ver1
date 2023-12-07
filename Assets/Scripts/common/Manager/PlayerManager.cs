using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�v���C���[���
public class PlayerInfo
{
    public string visualPath;       //�v���C���[�̌�����
    public string vitualImagePath;  //�v���C���[�̌����ډ摜�p�X
    public bool isThreePlayer;      //3�l���v���C���[���ǂ���
}

public static class PlayerManager
{

    //�v���C���[���(key : �v���C���[�ԍ�)
    private static Dictionary<byte, PlayerInfo> player;

    //�v���C���[�l��
    public const int PLAYER_MAX = 4;

    //1�l���v���C���[�̔ԍ�
    private static byte onePlayerNum = 1;

    //������
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

        ///////////////////�����ڂƉ摜���蓮�Őݒ肷��̂ŉ����Ȃ�܂�(���ł̂�)//////////////////////////
        player[1].visualPath = "Slime_01";
        player[2].visualPath = "Slime_01_King";
        player[3].visualPath = "Slime_01_MeltalHelmet";
        player[4].visualPath = "Slime_01_Viking";

        player[1].vitualImagePath = "image_010";
        player[2].vitualImagePath = "image_002";
        player[3].vitualImagePath = "image_003";
        player[4].vitualImagePath = "image_004";


    }
    
    //����1�l����ݒ�
    public static void NextOnePlayer() {

        player[onePlayerNum].isThreePlayer = true;
        onePlayerNum++;
        if (onePlayerNum > 4) return;

        player[onePlayerNum].isThreePlayer = false;
    }

    //�v���C���[�̌����ڂ�ݒ�
    public static void SetPlayerVisual(byte num, string playerVisual) { player[num].visualPath = playerVisual; }

    //�v���C���[�̌����ډ摜��ݒ�
    public static void SetPlayerVisualImage(byte num, string playerVisual) { player[num].vitualImagePath = playerVisual; }

    //1�l���v���C���[�̐ݒ�
    public static void SetOnePlayer(byte num) 
    {
        for (byte i = 1; i < PLAYER_MAX + 1; i++)
            player[i].isThreePlayer = true;

        player[num].isThreePlayer = false;
    }

    //�v���C���[�����擾
    public static PlayerInfo GetPlayerInfo(byte num) { return player[num]; }

    //�v���C���[�̌����ڂ��擾
    public static string GetPlayerVisual(byte num) { return player[num].visualPath; }

    //�v���C���[�̌����ډ摜���擾
    public static string GetPlayerVisualImage(byte num) { return player[num].vitualImagePath; }

    //3�l���v���C���[�ԍ��擾
    public static List<byte> GetThreePlayer() 
    {
        List<byte> p = new List<byte>();
     
        for (byte i = 1; i < PLAYER_MAX + 1; i++)
            if (player[i].isThreePlayer) p.Add(i);

        return p;
    }

    //1�l���v���C���[�ԍ��擾
    public static byte GetOnePlayer()
    {
        for (byte i = 1; i < PLAYER_MAX + 1; i++)
            if (!player[i].isThreePlayer) return i;

        return 0;
    }

    //�v���C���[�ԍ��擾
    public static byte GetPlayerNum(string vitualPath)
    {
        for (byte i = 1; i < PLAYER_MAX + 1; i++)
            if (player[i].visualPath == vitualPath) return i;

        return 0;
    }
}
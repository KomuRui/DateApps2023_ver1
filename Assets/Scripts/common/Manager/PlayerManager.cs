using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�v���C���[���
public class PlayerInfo
{
    public GameObject visual;  //�v���C���[�̌�����
    public bool isThreePlayer; //3�l���v���C���[���ǂ���
}

public static class PlayerManager
{

    //�v���C���[���(key : �v���C���[�ԍ�)
    private static Dictionary<byte, PlayerInfo> player;

    //�v���C���[�l��
    public const int PLAYER_MAX = 4;

    //������
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

    //�v���C���[�̌����ڂ�ݒ�
    public static void SetPlayerVisual(byte num, GameObject playerVisual) { player[num].visual = playerVisual; }

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
    public static GameObject GetPlayerVisual(byte num) { return player[num].visual; }

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
}
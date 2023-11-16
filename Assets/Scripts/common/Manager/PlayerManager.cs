using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�v���C���[���
public class PlayerInfo
{
    GameObject visual; //�v���C���[�̌�����
}

public static class PlayerManager
{

    //�v���C���[���(key : �v���C���[�ԍ�)
    private static Dictionary<byte, PlayerInfo> player;

    //������
    public static void Initializ()
    {
        Dictionary<byte, PlayerInfo> player = new Dictionary<byte, PlayerInfo>();
    }

    //�v���C���[����ݒ�
    public static void SetPlayerInfo(byte num, PlayerInfo playerInfo)
    {
        //��񂪂���΂��̐揈�����Ȃ�
        if (player[num] != null) return;
        
        //�Z�b�g
        player[num] = playerInfo;
    }

    //�v���C���[�����擾
    public static PlayerInfo GetPlayerInfo(byte num) { return player[num]; }
}
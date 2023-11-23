using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreManager
{
    //�v���C���[�X�R�A(key : �v���C���[�ԍ�)
    private static Dictionary<byte, int> score;

    //�X�R�A�\(���ʂɂ���ĉ��|�C���g�擾���邩)
    private static Dictionary<byte, byte> scoreTable;

    //������
    public static void Initializ()
    {
        score = new Dictionary<byte, int>();
        scoreTable = new Dictionary<byte, byte>();

        for (byte i = 1; i < PlayerManager.PLAYER_MAX + 1; i++)
            score[i] = 0;

        //�X�R�A�\������
        scoreTable[1] = 4;
        scoreTable[2] = 3;
        scoreTable[3] = 2;
        scoreTable[4] = 1;
    }

    //�X�R�A�擾
    public static int GetScore(byte numPlayer) { return score[numPlayer]; }

    //�X�R�A���Z
    public static void AddScore(byte numPlayer,byte rank) 
    {
        score[numPlayer] += scoreTable[rank];

        //1�l����1�ʂ̏ꍇ��2�_���Z����
        if (PlayerManager.GetOnePlayer() == numPlayer && rank == 1) score[numPlayer] += 2;

        //���ʂ�ݒ�
        GameManager.nowMiniGameManager.nowMiniGameRank[numPlayer] = rank;
    }

}

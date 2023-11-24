using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//�v���C�������N�[���
public class PlayerRank
{
    public byte rank; //����
    public int score; //�|�C���g��
}

public static class ScoreManager
{
    //�v���C���[�X�R�A(key : �v���C���[�ԍ�)
    private static Dictionary<byte, PlayerRank> score;

    //�X�R�A�\(���ʂɂ���ĉ��|�C���g�擾���邩)
    private static Dictionary<byte, byte> scoreTable;

    //������
    public static void Initializ()
    {
        score = new Dictionary<byte, PlayerRank>();
        scoreTable = new Dictionary<byte, byte>();

        for (byte i = 1; i < PlayerManager.PLAYER_MAX + 1; i++)
        {
            PlayerRank rank = new PlayerRank();
            rank.rank = 1;
            rank.score = 0;
            score[i] = rank;
        }

        //�X�R�A�\������
        scoreTable[1] = 4;
        scoreTable[2] = 3;
        scoreTable[3] = 2;
        scoreTable[4] = 1;
    }

    //���ʂ��Čv�Z����
    public static void ReCalcRank()
    {
        //3�l���̓��_���~���\�[�g�ŕ��ѕς���
        var dict = new Dictionary<byte, int>();
        for (int i = 0; i < PlayerManager.PLAYER_MAX; i++)
            dict.Add(score.Keys.ElementAt(i), score.Values.ElementAt(i).score);

        var sortedDictionary = dict.OrderByDescending(pair => pair.Value);

        //���ʂ��m�F
        byte nowRank = 0;
        byte sameRank = 1;
        float beforeValue = -1;
        foreach (var item in sortedDictionary)
        {
            if (beforeValue != item.Value)
            {
                nowRank += sameRank;
                sameRank = 1;
            }
            else
                sameRank++;

            beforeValue = item.Value;
            score[item.Key].rank = nowRank;
        }
    }


    //�X�R�A�擾
    public static int GetScore(byte numPlayer) { return score[numPlayer].score; }

    //���ʎ擾
    public static int GetRank(byte numPlayer) { return score[numPlayer].rank; }

    //���ʂɂ���Ċl������|�C���g���擾
    public static int GetRankScore(byte numPlayer,byte rank) 
    {
        int point = scoreTable[rank];

        //1�l����1�ʂ̏ꍇ��2�_���Z����
        if (PlayerManager.GetOnePlayer() == numPlayer && rank == 1) point += 2;

        return point; 
    }

    //�X�R�A���Z
    public static void AddScore(byte numPlayer,byte rank) 
    {
        score[numPlayer].score += scoreTable[rank];

        //1�l����1�ʂ̏ꍇ��2�_���Z����
        if (PlayerManager.GetOnePlayer() == numPlayer && rank == 1) score[numPlayer].score += 2;

        //���ʂ�ݒ�
        GameManager.nowMiniGameManager.nowMiniGameRank[numPlayer] = rank;
    }

}
